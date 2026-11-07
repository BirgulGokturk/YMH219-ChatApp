namespace YMH219_ChatApp.Models
{
    /// <summary>
    /// Sunucuya bağlı olan istemcinin temel bilgilerini saklayan model.
    /// </summary>
    public class ClientInfo
    {
        public string Username { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public DateTime ConnectedAt { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Username} ({EndPoint}) - Giriş: {ConnectedAt:HH:mm:ss}";
        }
    }
}
