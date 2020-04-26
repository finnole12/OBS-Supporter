namespace OBS_Supporter
{
    partial class frmOBSSupporter
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOBSSupporter));
            this.nfiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblObsPath = new System.Windows.Forms.Label();
            this.tbxObsPath = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblInvalid = new System.Windows.Forms.Label();
            this.cbxShowConsoleOnLaunch = new System.Windows.Forms.CheckBox();
            this.btnShowConsole = new System.Windows.Forms.Button();
            this.cbxStrikeDriverRelaunch = new System.Windows.Forms.CheckBox();
            this.cbxSynapseRelaunch = new System.Windows.Forms.CheckBox();
            this.cbxTeamspeakRelaunch = new System.Windows.Forms.CheckBox();
            this.btnAddSceneConfig = new System.Windows.Forms.Button();
            this.btnOpenConnect = new System.Windows.Forms.Button();
            this.lblConnection = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlConsole = new System.Windows.Forms.Panel();
            this.pnlGeneral = new System.Windows.Forms.Panel();
            this.pnlFixes = new System.Windows.Forms.Panel();
            this.pnlSceneConfig = new System.Windows.Forms.Panel();
            this.btnGeneral = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnConsole = new System.Windows.Forms.Button();
            this.btnSceneConfig = new System.Windows.Forms.Button();
            this.btnFixes = new System.Windows.Forms.Button();
            this.pnlConsole.SuspendLayout();
            this.pnlGeneral.SuspendLayout();
            this.pnlFixes.SuspendLayout();
            this.pnlSceneConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // nfiTrayIcon
            // 
            this.nfiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("nfiTrayIcon.Icon")));
            this.nfiTrayIcon.Text = "OBS Supporter";
            this.nfiTrayIcon.Visible = true;
            this.nfiTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nfiTrayIcon_MouseClick);
            // 
            // lblObsPath
            // 
            this.lblObsPath.AutoSize = true;
            this.lblObsPath.Location = new System.Drawing.Point(3, 11);
            this.lblObsPath.Name = "lblObsPath";
            this.lblObsPath.Size = new System.Drawing.Size(69, 13);
            this.lblObsPath.TabIndex = 0;
            this.lblObsPath.Text = "Path to OBS:";
            // 
            // tbxObsPath
            // 
            this.tbxObsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxObsPath.BackColor = System.Drawing.SystemColors.Window;
            this.tbxObsPath.Location = new System.Drawing.Point(78, 8);
            this.tbxObsPath.Name = "tbxObsPath";
            this.tbxObsPath.Size = new System.Drawing.Size(532, 20);
            this.tbxObsPath.TabIndex = 1;
            this.tbxObsPath.TextChanged += new System.EventHandler(this.tbxObsPath_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(809, 320);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblInvalid
            // 
            this.lblInvalid.AutoSize = true;
            this.lblInvalid.BackColor = System.Drawing.SystemColors.Control;
            this.lblInvalid.ForeColor = System.Drawing.Color.Red;
            this.lblInvalid.Location = new System.Drawing.Point(75, 31);
            this.lblInvalid.Name = "lblInvalid";
            this.lblInvalid.Size = new System.Drawing.Size(76, 13);
            this.lblInvalid.TabIndex = 3;
            this.lblInvalid.Text = "*Path is invalid";
            this.lblInvalid.Visible = false;
            // 
            // cbxShowConsoleOnLaunch
            // 
            this.cbxShowConsoleOnLaunch.AutoSize = true;
            this.cbxShowConsoleOnLaunch.Location = new System.Drawing.Point(3, 3);
            this.cbxShowConsoleOnLaunch.Name = "cbxShowConsoleOnLaunch";
            this.cbxShowConsoleOnLaunch.Size = new System.Drawing.Size(148, 17);
            this.cbxShowConsoleOnLaunch.TabIndex = 2;
            this.cbxShowConsoleOnLaunch.Text = "Show Console on Launch";
            this.cbxShowConsoleOnLaunch.UseVisualStyleBackColor = true;
            this.cbxShowConsoleOnLaunch.CheckStateChanged += new System.EventHandler(this.cbxShowConsoleOnBoot_CheckStateChanged);
            // 
            // btnShowConsole
            // 
            this.btnShowConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowConsole.Location = new System.Drawing.Point(569, 3);
            this.btnShowConsole.Name = "btnShowConsole";
            this.btnShowConsole.Size = new System.Drawing.Size(119, 23);
            this.btnShowConsole.TabIndex = 0;
            this.btnShowConsole.Text = "Click to hide Console";
            this.btnShowConsole.UseVisualStyleBackColor = true;
            this.btnShowConsole.Click += new System.EventHandler(this.btnShowConsole_Click);
            // 
            // cbxStrikeDriverRelaunch
            // 
            this.cbxStrikeDriverRelaunch.AutoSize = true;
            this.cbxStrikeDriverRelaunch.Location = new System.Drawing.Point(3, 49);
            this.cbxStrikeDriverRelaunch.Name = "cbxStrikeDriverRelaunch";
            this.cbxStrikeDriverRelaunch.Size = new System.Drawing.Size(172, 17);
            this.cbxStrikeDriverRelaunch.TabIndex = 2;
            this.cbxStrikeDriverRelaunch.Text = "S.T.R.I.K.E TE Driver relaunch";
            this.cbxStrikeDriverRelaunch.UseVisualStyleBackColor = true;
            this.cbxStrikeDriverRelaunch.CheckedChanged += new System.EventHandler(this.cbxStrikeDriverRelaunch_CheckedChanged);
            // 
            // cbxSynapseRelaunch
            // 
            this.cbxSynapseRelaunch.AutoSize = true;
            this.cbxSynapseRelaunch.Location = new System.Drawing.Point(3, 26);
            this.cbxSynapseRelaunch.Name = "cbxSynapseRelaunch";
            this.cbxSynapseRelaunch.Size = new System.Drawing.Size(142, 17);
            this.cbxSynapseRelaunch.TabIndex = 1;
            this.cbxSynapseRelaunch.Text = "Razor Synapse relaunch";
            this.cbxSynapseRelaunch.UseVisualStyleBackColor = true;
            this.cbxSynapseRelaunch.CheckedChanged += new System.EventHandler(this.cbxSynapseRelaunch_CheckedChanged);
            // 
            // cbxTeamspeakRelaunch
            // 
            this.cbxTeamspeakRelaunch.AutoSize = true;
            this.cbxTeamspeakRelaunch.Location = new System.Drawing.Point(3, 3);
            this.cbxTeamspeakRelaunch.Name = "cbxTeamspeakRelaunch";
            this.cbxTeamspeakRelaunch.Size = new System.Drawing.Size(126, 17);
            this.cbxTeamspeakRelaunch.TabIndex = 0;
            this.cbxTeamspeakRelaunch.Text = "Teamspeak relaunch";
            this.cbxTeamspeakRelaunch.UseVisualStyleBackColor = true;
            this.cbxTeamspeakRelaunch.CheckedChanged += new System.EventHandler(this.cbxTeamspeakRelaunch_CheckedChanged);
            // 
            // btnAddSceneConfig
            // 
            this.btnAddSceneConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSceneConfig.Location = new System.Drawing.Point(3, 3);
            this.btnAddSceneConfig.Name = "btnAddSceneConfig";
            this.btnAddSceneConfig.Size = new System.Drawing.Size(688, 23);
            this.btnAddSceneConfig.TabIndex = 11;
            this.btnAddSceneConfig.Text = "add Scene Configuration";
            this.btnAddSceneConfig.UseVisualStyleBackColor = true;
            this.btnAddSceneConfig.LocationChanged += new System.EventHandler(this.btnAddSceneConfig_LocationChanged);
            this.btnAddSceneConfig.Click += new System.EventHandler(this.btnAddSceneConfig_Click);
            // 
            // btnOpenConnect
            // 
            this.btnOpenConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenConnect.Location = new System.Drawing.Point(587, 35);
            this.btnOpenConnect.Name = "btnOpenConnect";
            this.btnOpenConnect.Size = new System.Drawing.Size(104, 23);
            this.btnOpenConnect.TabIndex = 6;
            this.btnOpenConnect.Text = "open and connect";
            this.btnOpenConnect.UseVisualStyleBackColor = true;
            this.btnOpenConnect.Click += new System.EventHandler(this.btnOpenConnect_Click);
            // 
            // lblConnection
            // 
            this.lblConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnection.AutoSize = true;
            this.lblConnection.ForeColor = System.Drawing.Color.Red;
            this.lblConnection.Location = new System.Drawing.Point(611, 61);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(80, 13);
            this.lblConnection.TabIndex = 7;
            this.lblConnection.Text = "*not connected";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(616, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlConsole
            // 
            this.pnlConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConsole.Controls.Add(this.btnShowConsole);
            this.pnlConsole.Controls.Add(this.cbxShowConsoleOnLaunch);
            this.pnlConsole.Location = new System.Drawing.Point(190, 23);
            this.pnlConsole.Name = "pnlConsole";
            this.pnlConsole.Size = new System.Drawing.Size(694, 54);
            this.pnlConsole.TabIndex = 12;
            this.pnlConsole.Visible = false;
            // 
            // pnlGeneral
            // 
            this.pnlGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGeneral.Controls.Add(this.lblInvalid);
            this.pnlGeneral.Controls.Add(this.btnBrowse);
            this.pnlGeneral.Controls.Add(this.tbxObsPath);
            this.pnlGeneral.Controls.Add(this.lblObsPath);
            this.pnlGeneral.Controls.Add(this.btnOpenConnect);
            this.pnlGeneral.Controls.Add(this.lblConnection);
            this.pnlGeneral.Location = new System.Drawing.Point(190, 23);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.Size = new System.Drawing.Size(694, 138);
            this.pnlGeneral.TabIndex = 13;
            // 
            // pnlFixes
            // 
            this.pnlFixes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFixes.Controls.Add(this.cbxStrikeDriverRelaunch);
            this.pnlFixes.Controls.Add(this.cbxTeamspeakRelaunch);
            this.pnlFixes.Controls.Add(this.cbxSynapseRelaunch);
            this.pnlFixes.Location = new System.Drawing.Point(190, 23);
            this.pnlFixes.Name = "pnlFixes";
            this.pnlFixes.Size = new System.Drawing.Size(694, 76);
            this.pnlFixes.TabIndex = 14;
            this.pnlFixes.Visible = false;
            // 
            // pnlSceneConfig
            // 
            this.pnlSceneConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSceneConfig.Controls.Add(this.btnAddSceneConfig);
            this.pnlSceneConfig.Location = new System.Drawing.Point(190, 23);
            this.pnlSceneConfig.Name = "pnlSceneConfig";
            this.pnlSceneConfig.Size = new System.Drawing.Size(694, 39);
            this.pnlSceneConfig.TabIndex = 15;
            this.pnlSceneConfig.Visible = false;
            // 
            // btnGeneral
            // 
            this.btnGeneral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeneral.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGeneral.ImageIndex = 0;
            this.btnGeneral.ImageList = this.imageList1;
            this.btnGeneral.Location = new System.Drawing.Point(13, 23);
            this.btnGeneral.Name = "btnGeneral";
            this.btnGeneral.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGeneral.Size = new System.Drawing.Size(171, 23);
            this.btnGeneral.TabIndex = 16;
            this.btnGeneral.Text = "         General";
            this.btnGeneral.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGeneral.UseVisualStyleBackColor = true;
            this.btnGeneral.Click += new System.EventHandler(this.btnGeneral_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "69524.png");
            this.imageList1.Images.SetKeyName(1, "images.png");
            this.imageList1.Images.SetKeyName(2, "1492790974-93list_84195.png");
            this.imageList1.Images.SetKeyName(3, "open-wrench-tool-silhouette_icon-icons.com_73472.png");
            // 
            // btnConsole
            // 
            this.btnConsole.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConsole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsole.ImageIndex = 1;
            this.btnConsole.ImageList = this.imageList1;
            this.btnConsole.Location = new System.Drawing.Point(13, 46);
            this.btnConsole.Name = "btnConsole";
            this.btnConsole.Size = new System.Drawing.Size(171, 23);
            this.btnConsole.TabIndex = 17;
            this.btnConsole.Text = "         Console";
            this.btnConsole.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConsole.UseVisualStyleBackColor = true;
            this.btnConsole.Click += new System.EventHandler(this.btnConsole_Click);
            // 
            // btnSceneConfig
            // 
            this.btnSceneConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSceneConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSceneConfig.ImageIndex = 2;
            this.btnSceneConfig.ImageList = this.imageList1;
            this.btnSceneConfig.Location = new System.Drawing.Point(13, 69);
            this.btnSceneConfig.Name = "btnSceneConfig";
            this.btnSceneConfig.Size = new System.Drawing.Size(171, 23);
            this.btnSceneConfig.TabIndex = 18;
            this.btnSceneConfig.Text = "         Scene Configuration";
            this.btnSceneConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSceneConfig.UseVisualStyleBackColor = true;
            this.btnSceneConfig.Click += new System.EventHandler(this.btnSceneConfig_Click);
            // 
            // btnFixes
            // 
            this.btnFixes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFixes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFixes.ImageIndex = 3;
            this.btnFixes.ImageList = this.imageList1;
            this.btnFixes.Location = new System.Drawing.Point(13, 92);
            this.btnFixes.Name = "btnFixes";
            this.btnFixes.Size = new System.Drawing.Size(171, 23);
            this.btnFixes.TabIndex = 19;
            this.btnFixes.Text = "         Fixes";
            this.btnFixes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFixes.UseVisualStyleBackColor = true;
            this.btnFixes.Click += new System.EventHandler(this.btnFixes_Click);
            // 
            // frmOBSSupporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 353);
            this.Controls.Add(this.btnFixes);
            this.Controls.Add(this.pnlConsole);
            this.Controls.Add(this.btnSceneConfig);
            this.Controls.Add(this.btnConsole);
            this.Controls.Add(this.btnGeneral);
            this.Controls.Add(this.pnlSceneConfig);
            this.Controls.Add(this.pnlFixes);
            this.Controls.Add(this.pnlGeneral);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmOBSSupporter";
            this.Text = "OBS Supporter";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Shown += new System.EventHandler(this.frmOBSSupporter_Shown);
            this.Resize += new System.EventHandler(this.frmOBSSupporter_Resize);
            this.pnlConsole.ResumeLayout(false);
            this.pnlConsole.PerformLayout();
            this.pnlGeneral.ResumeLayout(false);
            this.pnlGeneral.PerformLayout();
            this.pnlFixes.ResumeLayout(false);
            this.pnlFixes.PerformLayout();
            this.pnlSceneConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon nfiTrayIcon;
        private System.Windows.Forms.Label lblObsPath;
        private System.Windows.Forms.TextBox tbxObsPath;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblInvalid;
        private System.Windows.Forms.CheckBox cbxShowConsoleOnLaunch;
        private System.Windows.Forms.Button btnShowConsole;
        private System.Windows.Forms.Button btnOpenConnect;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Button btnAddSceneConfig;
        private System.Windows.Forms.CheckBox cbxStrikeDriverRelaunch;
        private System.Windows.Forms.CheckBox cbxSynapseRelaunch;
        private System.Windows.Forms.CheckBox cbxTeamspeakRelaunch;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Panel pnlConsole;
        private System.Windows.Forms.Panel pnlGeneral;
        private System.Windows.Forms.Panel pnlFixes;
        private System.Windows.Forms.Panel pnlSceneConfig;
        private System.Windows.Forms.Button btnGeneral;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnConsole;
        private System.Windows.Forms.Button btnSceneConfig;
        private System.Windows.Forms.Button btnFixes;
    }
}

