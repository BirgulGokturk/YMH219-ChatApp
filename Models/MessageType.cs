namespace YMH219_ChatApp.Models
{
    /// <summary>
    /// Ağ üzerinden iletilen mesajın niteliğini belirten tür.
    /// </summary>
    public enum MessageType
    {
        Chat,         // Normal oda mesajı
        Private,      // İki kullanıcı arasında özel mesaj
        Join,         // Odaya yeni bir kullanıcı katıldı
        Leave,        // Kullanıcı odadan ayrıldı
        UserList,     // Sunucudan istemcilere çevrimiçi kullanıcı listesi bildirimi
        System        // Sunucu sistem/bilgi mesajı
    }
}
