namespace YMH219_ChatApp.Models
{
    /// <summary>
    /// Kullanıcı adı kurallarını tek bir yerde toplar. İstemci formu bu kurallarla
    /// kullanıcıyı uyarır; sunucu ise gelen adı aynı kurallarla güvenli hale getirir.
    /// </summary>
    public static class UsernameRules
    {
        public const int MaxLength = 20;

        // Sunucunun kendi mesajlarında kullandığı ad; kullanıcılar bu adla sunucuyu taklit edemez
        private const string ReservedPrefix = "Sunucu";

        /// <summary>
        /// Kullanıcı adının kurallara uyup uymadığını kontrol eder.
        /// Uymuyorsa kullanıcıya gösterilecek hata mesajını döndürür.
        /// </summary>
        public static bool TryValidate(string? username, out string error)
        {
            string name = username?.Trim() ?? string.Empty;

            if (name.Length == 0)
            {
                error = "Lütfen bir kullanıcı adı giriniz.";
                return false;
            }

            if (name.Length > MaxLength)
            {
                error = $"Kullanıcı adı en fazla {MaxLength} karakter olabilir.";
                return false;
            }

            // Çevrimiçi kullanıcı listesi virgülle ayrılarak gönderildiği için virgül kullanılamaz
            if (name.Contains(','))
            {
                error = "Kullanıcı adı virgül (,) içeremez.";
                return false;
            }

            if (name.StartsWith(ReservedPrefix, StringComparison.OrdinalIgnoreCase))
            {
                error = $"\"{ReservedPrefix}\" ile başlayan adlar sunucu mesajları için ayrılmıştır.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        /// <summary>
        /// Sunucu tarafında kullanılır: istemciden gelen adı kurallara uygun hale getirir.
        /// Düzeltilemeyen adların yerine rastgele bir ad üretir.
        /// </summary>
        public static string Normalize(string? username)
        {
            string name = (username ?? string.Empty).Replace(",", string.Empty).Trim();

            if (name.Length > MaxLength)
            {
                name = name[..MaxLength];
            }

            if (!TryValidate(name, out _))
            {
                name = $"Kullanıcı_{Random.Shared.Next(1000, 9999)}";
            }

            return name;
        }
    }
}
