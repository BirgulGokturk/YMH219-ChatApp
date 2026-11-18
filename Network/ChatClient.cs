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
        private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(5);

        private TcpClient? _tcpClient;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private CancellationTokenSource? _cts;

        // Disconnect hem arayüzden hem de arka plandaki dinleme görevinden çağrılabilir.
        // Bağlantı alanları bu kilit altında değiştirilir.
        private readonly object _stateLock = new();

        // Kullanıcı art arda hızlıca Enter'a basarsa iki gönderim aynı anda StreamWriter'a
        // yazmaya çalışabilir; bu kilit gönderimleri sıraya alır.
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        public bool IsConnected
        {
            get
            {
                var client = _tcpClient;
                return client != null && client.Connected;
            }
        }

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

                // Ulaşılamayan bir IP adresinde uzun süre beklememek için zaman aşımı uygulanır
                using (var timeoutCts = new CancellationTokenSource(ConnectTimeout))
                {
                    await _tcpClient.ConnectAsync(host, port, timeoutCts.Token);
                }

                var stream = _tcpClient.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                _reader = reader;
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                _cts = new CancellationTokenSource();

                // İlk olarak odaya katılım (Join) paketini sunucuya gönder
                var joinMsg = new ChatMessage(Username, $"{Username} bağlandı.", MessageType.Join);
                await _writer.WriteLineAsync(joinMsg.ToJson());

                ConnectionStateChanged?.Invoke(true);

                // Arka planda gelen mesajları dinlemeye başla
                _ = ListenMessagesAsync(reader, _cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                Disconnect();
                ErrorOccurred?.Invoke(
                    $"Sunucuya bağlanılamadı: {host}:{port} adresinden {ConnectTimeout.TotalSeconds:0} saniye içinde yanıt alınamadı (zaman aşımı).\n\n" +
                    "IP adresini ve sunucunun aynı ağda çalıştığını kontrol edin.");
                return false;
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Disconnect();
                ErrorOccurred?.Invoke(
                    $"Sunucuya bağlanılamadı: {host}:{port} adresinde çalışan bir sunucu bulunamadı (bağlantı reddedildi).\n\n" +
                    "Önce 'Sunucu Yönetimi' sekmesinden sunucuyu başlattığınızdan ve port numarasının aynı olduğundan emin olun.");
                return false;
            }
            catch (Exception ex)
            {
                Disconnect();
                ErrorOccurred?.Invoke($"Sunucuya bağlanılamadı: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sunucudan gelen mesajları sürekli okur. Bağlantı kullanıcı isteği dışında
        /// koparsa (ör. sunucu kapandıysa) bunu ErrorOccurred olayı ile bildirir.
        /// </summary>
        private async Task ListenMessagesAsync(StreamReader reader, CancellationToken token)
        {
            string? disconnectReason = null;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    string? line = await reader.ReadLineAsync(token);
                    if (line == null)
                    {
                        // Karşı taraf bağlantıyı düzgün şekilde kapattı
                        disconnectReason = "Sunucu bağlantıyı kapattı. Sunucu durdurulmuş olabilir.";
                        break;
                    }

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
            catch (OperationCanceledException)
            {
                // Kullanıcı 'Bağlantıyı Kes' butonuna bastı; bildirim gerekmez
            }
            catch (Exception ex)
            {
                disconnectReason = $"Sunucu ile bağlantı beklenmedik şekilde koptu: {ex.Message}";
            }
            finally
            {
                // Disconnect token'ı iptal ettiği için, kullanıcı kaynaklı olup olmadığını önce kaydet
                bool closedByUser = token.IsCancellationRequested;

                Disconnect();

                if (!closedByUser && disconnectReason != null)
                {
                    ErrorOccurred?.Invoke(disconnectReason);
                }
            }
        }

        /// <summary>
        /// Odaya veya özel kullanıcıya mesaj gönderir.
        /// </summary>
        public async Task<bool> SendMessageAsync(string content, string? recipient = null)
        {
            if (!IsConnected)
            {
                ErrorOccurred?.Invoke("Mesaj gönderilemedi: Sunucuya bağlı değilsiniz.");
                return false;
            }

            await _sendLock.WaitAsync();
            try
            {
                // Bekleme sırasında bağlantı kesilmiş olabilir
                var writer = _writer;
                if (writer == null)
                {
                    ErrorOccurred?.Invoke("Mesaj gönderilemedi: Sunucuya bağlı değilsiniz.");
                    return false;
                }

                var type = string.IsNullOrWhiteSpace(recipient) ? MessageType.Chat : MessageType.Private;
                var msg = new ChatMessage(Username, content, type, recipient);
                await writer.WriteLineAsync(msg.ToJson());
                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"Mesaj gönderilirken hata oluştu: {ex.Message}");
                Disconnect();
                return false;
            }
            finally
            {
                _sendLock.Release();
            }
        }

        /// <summary>
        /// Sunucu bağlantısını güvenli şekilde sonlandırır. Birden fazla kez veya farklı
        /// thread'lerden çağrılması güvenlidir; kapatma işlemi yalnızca bir kez yapılır.
        /// </summary>
        public void Disconnect()
        {
            TcpClient? client;
            StreamReader? reader;
            StreamWriter? writer;
            CancellationTokenSource? cts;

            lock (_stateLock)
            {
                if (_tcpClient == null) return;

                client = _tcpClient;
                reader = _reader;
                writer = _writer;
                cts = _cts;

                _tcpClient = null;
                _reader = null;
                _writer = null;
                _cts = null;
            }

            try { cts?.Cancel(); } catch { }
            try { writer?.Dispose(); } catch { }
            try { reader?.Dispose(); } catch { }
            try { client.Close(); } catch { }

            ConnectionStateChanged?.Invoke(false);
        }
    }
}
