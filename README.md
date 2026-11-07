# YMH219 - İstemci-Sunucu Chat Uygulaması

Bu proje, Fırat Üniversitesi **Nesne Tabanlı Programlama (YMH 219)** dersi kapsamında Windows Forms ortamında C# dili kullanılarak geliştirilmiş çok kullanıcılı bir TCP/IP istemci-sunucu (Client-Server) sohbet uygulamasıdır.

---

## 📌 Projenin Amacı ve Kısa Açıklama

Projenin temel amacı; ağ programlama kavramlarını (TCP/IP, soket haberleşmesi), `TcpListener` ve `TcpClient` sınıflarını, asenkron işlemleri (`async`/`await`) ve arayüzün eş zamanlı thread güvenli güncellenmesini nesne yönelimli mimariyle uygulamaktır. Uygulama aynı form üzerinde hem sunucu (Server) yönetimini hem de istemci (Client) sohbet modunu barındırır.

---

## 🛠️ Kullanılan Teknolojiler

- **Programlama Dili:** C# (.NET 10.0 / Windows Desktop SDK)
- **Ağ İletişimi:** `System.Net.Sockets` (TcpListener, TcpClient, NetworkStream, StreamReader/StreamWriter)
- **Veri Paketleme:** `System.Text.Json` (JSON serileştirme ile güvenilir paket iletimi)
- **Arayüz Çerçevesi:** Windows Forms (WinForms, TabControl, RichTextBox, ListView)
- **Sürüm Kontrolü:** Git & GitHub

---

## 🚀 Kurulum ve Çalıştırma Adımları

1. Proje dizinine gidin:
   ```bash
   cd YMH219-ChatApp
   ```
2. Projeyi derleyin ve çalıştırın:
   ```bash
   dotnet build
   dotnet run
   ```
   *Veya `YMH219_ChatApp.sln` dosyasını Visual Studio ile açarak F5 tuşu ile başlatabilirsiniz.*

### Test Senaryosu (Aynı Bilgisayarda Test Etme):
1. Uygulamayı başlatın ve **"🖥️ Sunucu Yönetimi"** sekmesine geçerek **"Sunucuyu Başlat"** butonuna tıklayın (Varsayılan Port: `8888`).
2. **"💬 Sohbet İstemcisi"** sekmesine geçin, kullanıcı adınızı yazın ve **"Bağlan"** butonuna tıklayın.
3. İkinci bir istemciyi test etmek için projenin `.exe` dosyasını veya yeni bir terminalden `dotnet run` komutunu tekrar çalıştırın; farklı bir kullanıcı adı (örn: `Ahmet`) ile bağlanarak karşılıklı mesajlaşabilirsiniz.

---

## ⚙️ Uygulamanın Temel Özellikleri

### 1. Asgari Gereksinimler
- **TCP/IP Ağ İletişimi:** `TcpListener` ile yerel port dinleme ve `TcpClient` ile asenkron soket bağlantısı.
- **Kullanıcı Adı ile Bağlantı:** İstemciler rumuz/kullanıcı adı belirterek odaya giriş yapar; kullanıcı adı çakışması sunucu tarafından otomatik yönetilir.
- **Zaman Bilgili Mesajlaşma:** Alınan ve gönderilen tüm mesajlar saat/dakika/saniye zaman damgasıyla gösterilir.
- **Bağlantı Kontrolü:** "Bağlan", "Mesaj Gönder" ve "Bağlantıyı Kes" fonksiyonları mevcuttur.
- **Hata Yönetimi:** Ağ kopmaları veya geçersiz port/IP girişleri uygulamanın çökmesine yol açmadan kullanıcıya anlaşılır diyaloglarla bildirilir.
- **Bağlı İstemcileri Listeleme:** Sunucu ekranında bağlı tüm istemcilerin kullanıcı adı, IP:Port adresi ve katılım zamanı anlık olarak listelenir.

### 2. İsteğe Bağlı / Ek Geliştirmeler (Bonus)
- 👥 **Çoklu İstemci ve Genel Yayın (Broadcast):** Birden fazla istemci aynı odaya katılarak eş zamanlı grup sohbeti gerçekleştirebilir.
- 🔒 **Özel Mesaj (Whisper / Private Message):** Çevrimiçi kullanıcılar listesinden bir isme çift tıklayarak veya `@KullanıcıAdı mesaj` formatında yalnızca seçilen kişiye özel mesaj gönderilebilir.
- 💾 **Sohbet Geçmişini Kaydetme (.txt):** "Geçmişi Kaydet" butonu ile yapılan tüm yazışmalar tarih/saat damgalı olarak yerel bir metin dosyasına aktarılabilir.
- 📢 **Sunucu Genel Anonsu:** Sunucu yöneticisi bağlı tüm istemcilere genel sistem anonsu gönderebilir.
- 🎨 **Renkli Mesajlaşma:** Kullanıcının kendi mesajları, diğer kullanıcıların mesajları, katılım/ayrılma bildirimleri ve özel mesajlar farklı renk ve stillerde görüntülenir.

---

## 📸 Ekran Görüntüleri

<!-- Buraya uygulamanın ekran görüntülerini ekleyebilirsiniz (screenshots klasörüne ekran1.png ve ekran2.png olarak kaydedebilirsiniz) -->

| Sohbet İstemcisi Ekranı | Sunucu Yönetimi ve Bağlı İstemciler |
|:---:|:---:|
| ![Ekran Görüntüsü 1](screenshots/ekran1.png) | ![Ekran Görüntüsü 2](screenshots/ekran2.png) |

*(Not: `screenshots/` klasörüne `ekran1.png` ve `ekran2.png` isimli ekran görüntülerinizi eklediğinizde yukarıdaki tabloda otomatik olarak görünecektir.)*

---

## 🏗️ Nesne Tabanlı Mimari

```
YMH219-ChatApp/
├── Models/
│   ├── MessageType.cs       # Mesaj türleri (Chat, Private, Join, Leave, UserList, System)
│   ├── ChatMessage.cs       # Ağ üzerinden aktarılan JSON veri paketi modeli
│   └── ClientInfo.cs        # Sunucuda listelenen istemci bilgileri modeli
├── Network/
│   ├── ChatServer.cs        # Çoklu istemci yönetimi, dinleme döngüsü ve broadcast sunucusu
│   └── ChatClient.cs        # Asenkron TCP bağlantısı ve mesajlaşma istemcisi
├── Services/
│   └── ChatHistoryManager.cs# Mesaj geçmişi yönetimi ve .txt dışa aktarma servisi
├── screenshots/
│   └── .gitkeep             # Ekran görüntüleri klasörü
├── Form1.cs                 # Form olayları, thread senkronizasyonu ve arayüz mantığı
├── Form1.Designer.cs        # İstemci ve Sunucu sekmeli modern WinForms tasarımı
├── Program.cs               # Giriş noktası
├── YMH219_ChatApp.csproj
└── YMH219_ChatApp.sln
```

---

## 📝 Bilinen Sınırlamalar / Geliştirme Notları

- İletişim yerel ağ (LAN) veya yerel bilgisayar (localhost - 127.0.0.1) üzerinde çalışmak üzere optimize edilmiştir.
- Gelecek versiyonlarda uçtan uca şifreleme (AES) ve dosya/resim transferi özellikleri eklenebilir.

---

## 👤 Öğrenci Bilgileri

- **Adı Soyadı:** Birgül Göktürk
- **Öğrenci Numarası:** 250541094
- **Ders:** Nesne Tabanlı Programlama (YMH 219)
- **Dönem:** 2026-2027 Güz
