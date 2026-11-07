using System.Net;
using System.Net.Sockets;
using System.Text;
using YMH219_ChatApp.Models;

namespace YMH219_ChatApp.Network
{
    /// <summary>
    /// Çoklu istemci bağlantılarını, mesaj yönlendirmeyi ve yayınları (broadcast) yöneten TCP Sunucu sınıfı.
    /// </summary>
    public class ChatServer
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
        private readonly List<ConnectedClientSession> _clients = new();
        private readonly object _lock = new();

        public bool IsRunning { get; private set; } = false;
        public int Port { get; private set; }

        public event Action<ChatMessage>? MessageReceived;
        public event Action<ClientInfo>? ClientConnected;
        public event Action<ClientInfo>? ClientDisconnected;
        public event Action<string>? ServerLog;
        public event Action<bool>? StateChanged;

        /// <summary>
        /// Sunucuyu belirtilen portta dinlemeye başlatır.
        /// </summary>
        public async Task StartAsync(int port)
        {
            if (IsRunning) return;

            Port = port;
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            IsRunning = true;

            StateChanged?.Invoke(true);
            ServerLog?.Invoke($"[SUNUCU] Dinleme başlatıldı. Port: {port}");

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    TcpClient tcpClient = await _listener.AcceptTcpClientAsync(_cts.Token);
                    _ = HandleIncomingClientAsync(tcpClient, _cts.Token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                ServerLog?.Invoke($"[HATA] Sunucu dinleme hatası: {ex.Message}");
            }
            finally
            {
                Stop();
            }
        }

        /// <summary>
        /// Sunucuyu durdurur ve tüm bağlı istemcileri kapatır.
        /// </summary>
        public void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;
            _cts?.Cancel();

            lock (_lock)
            {
                foreach (var client in _clients.ToList())
                {
                    client.Dispose();
                }
                _clients.Clear();
            }

            try
            {
                _listener?.Stop();
            }
            catch { }

            StateChanged?.Invoke(false);
            ServerLog?.Invoke("[SUNUCU] Sunucu durduruldu.");
        }

        private async Task HandleIncomingClientAsync(TcpClient tcpClient, CancellationToken token)
        {
            ConnectedClientSession? session = null;
            string remoteEndpoint = tcpClient.Client.RemoteEndPoint?.ToString() ?? "Bilinmiyor";

            try
            {
                var stream = tcpClient.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                // İlk mesaj istemcinin kullanıcı adı ve Join paketidir
                string? firstLine = await reader.ReadLineAsync(token);
                if (string.IsNullOrWhiteSpace(firstLine))
                {
                    tcpClient.Close();
                    return;
                }

                var joinMsg = ChatMessage.FromJson(firstLine);
                string username = joinMsg?.Sender ?? $"Kullanıcı_{Random.Shared.Next(1000, 9999)}";

                // Kullanıcı adı çakışma kontrolü
                lock (_lock)
                {
                    if (_clients.Any(c => c.Info.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                    {
                        username = $"{username}_{Random.Shared.Next(10, 99)}";
                    }

                    session = new ConnectedClientSession(tcpClient, reader, writer, new ClientInfo
                    {
                        Username = username,
                        EndPoint = remoteEndpoint,
                        ConnectedAt = DateTime.Now
                    });

                    _clients.Add(session);
                }

                ClientConnected?.Invoke(session.Info);
                ServerLog?.Invoke($"[BAĞLANTI] {session.Info.Username} bağlandı ({remoteEndpoint})");

                // Odaya katılım mesajını herkese bildir
                var welcomeMsg = new ChatMessage("Sunucu", $"{session.Info.Username} odaya katıldı.", MessageType.Join);
                await BroadcastAsync(welcomeMsg);
                await BroadcastUserListAsync();

                // İstemciden sürekli mesaj dinleme döngüsü
                while (!token.IsCancellationRequested && tcpClient.Connected)
                {
                    string? line = await reader.ReadLineAsync(token);
                    if (line == null) break;

                    var incoming = ChatMessage.FromJson(line);
                    if (incoming == null) continue;

                    incoming.Sender = session.Info.Username; // Göndereni garanti et
                    MessageReceived?.Invoke(incoming);

                    if (incoming.Type == MessageType.Private && !string.IsNullOrWhiteSpace(incoming.Recipient))
                    {
                        // Özel mesaj yönlendirme
                        await SendPrivateMessageAsync(incoming);
                    }
                    else
                    {
                        // Genel odaya broadcast
                        await BroadcastAsync(incoming);
                    }
                }
            }
            catch (Exception)
            {
                // Bağlantı koptu veya hata oluştu
            }
            finally
            {
                if (session != null)
                {
                    lock (_lock)
                    {
                        _clients.Remove(session);
                    }

                    ClientDisconnected?.Invoke(session.Info);
                    ServerLog?.Invoke($"[AYRILMA] {session.Info.Username} ayrıldı.");
                    session.Dispose();

                    var leaveMsg = new ChatMessage("Sunucu", $"{session.Info.Username} sohbetten ayrıldı.", MessageType.Leave);
                    _ = BroadcastAsync(leaveMsg);
                    _ = BroadcastUserListAsync();
                }
            }
        }

        /// <summary>
        /// Mesajı tüm bağlı istemcilere dağıtır.
        /// </summary>
        public async Task BroadcastAsync(ChatMessage message)
        {
            string json = message.ToJson();
            List<ConnectedClientSession> targets;

            lock (_lock)
            {
                targets = _clients.ToList();
            }

            foreach (var client in targets)
            {
                try
                {
                    await client.Writer.WriteLineAsync(json);
                }
                catch
                {
                    // Yazma hatası durumunda istemci döngüsü zaten kapanacaktır
                }
            }
        }

        /// <summary>
        /// Belirli bir kullanıcıya özel mesaj gönderir (aynı zamanda gönderene de bir kopyası iletilir).
        /// </summary>
        public async Task SendPrivateMessageAsync(ChatMessage message)
        {
            string json = message.ToJson();
            ConnectedClientSession? senderSession = null;
            ConnectedClientSession? targetSession = null;

            lock (_lock)
            {
                senderSession = _clients.FirstOrDefault(c => c.Info.Username == message.Sender);
                targetSession = _clients.FirstOrDefault(c => c.Info.Username.Equals(message.Recipient, StringComparison.OrdinalIgnoreCase));
            }

            if (targetSession != null)
            {
                try { await targetSession.Writer.WriteLineAsync(json); } catch { }
                if (senderSession != null && senderSession != targetSession)
                {
                    try { await senderSession.Writer.WriteLineAsync(json); } catch { }
                }
            }
            else if (senderSession != null)
            {
                var errorMsg = new ChatMessage("Sunucu", $"Hedef kullanıcı bulunamadı: {message.Recipient}", MessageType.System);
                try { await senderSession.Writer.WriteLineAsync(errorMsg.ToJson()); } catch { }
            }
        }

        /// <summary>
        /// Güncel çevrimiçi kullanıcı listesini tüm istemcilere bildirir.
        /// </summary>
        private async Task BroadcastUserListAsync()
        {
            List<string> usernames;
            lock (_lock)
            {
                usernames = _clients.Select(c => c.Info.Username).ToList();
            }

            string userListCsv = string.Join(",", usernames);
            var listMessage = new ChatMessage("Sunucu", userListCsv, MessageType.UserList);
            await BroadcastAsync(listMessage);
        }

        /// <summary>
        /// Sunucuya bağlı tüm istemcilerin anlık listesini döndürür.
        /// </summary>
        public IReadOnlyList<ClientInfo> GetConnectedClients()
        {
            lock (_lock)
            {
                return _clients.Select(c => c.Info).ToList().AsReadOnly();
            }
        }

        private class ConnectedClientSession : IDisposable
        {
            public TcpClient Client { get; }
            public StreamReader Reader { get; }
            public StreamWriter Writer { get; }
            public ClientInfo Info { get; }

            public ConnectedClientSession(TcpClient client, StreamReader reader, StreamWriter writer, ClientInfo info)
            {
                Client = client;
                Reader = reader;
                Writer = writer;
                Info = info;
            }

            public void Dispose()
            {
                try { Writer.Dispose(); } catch { }
                try { Reader.Dispose(); } catch { }
                try { Client.Dispose(); } catch { }
            }
        }
    }
}
