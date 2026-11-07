using System.Text.Json;

namespace YMH219_ChatApp.Models
{
    /// <summary>
    /// Ağ üzerinden istemci ve sunucu arasında iletilen veri paketini temsil eden sınıf.
    /// </summary>
    public class ChatMessage
    {
        public string Sender { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public MessageType Type { get; set; } = MessageType.Chat;
        public string? Recipient { get; set; } = null; // Özel mesajlar için hedef kullanıcı

        public ChatMessage() { }

        public ChatMessage(string sender, string content, MessageType type = MessageType.Chat, string? recipient = null)
        {
            Sender = sender;
            Content = content;
            Timestamp = DateTime.Now;
            Type = type;
            Recipient = recipient;
        }

        /// <summary>
        /// Mesajı ağ üzerinden iletilmek üzere JSON dizesine dönüştürür.
        /// </summary>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// JSON dizesini ChatMessage nesnesine dönüştürür.
        /// </summary>
        public static ChatMessage? FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ChatMessage>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Arayüzde veya logda gösterilecek biçimlendirilmiş metin.
        /// </summary>
        public string ToFormattedString()
        {
            string time = Timestamp.ToString("HH:mm:ss");

            return Type switch
            {
                MessageType.Join => $"[{time}] [BİLGİ] {Sender} sohbete katıldı.",
                MessageType.Leave => $"[{time}] [BİLGİ] {Sender} sohbetten ayrıldı.",
                MessageType.System => $"[{time}] [SİSTEM] {Content}",
                MessageType.Private => $"[{time}] [ÖZEL -> {Recipient}] {Sender}: {Content}",
                _ => $"[{time}] {Sender}: {Content}"
            };
        }
    }
}
