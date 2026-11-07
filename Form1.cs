using YMH219_ChatApp.Models;
using YMH219_ChatApp.Network;
using YMH219_ChatApp.Services;

namespace YMH219_ChatApp;

public partial class Form1 : Form
{
    private readonly ChatServer _server = new();
    private readonly ChatClient _client = new();
    private readonly ChatHistoryManager _history = new();

    public Form1()
    {
        InitializeComponent();

        SetupServerEvents();
        SetupClientEvents();
    }

    #region Sunucu Yönetimi ve Olayları

    private void SetupServerEvents()
    {
        _server.ServerLog += log =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                rtbServerLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {log}\n");
                rtbServerLogs.ScrollToCaret();
            });
        };

        _server.ClientConnected += client =>
        {
            if (IsDisposed) return;
            BeginInvoke(RefreshConnectedClientsList);
        };

        _server.ClientDisconnected += client =>
        {
            if (IsDisposed) return;
            BeginInvoke(RefreshConnectedClientsList);
        };

        _server.StateChanged += isRunning =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                btnStartServer.Enabled = !isRunning;
                btnStopServer.Enabled = isRunning;
                txtListenPort.Enabled = !isRunning;
                txtBroadcastMsg.Enabled = isRunning;
                btnBroadcast.Enabled = isRunning;

                if (isRunning)
                {
                    lblServerStatus.Text = $"● DİNLENİYOR (Port: {_server.Port})";
                    lblServerStatus.BackColor = Color.FromArgb(220, 245, 220);
                    lblServerStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lblServerStatus.Text = "○ SUNUCU KAPALI";
                    lblServerStatus.BackColor = Color.FromArgb(240, 240, 240);
                    lblServerStatus.ForeColor = Color.DimGray;
                    lvwClients.Items.Clear();
                }
            });
        };
    }

    private async void BtnStartServer_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtListenPort.Text.Trim(), out int port) || port < 1024 || port > 65535)
        {
            MessageBox.Show("Lütfen geçerli bir port numarası giriniz (1024 - 65535).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _ = _server.StartAsync(port);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Sunucu başlatılamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnStopServer_Click(object? sender, EventArgs e)
    {
        _server.Stop();
    }

    private async void BtnBroadcast_Click(object? sender, EventArgs e)
    {
        string text = txtBroadcastMsg.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        var msg = new ChatMessage("Sunucu (Anons)", text, MessageType.System);
        await _server.BroadcastAsync(msg);
        txtBroadcastMsg.Clear();
    }

    private void RefreshConnectedClientsList()
    {
        lvwClients.BeginUpdate();
        lvwClients.Items.Clear();

        foreach (var client in _server.GetConnectedClients())
        {
            var lvi = new ListViewItem(client.Username);
            lvi.SubItems.Add(client.EndPoint);
            lvi.SubItems.Add(client.ConnectedAt.ToString("HH:mm:ss"));
            lvwClients.Items.Add(lvi);
        }

        lvwClients.EndUpdate();
    }

    #endregion

    #region İstemci Yönetimi ve Olayları

    private void SetupClientEvents()
    {
        _client.MessageReceived += message =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                _history.Add(message);
                AppendChatMessage(message);
            });
        };

        _client.UserListUpdated += users =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                lstOnlineUsers.BeginUpdate();
                lstOnlineUsers.Items.Clear();
                foreach (var user in users)
                {
                    lstOnlineUsers.Items.Add(user);
                }
                lstOnlineUsers.EndUpdate();
            });
        };

        _client.ConnectionStateChanged += isConnected =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                btnConnect.Enabled = !isConnected;
                btnDisconnect.Enabled = isConnected;
                txtServerIp.Enabled = !isConnected;
                txtServerPort.Enabled = !isConnected;
                txtUsername.Enabled = !isConnected;

                txtMessage.Enabled = isConnected;
                btnSend.Enabled = isConnected;

                if (isConnected)
                {
                    lblClientStatus.Text = "● BAĞLI";
                    lblClientStatus.BackColor = Color.FromArgb(220, 245, 220);
                    lblClientStatus.ForeColor = Color.DarkGreen;
                    txtMessage.Focus();
                }
                else
                {
                    lblClientStatus.Text = "○ BAĞLI DEĞİL";
                    lblClientStatus.BackColor = Color.FromArgb(240, 240, 240);
                    lblClientStatus.ForeColor = Color.DimGray;
                    lstOnlineUsers.Items.Clear();
                }
            });
        };

        _client.ErrorOccurred += error =>
        {
            if (IsDisposed) return;
            BeginInvoke(() =>
            {
                MessageBox.Show(error, "Bağlantı Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        };
    }

    private async void BtnConnect_Click(object? sender, EventArgs e)
    {
        string host = txtServerIp.Text.Trim();
        string user = txtUsername.Text.Trim();

        if (string.IsNullOrEmpty(host))
        {
            MessageBox.Show("Lütfen geçerli bir IP adresi giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrEmpty(user))
        {
            MessageBox.Show("Lütfen bir kullanıcı adı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(txtServerPort.Text.Trim(), out int port) || port < 1024 || port > 65535)
        {
            MessageBox.Show("Lütfen geçerli bir port numarası giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnConnect.Enabled = false;
        bool ok = await _client.ConnectAsync(host, port, user);
        if (!ok)
        {
            btnConnect.Enabled = true;
        }
    }

    private void BtnDisconnect_Click(object? sender, EventArgs e)
    {
        _client.Disconnect();
    }

    private async void BtnSend_Click(object? sender, EventArgs e)
    {
        await SendMessageAsync();
    }

    private async void TxtMessage_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await SendMessageAsync();
        }
    }

    private async Task SendMessageAsync()
    {
        string text = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        string? recipient = null;

        // Özel mesaj formatı desteği: @KullanıcıAdı mesaj
        if (text.StartsWith("@") && text.Contains(" "))
        {
            int spaceIdx = text.IndexOf(' ');
            recipient = text[1..spaceIdx];
            text = text[(spaceIdx + 1)..].Trim();
        }

        bool sent = await _client.SendMessageAsync(text, recipient);
        if (sent)
        {
            txtMessage.Clear();
        }
    }

    private void AppendChatMessage(ChatMessage message)
    {
        string time = message.Timestamp.ToString("HH:mm:ss");

        switch (message.Type)
        {
            case MessageType.Join:
            case MessageType.Leave:
                AppendColoredText($"[{time}] [BİLGİ] {message.Sender} {(message.Type == MessageType.Join ? "sohbete katıldı." : "sohbetten ayrıldı.")}\n", Color.DarkCyan, FontStyle.Italic);
                break;

            case MessageType.System:
                AppendColoredText($"[{time}] [SİSTEM] {message.Sender}: {message.Content}\n", Color.DarkMagenta, FontStyle.Bold);
                break;

            case MessageType.Private:
                AppendColoredText($"[{time}] [ÖZEL] {message.Sender} -> {message.Recipient}: {message.Content}\n", Color.DarkOrange, FontStyle.Bold);
                break;

            case MessageType.Chat:
            default:
                bool isMe = message.Sender.Equals(_client.Username, StringComparison.OrdinalIgnoreCase);
                Color senderColor = isMe ? Color.Navy : Color.DarkGreen;
                AppendColoredText($"[{time}] {message.Sender}: ", senderColor, FontStyle.Bold);
                AppendColoredText($"{message.Content}\n", Color.Black, FontStyle.Regular);
                break;
        }

        rtbChat.ScrollToCaret();
    }

    private void AppendColoredText(string text, Color color, FontStyle style)
    {
        rtbChat.SelectionStart = rtbChat.TextLength;
        rtbChat.SelectionLength = 0;
        rtbChat.SelectionColor = color;
        rtbChat.SelectionFont = new Font(rtbChat.Font, style);
        rtbChat.AppendText(text);
        rtbChat.SelectionColor = rtbChat.ForeColor;
    }

    private void LstOnlineUsers_DoubleClick(object? sender, EventArgs e)
    {
        if (lstOnlineUsers.SelectedItem is string selectedUser && selectedUser != _client.Username)
        {
            txtMessage.Text = $"@{selectedUser} ";
            txtMessage.SelectionStart = txtMessage.Text.Length;
            txtMessage.Focus();
        }
    }

    private void BtnExportChat_Click(object? sender, EventArgs e)
    {
        if (_history.Messages.Count == 0)
        {
            MessageBox.Show("Kaydedilecek herhangi bir sohbet mesajı bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "Metin Dosyası (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*",
            FileName = $"SohbetGecmisi_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
            Title = "Sohbet Geçmişini Kaydet"
        };

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _history.ExportToFile(sfd.FileName);
                MessageBox.Show($"Sohbet geçmişi başarıyla kaydedildi:\n{sfd.FileName}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _client.Disconnect();
        _server.Stop();
    }
}
