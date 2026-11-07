namespace YMH219_ChatApp;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabClient;
    private System.Windows.Forms.TabPage tabServer;

    // --- İstemci Bileşenleri ---
    private System.Windows.Forms.Panel pnlClientTop;
    private System.Windows.Forms.Label lblIp;
    private System.Windows.Forms.TextBox txtServerIp;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.TextBox txtServerPort;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnDisconnect;
    private System.Windows.Forms.Label lblClientStatus;

    private System.Windows.Forms.SplitContainer splitClient;
    private System.Windows.Forms.RichTextBox rtbChat;
    private System.Windows.Forms.GroupBox grpOnlineUsers;
    private System.Windows.Forms.ListBox lstOnlineUsers;
    private System.Windows.Forms.Label lblPrivateHint;

    private System.Windows.Forms.Panel pnlClientBottom;
    private System.Windows.Forms.TextBox txtMessage;
    private System.Windows.Forms.Button btnSend;
    private System.Windows.Forms.Button btnExportChat;

    // --- Sunucu Bileşenleri ---
    private System.Windows.Forms.Panel pnlServerTop;
    private System.Windows.Forms.Label lblServerPort;
    private System.Windows.Forms.TextBox txtListenPort;
    private System.Windows.Forms.Button btnStartServer;
    private System.Windows.Forms.Button btnStopServer;
    private System.Windows.Forms.Label lblServerStatus;

    private System.Windows.Forms.SplitContainer splitServer;
    private System.Windows.Forms.GroupBox grpConnectedClients;
    private System.Windows.Forms.ListView lvwClients;
    private System.Windows.Forms.ColumnHeader colClientUser;
    private System.Windows.Forms.ColumnHeader colClientIp;
    private System.Windows.Forms.ColumnHeader colClientTime;

    private System.Windows.Forms.GroupBox grpServerLogs;
    private System.Windows.Forms.RichTextBox rtbServerLogs;
    private System.Windows.Forms.Panel pnlServerBroadcast;
    private System.Windows.Forms.TextBox txtBroadcastMsg;
    private System.Windows.Forms.Button btnBroadcast;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        tabControl = new System.Windows.Forms.TabControl();
        tabClient = new System.Windows.Forms.TabPage();
        tabServer = new System.Windows.Forms.TabPage();

        // İstemci bileşenleri
        pnlClientTop = new System.Windows.Forms.Panel();
        lblIp = new System.Windows.Forms.Label();
        txtServerIp = new System.Windows.Forms.TextBox();
        lblPort = new System.Windows.Forms.Label();
        txtServerPort = new System.Windows.Forms.TextBox();
        lblUser = new System.Windows.Forms.Label();
        txtUsername = new System.Windows.Forms.TextBox();
        btnConnect = new System.Windows.Forms.Button();
        btnDisconnect = new System.Windows.Forms.Button();
        lblClientStatus = new System.Windows.Forms.Label();

        splitClient = new System.Windows.Forms.SplitContainer();
        rtbChat = new System.Windows.Forms.RichTextBox();
        grpOnlineUsers = new System.Windows.Forms.GroupBox();
        lstOnlineUsers = new System.Windows.Forms.ListBox();
        lblPrivateHint = new System.Windows.Forms.Label();

        pnlClientBottom = new System.Windows.Forms.Panel();
        txtMessage = new System.Windows.Forms.TextBox();
        btnSend = new System.Windows.Forms.Button();
        btnExportChat = new System.Windows.Forms.Button();

        // Sunucu bileşenleri
        pnlServerTop = new System.Windows.Forms.Panel();
        lblServerPort = new System.Windows.Forms.Label();
        txtListenPort = new System.Windows.Forms.TextBox();
        btnStartServer = new System.Windows.Forms.Button();
        btnStopServer = new System.Windows.Forms.Button();
        lblServerStatus = new System.Windows.Forms.Label();

        splitServer = new System.Windows.Forms.SplitContainer();
        grpConnectedClients = new System.Windows.Forms.GroupBox();
        lvwClients = new System.Windows.Forms.ListView();
        colClientUser = new System.Windows.Forms.ColumnHeader();
        colClientIp = new System.Windows.Forms.ColumnHeader();
        colClientTime = new System.Windows.Forms.ColumnHeader();

        grpServerLogs = new System.Windows.Forms.GroupBox();
        rtbServerLogs = new System.Windows.Forms.RichTextBox();
        pnlServerBroadcast = new System.Windows.Forms.Panel();
        txtBroadcastMsg = new System.Windows.Forms.TextBox();
        btnBroadcast = new System.Windows.Forms.Button();

        tabControl.SuspendLayout();
        tabClient.SuspendLayout();
        pnlClientTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitClient).BeginInit();
        splitClient.Panel1.SuspendLayout();
        splitClient.Panel2.SuspendLayout();
        splitClient.SuspendLayout();
        grpOnlineUsers.SuspendLayout();
        pnlClientBottom.SuspendLayout();

        tabServer.SuspendLayout();
        pnlServerTop.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitServer).BeginInit();
        splitServer.Panel1.SuspendLayout();
        splitServer.Panel2.SuspendLayout();
        splitServer.SuspendLayout();
        grpConnectedClients.SuspendLayout();
        grpServerLogs.SuspendLayout();
        pnlServerBroadcast.SuspendLayout();
        SuspendLayout();

        // 
        // tabControl
        // 
        tabControl.Controls.Add(tabClient);
        tabControl.Controls.Add(tabServer);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Font = new Font("Segoe UI", 9.5F);
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(884, 561);
        tabControl.TabIndex = 0;

        // ==========================================
        // TAB 1: İSTEMCİ (CHAT CLIENT)
        // ==========================================
        tabClient.Controls.Add(splitClient);
        tabClient.Controls.Add(pnlClientBottom);
        tabClient.Controls.Add(pnlClientTop);
        tabClient.Location = new Point(4, 25);
        tabClient.Name = "tabClient";
        tabClient.Padding = new Padding(3);
        tabClient.Size = new Size(876, 532);
        tabClient.Text = "💬 Sohbet İstemcisi";
        tabClient.UseVisualStyleBackColor = true;

        // pnlClientTop
        pnlClientTop.BackColor = Color.FromArgb(245, 248, 250);
        pnlClientTop.BorderStyle = BorderStyle.FixedSingle;
        pnlClientTop.Controls.Add(lblIp);
        pnlClientTop.Controls.Add(txtServerIp);
        pnlClientTop.Controls.Add(lblPort);
        pnlClientTop.Controls.Add(txtServerPort);
        pnlClientTop.Controls.Add(lblUser);
        pnlClientTop.Controls.Add(txtUsername);
        pnlClientTop.Controls.Add(btnConnect);
        pnlClientTop.Controls.Add(btnDisconnect);
        pnlClientTop.Controls.Add(lblClientStatus);
        pnlClientTop.Dock = DockStyle.Top;
        pnlClientTop.Location = new Point(3, 3);
        pnlClientTop.Name = "pnlClientTop";
        pnlClientTop.Size = new Size(870, 48);
        pnlClientTop.TabIndex = 0;

        // lblIp & txtServerIp
        lblIp.AutoSize = true;
        lblIp.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblIp.Location = new Point(8, 15);
        lblIp.Name = "lblIp";
        lblIp.Size = new Size(65, 15);
        lblIp.Text = "Sunucu IP:";
        txtServerIp.Location = new Point(75, 11);
        txtServerIp.Name = "txtServerIp";
        txtServerIp.Size = new Size(95, 24);
        txtServerIp.Text = "127.0.0.1";

        // lblPort & txtServerPort
        lblPort.AutoSize = true;
        lblPort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPort.Location = new Point(178, 15);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(34, 15);
        lblPort.Text = "Port:";
        txtServerPort.Location = new Point(215, 11);
        txtServerPort.Name = "txtServerPort";
        txtServerPort.Size = new Size(55, 24);
        txtServerPort.Text = "8888";

        // lblUser & txtUsername
        lblUser.AutoSize = true;
        lblUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUser.Location = new Point(280, 15);
        lblUser.Name = "lblUser";
        lblUser.Size = new Size(76, 15);
        lblUser.Text = "Kullanıcı Adı:";
        txtUsername.Location = new Point(360, 11);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(110, 24);
        txtUsername.Text = "Birgül";

        // btnConnect & btnDisconnect
        btnConnect.BackColor = Color.FromArgb(46, 139, 87);
        btnConnect.FlatStyle = FlatStyle.Flat;
        btnConnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnConnect.ForeColor = Color.White;
        btnConnect.Location = new Point(480, 9);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(90, 28);
        btnConnect.Text = "Bağlan";
        btnConnect.UseVisualStyleBackColor = false;
        btnConnect.Click += BtnConnect_Click;

        btnDisconnect.BackColor = Color.FromArgb(205, 92, 92);
        btnDisconnect.Enabled = false;
        btnDisconnect.FlatStyle = FlatStyle.Flat;
        btnDisconnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDisconnect.ForeColor = Color.White;
        btnDisconnect.Location = new Point(576, 9);
        btnDisconnect.Name = "btnDisconnect";
        btnDisconnect.Size = new Size(100, 28);
        btnDisconnect.Text = "Bağlantıyı Kes";
        btnDisconnect.UseVisualStyleBackColor = false;
        btnDisconnect.Click += BtnDisconnect_Click;

        // lblClientStatus
        lblClientStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblClientStatus.BackColor = Color.FromArgb(235, 235, 235);
        lblClientStatus.BorderStyle = BorderStyle.FixedSingle;
        lblClientStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblClientStatus.ForeColor = Color.DimGray;
        lblClientStatus.Location = new Point(685, 9);
        lblClientStatus.Name = "lblClientStatus";
        lblClientStatus.Size = new Size(175, 28);
        lblClientStatus.Text = "○ BAĞLI DEĞİL";
        lblClientStatus.TextAlign = ContentAlignment.MiddleCenter;

        // splitClient
        splitClient.Dock = DockStyle.Fill;
        splitClient.FixedPanel = FixedPanel.Panel2;
        splitClient.Location = new Point(3, 51);
        splitClient.Name = "splitClient";
        splitClient.Panel1.Controls.Add(rtbChat);
        splitClient.Panel2.Controls.Add(grpOnlineUsers);
        splitClient.Panel2.Padding = new Padding(4);
        splitClient.Size = new Size(870, 433);
        splitClient.SplitterDistance = 640;

        // rtbChat
        rtbChat.BackColor = Color.White;
        rtbChat.Dock = DockStyle.Fill;
        rtbChat.Font = new Font("Segoe UI", 9.75F);
        rtbChat.Location = new Point(0, 0);
        rtbChat.Name = "rtbChat";
        rtbChat.ReadOnly = true;
        rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbChat.Size = new Size(640, 433);

        // grpOnlineUsers
        grpOnlineUsers.Controls.Add(lstOnlineUsers);
        grpOnlineUsers.Controls.Add(lblPrivateHint);
        grpOnlineUsers.Dock = DockStyle.Fill;
        grpOnlineUsers.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpOnlineUsers.Location = new Point(4, 4);
        grpOnlineUsers.Name = "grpOnlineUsers";
        grpOnlineUsers.Size = new Size(218, 425);
        grpOnlineUsers.Text = "👥 Çevrimiçi Kullanıcılar";

        lstOnlineUsers.Dock = DockStyle.Fill;
        lstOnlineUsers.Font = new Font("Segoe UI", 9.5F);
        lstOnlineUsers.FormattingEnabled = true;
        lstOnlineUsers.ItemHeight = 17;
        lstOnlineUsers.Location = new Point(3, 19);
        lstOnlineUsers.Name = "lstOnlineUsers";
        lstOnlineUsers.Size = new Size(212, 365);
        lstOnlineUsers.DoubleClick += LstOnlineUsers_DoubleClick;

        lblPrivateHint.Dock = DockStyle.Bottom;
        lblPrivateHint.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
        lblPrivateHint.ForeColor = Color.DarkSlateGray;
        lblPrivateHint.Location = new Point(3, 384);
        lblPrivateHint.Name = "lblPrivateHint";
        lblPrivateHint.Size = new Size(212, 38);
        lblPrivateHint.Text = "İpucu: Özel mesaj göndermek için kullanıcı adına çift tıklayın.";
        lblPrivateHint.TextAlign = ContentAlignment.TopCenter;

        // pnlClientBottom
        pnlClientBottom.BackColor = Color.FromArgb(245, 248, 250);
        pnlClientBottom.BorderStyle = BorderStyle.FixedSingle;
        pnlClientBottom.Controls.Add(txtMessage);
        pnlClientBottom.Controls.Add(btnSend);
        pnlClientBottom.Controls.Add(btnExportChat);
        pnlClientBottom.Dock = DockStyle.Bottom;
        pnlClientBottom.Location = new Point(3, 484);
        pnlClientBottom.Name = "pnlClientBottom";
        pnlClientBottom.Size = new Size(870, 45);

        txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtMessage.Enabled = false;
        txtMessage.Location = new Point(8, 10);
        txtMessage.Name = "txtMessage";
        txtMessage.PlaceholderText = "Mesajınızı yazın (Özel mesaj için: @KullanıcıAdı mesajınız)...";
        txtMessage.Size = new Size(590, 24);
        txtMessage.KeyDown += TxtMessage_KeyDown;

        btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSend.BackColor = Color.FromArgb(70, 130, 180);
        btnSend.Enabled = false;
        btnSend.FlatStyle = FlatStyle.Flat;
        btnSend.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSend.ForeColor = Color.White;
        btnSend.Location = new Point(606, 8);
        btnSend.Name = "btnSend";
        btnSend.Size = new Size(95, 28);
        btnSend.Text = "Gönder (↵)";
        btnSend.UseVisualStyleBackColor = false;
        btnSend.Click += BtnSend_Click;

        btnExportChat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnExportChat.BackColor = Color.FromArgb(240, 240, 240);
        btnExportChat.FlatStyle = FlatStyle.Flat;
        btnExportChat.Font = new Font("Segoe UI", 8.5F);
        btnExportChat.Location = new Point(707, 8);
        btnExportChat.Name = "btnExportChat";
        btnExportChat.Size = new Size(153, 28);
        btnExportChat.Text = "💾 Geçmişi Kaydet (.txt)";
        btnExportChat.UseVisualStyleBackColor = true;
        btnExportChat.Click += BtnExportChat_Click;

        // ==========================================
        // TAB 2: SUNUCU (CHAT SERVER)
        // ==========================================
        tabServer.Controls.Add(splitServer);
        tabServer.Controls.Add(pnlServerTop);
        tabServer.Location = new Point(4, 25);
        tabServer.Name = "tabServer";
        tabServer.Padding = new Padding(3);
        tabServer.Size = new Size(876, 532);
        tabServer.Text = "🖥️ Sunucu Yönetimi";
        tabServer.UseVisualStyleBackColor = true;

        // pnlServerTop
        pnlServerTop.BackColor = Color.FromArgb(245, 248, 250);
        pnlServerTop.BorderStyle = BorderStyle.FixedSingle;
        pnlServerTop.Controls.Add(lblServerPort);
        pnlServerTop.Controls.Add(txtListenPort);
        pnlServerTop.Controls.Add(btnStartServer);
        pnlServerTop.Controls.Add(btnStopServer);
        pnlServerTop.Controls.Add(lblServerStatus);
        pnlServerTop.Dock = DockStyle.Top;
        pnlServerTop.Location = new Point(3, 3);
        pnlServerTop.Name = "pnlServerTop";
        pnlServerTop.Size = new Size(870, 48);

        lblServerPort.AutoSize = true;
        lblServerPort.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblServerPort.Location = new Point(12, 16);
        lblServerPort.Name = "lblServerPort";
        lblServerPort.Size = new Size(83, 15);
        lblServerPort.Text = "Dinleme Portu:";
        txtListenPort.Location = new Point(100, 11);
        txtListenPort.Name = "txtListenPort";
        txtListenPort.Size = new Size(70, 24);
        txtListenPort.Text = "8888";

        btnStartServer.BackColor = Color.FromArgb(46, 139, 87);
        btnStartServer.FlatStyle = FlatStyle.Flat;
        btnStartServer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStartServer.ForeColor = Color.White;
        btnStartServer.Location = new Point(185, 9);
        btnStartServer.Name = "btnStartServer";
        btnStartServer.Size = new Size(130, 28);
        btnStartServer.Text = "Sunucuyu Başlat";
        btnStartServer.UseVisualStyleBackColor = false;
        btnStartServer.Click += BtnStartServer_Click;

        btnStopServer.BackColor = Color.FromArgb(205, 92, 92);
        btnStopServer.Enabled = false;
        btnStopServer.FlatStyle = FlatStyle.Flat;
        btnStopServer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStopServer.ForeColor = Color.White;
        btnStopServer.Location = new Point(325, 9);
        btnStopServer.Name = "btnStopServer";
        btnStopServer.Size = new Size(130, 28);
        btnStopServer.Text = "Sunucuyu Durdur";
        btnStopServer.UseVisualStyleBackColor = false;
        btnStopServer.Click += BtnStopServer_Click;

        lblServerStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblServerStatus.BackColor = Color.FromArgb(235, 235, 235);
        lblServerStatus.BorderStyle = BorderStyle.FixedSingle;
        lblServerStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblServerStatus.ForeColor = Color.DimGray;
        lblServerStatus.Location = new Point(660, 9);
        lblServerStatus.Name = "lblServerStatus";
        lblServerStatus.Size = new Size(200, 28);
        lblServerStatus.Text = "○ SUNUCU KAPALI";
        lblServerStatus.TextAlign = ContentAlignment.MiddleCenter;

        // splitServer
        splitServer.Dock = DockStyle.Fill;
        splitServer.Location = new Point(3, 51);
        splitServer.Name = "splitServer";
        splitServer.Orientation = Orientation.Horizontal;
        splitServer.Panel1.Controls.Add(grpConnectedClients);
        splitServer.Panel1.Padding = new Padding(3);
        splitServer.Panel2.Controls.Add(grpServerLogs);
        splitServer.Panel2.Controls.Add(pnlServerBroadcast);
        splitServer.Panel2.Padding = new Padding(3);
        splitServer.Size = new Size(870, 478);
        splitServer.SplitterDistance = 210;

        // grpConnectedClients & lvwClients
        grpConnectedClients.Controls.Add(lvwClients);
        grpConnectedClients.Dock = DockStyle.Fill;
        grpConnectedClients.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpConnectedClients.Location = new Point(3, 3);
        grpConnectedClients.Name = "grpConnectedClients";
        grpConnectedClients.Size = new Size(864, 204);
        grpConnectedClients.Text = "📋 Bağlı İstemciler (Kullanıcı Adı, IP ve Bağlantı Zamanı)";

        lvwClients.Columns.AddRange(new ColumnHeader[] {
            colClientUser,
            colClientIp,
            colClientTime
        });
        lvwClients.Dock = DockStyle.Fill;
        lvwClients.FullRowSelect = true;
        lvwClients.GridLines = true;
        lvwClients.Font = new Font("Segoe UI", 9F);
        lvwClients.Location = new Point(3, 19);
        lvwClients.Name = "lvwClients";
        lvwClients.Size = new Size(858, 182);
        lvwClients.View = View.Details;

        colClientUser.Text = "Kullanıcı Adı";
        colClientUser.Width = 200;
        colClientIp.Text = "Uzak IP:Port Adresi";
        colClientIp.Width = 220;
        colClientTime.Text = "Bağlantı Saati";
        colClientTime.Width = 180;

        // grpServerLogs
        grpServerLogs.Controls.Add(rtbServerLogs);
        grpServerLogs.Dock = DockStyle.Fill;
        grpServerLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpServerLogs.Location = new Point(3, 3);
        grpServerLogs.Name = "grpServerLogs";
        grpServerLogs.Size = new Size(864, 218);
        grpServerLogs.Text = "📜 Sunucu Olay Günlüğü (Log)";

        rtbServerLogs.BackColor = Color.FromArgb(30, 30, 30);
        rtbServerLogs.Dock = DockStyle.Fill;
        rtbServerLogs.Font = new Font("Consolas", 9.5F);
        rtbServerLogs.ForeColor = Color.LightGreen;
        rtbServerLogs.Location = new Point(3, 19);
        rtbServerLogs.Name = "rtbServerLogs";
        rtbServerLogs.ReadOnly = true;
        rtbServerLogs.Size = new Size(858, 196);

        // pnlServerBroadcast
        pnlServerBroadcast.Controls.Add(txtBroadcastMsg);
        pnlServerBroadcast.Controls.Add(btnBroadcast);
        pnlServerBroadcast.Dock = DockStyle.Bottom;
        pnlServerBroadcast.Location = new Point(3, 221);
        pnlServerBroadcast.Name = "pnlServerBroadcast";
        pnlServerBroadcast.Size = new Size(864, 40);

        txtBroadcastMsg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtBroadcastMsg.Enabled = false;
        txtBroadcastMsg.Location = new Point(3, 8);
        txtBroadcastMsg.Name = "txtBroadcastMsg";
        txtBroadcastMsg.PlaceholderText = "Tüm bağlı kullanıcılara sunucu adına genel anons/duyuru yap...";
        txtBroadcastMsg.Size = new Size(710, 24);

        btnBroadcast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBroadcast.BackColor = Color.FromArgb(70, 130, 180);
        btnBroadcast.Enabled = false;
        btnBroadcast.FlatStyle = FlatStyle.Flat;
        btnBroadcast.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBroadcast.ForeColor = Color.White;
        btnBroadcast.Location = new Point(720, 6);
        btnBroadcast.Name = "btnBroadcast";
        btnBroadcast.Size = new Size(140, 28);
        btnBroadcast.Text = "📢 Anons Gönder";
        btnBroadcast.UseVisualStyleBackColor = false;
        btnBroadcast.Click += BtnBroadcast_Click;

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(884, 561);
        Controls.Add(tabControl);
        MinimumSize = new Size(780, 500);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "YMH219 - İstemci-Sunucu Chat Uygulaması";
        FormClosing += Form1_FormClosing;

        tabControl.ResumeLayout(false);
        tabClient.ResumeLayout(false);
        pnlClientTop.ResumeLayout(false);
        pnlClientTop.PerformLayout();
        splitClient.Panel1.ResumeLayout(false);
        splitClient.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitClient).EndInit();
        splitClient.ResumeLayout(false);
        grpOnlineUsers.ResumeLayout(false);
        pnlClientBottom.ResumeLayout(false);
        pnlClientBottom.PerformLayout();

        tabServer.ResumeLayout(false);
        pnlServerTop.ResumeLayout(false);
        pnlServerTop.PerformLayout();
        splitServer.Panel1.ResumeLayout(false);
        splitServer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitServer).EndInit();
        splitServer.ResumeLayout(false);
        grpConnectedClients.ResumeLayout(false);
        grpServerLogs.ResumeLayout(false);
        pnlServerBroadcast.ResumeLayout(false);
        pnlServerBroadcast.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
}
