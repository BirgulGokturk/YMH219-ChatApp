using System.Text;
using YMH219_ChatApp.Models;

namespace YMH219_ChatApp.Services
{
    /// <summary>
    /// Sohbet mesaj geçmişini saklayan ve yerel metin dosyasına kaydeden servis.
    /// </summary>
    public class ChatHistoryManager
    {
        private readonly List<ChatMessage> _messages = new();
        private readonly object _lock = new();

        public IReadOnlyList<ChatMessage> Messages
        {
            get
            {
                lock (_lock)
                {
                    return _messages.ToList().AsReadOnly();
                }
            }
        }

        public void Add(ChatMessage message)
        {
            lock (_lock)
            {
                _messages.Add(message);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _messages.Clear();
            }
        }

        /// <summary>
        /// Tüm sohbet geçmişini biçimlendirilmiş bir metin (.txt) dosyasına kaydeder.
        /// </summary>
        public void ExportToFile(string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

                writer.WriteLine("================================================================================");
                writer.WriteLine("                       YMH219 - CHAT SOHBET GEÇMİŞİ                             ");
                writer.WriteLine("================================================================================");
                writer.WriteLine($"Oluşturulma Tarihi : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Toplam Mesaj Sayısı: {_messages.Count}");
                writer.WriteLine("================================================================================");
                writer.WriteLine();

                lock (_lock)
                {
                    foreach (var msg in _messages)
                    {
                        writer.WriteLine(msg.ToFormattedString());
                    }
                }

                writer.WriteLine();
                writer.WriteLine("============================= SOHBET SONU ======================================");
            }
            catch (Exception ex)
            {
                throw new IOException($"Sohbet geçmişi kaydedilirken hata oluştu: {ex.Message}", ex);
            }
        }
    }
}
