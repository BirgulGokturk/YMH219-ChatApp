using System.Net.Sockets;
using System.Text;
using YMH219_ChatApp.Models;

namespace YMH219_ChatApp.Network
{
    /// <summary>
    /// Sunucuya TCP üzerinden bağlanan, mesaj alıp gönderen istemci sınıfı.
    /// </summary>
    public class ChatClient
    {
        private TcpClient? _tcpClient;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private CancellationTokenSource? _cts;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;
        public string Username { get; private set; } = string.Empty;

        public event Action<ChatMessage>? MessageReceived;
        public event Action<List<string>>? UserListUpdated;
        public event Action<bool>? ConnectionStateChanged;
        public event Action<string>? ErrorOccurred;

        /// <summary>
        /// Belirtilen IP ve Port üzerinden sunucuya bağlanır.
        /// </summary>
        public async Task<bool> ConnectAsync(string host, int port, string username)
        {
            if (IsConnected) return true;

            try
            {
                Username = username.Trim();
                _tcpClient = new TcpClient();

                // Asenkron bağlantı kur
                await _tcpClient.ConnectAsync(host, port);

                var stream = _tcpClient.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                _cts = new CancellationTokenSource();

                // İlk olarak odaya katılım (Join) paketini sunucuya gönder
                var joinMsg = new ChatMessage(Username, $"{Username} bağlandı.", MessageType.Join);
                await _writer.WriteLineAsync(joinMsg.ToJson());

                ConnectionStateChanged?.Invoke(true);

                // Arka planda gelen mesajları dinlemeye başla
                _ = ListenMessagesAsync(_cts.Token);
                return true;
            }
            catch (Exception ex)
            {
                Disconnect();
                ErrorOccurred?.Invoke($"Sunucuya bağlanılamadı: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sunucudan gelen mesajları sürekli okur.
        /// </summary>
        private async Task ListenMessagesAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _reader != null)
                {
                    string? line = await _reader.ReadLineAsync(token);
                    if (line == null) break;

                    var message = ChatMessage.FromJson(line);
                    if (message == null) continue;

                    if (message.Type == MessageType.UserList)
                    {
                        // Çevrimiçi kullanıcılar listesi güncellendi
                        var users = message.Content.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                        UserListUpdated?.Invoke(users);
                    }
                    else
                    {
                        // Normal, özel veya sistem mesajı
                        MessageReceived?.Invoke(message);
                    }
                }
            }
            catch (Exception)
            {
                // Bağlantı koptuğunda sessizce döngüden çık
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Odaya veya özel kullanıcıya mesaj gönderir.
        /// </summary>
        public async Task<bool> SendMessageAsync(string content, string? recipient = null)
        {
            if (!IsConnected || _writer == null)
            {
                ErrorOccurred?.Invoke("Mesaj gönderilemedi: Sunucuya bağlı değilsiniz.");
                return false;
            }

            try
            {
                var type = string.IsNullOrWhiteSpace(recipient) ? MessageType.Chat : MessageType.Private;
                var msg = new ChatMessage(Username, content, type, recipient);
                await _writer.WriteLineAsync(msg.ToJson());
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Mesaj gönderilirken hata oluştu: {ex.Message}");
                Disconnect();
                return false;
            }
        }

        /// <summary>
        /// Sunucu bağlantısını güvenli şekilde sonlandırır.
        /// </summary>
        public void Disconnect()
        {
            if (_tcpClient == null) return;

            _cts?.Cancel();

            try { _writer?.Dispose(); } catch { }
            try { _reader?.Dispose(); } catch { }
            try { _tcpClient?.Close(); } catch { }

            _writer = null;
            _reader = null;
            _tcpClient = null;

            ConnectionStateChanged?.Invoke(false);
        }
    }
}
