namespace SocketMessenger
{
    partial class Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbx_msg = new System.Windows.Forms.ListBox();
            this.txt_sendMsg = new System.Windows.Forms.TextBox();
            this.btn_sendMess = new System.Windows.Forms.Button();
            this.btn_closeServer = new System.Windows.Forms.Button();
            this.btn_newServer = new System.Windows.Forms.Button();
            this.txt_userIP = new System.Windows.Forms.TextBox();
            this.txt_userName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_newLink = new System.Windows.Forms.Button();
            this.btn_closeLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbx_msg
            // 
            this.lbx_msg.FormattingEnabled = true;
            this.lbx_msg.ItemHeight = 12;
            this.lbx_msg.Location = new System.Drawing.Point(12, 80);
            this.lbx_msg.Name = "lbx_msg";
            this.lbx_msg.Size = new System.Drawing.Size(388, 220);
            this.lbx_msg.TabIndex = 0;
            // 
            // txt_sendMsg
            // 
            this.txt_sendMsg.Location = new System.Drawing.Point(13, 307);
            this.txt_sendMsg.Name = "txt_sendMsg";
            this.txt_sendMsg.Size = new System.Drawing.Size(306, 22);
            this.txt_sendMsg.TabIndex = 1;
            this.txt_sendMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendMsgByKeyDown);
            // 
            // btn_sendMess
            // 
            this.btn_sendMess.Location = new System.Drawing.Point(325, 307);
            this.btn_sendMess.Name = "btn_sendMess";
            this.btn_sendMess.Size = new System.Drawing.Size(75, 23);
            this.btn_sendMess.TabIndex = 2;
            this.btn_sendMess.Text = "送出";
            this.btn_sendMess.UseVisualStyleBackColor = true;
            this.btn_sendMess.Click += new System.EventHandler(this.btn_sendMess_Click);
            // 
            // btn_closeServer
            // 
            this.btn_closeServer.Location = new System.Drawing.Point(325, 14);
            this.btn_closeServer.Name = "btn_closeServer";
            this.btn_closeServer.Size = new System.Drawing.Size(75, 23);
            this.btn_closeServer.TabIndex = 4;
            this.btn_closeServer.Text = "關閉伺服器";
            this.btn_closeServer.UseVisualStyleBackColor = true;
            this.btn_closeServer.Click += new System.EventHandler(this.btn_closeServer_Click);
            // 
            // btn_newServer
            // 
            this.btn_newServer.Location = new System.Drawing.Point(244, 13);
            this.btn_newServer.Name = "btn_newServer";
            this.btn_newServer.Size = new System.Drawing.Size(75, 23);
            this.btn_newServer.TabIndex = 5;
            this.btn_newServer.Text = "建立伺服器";
            this.btn_newServer.UseVisualStyleBackColor = true;
            this.btn_newServer.Click += new System.EventHandler(this.btn_newServer_Click);
            // 
            // txt_userIP
            // 
            this.txt_userIP.Location = new System.Drawing.Point(71, 45);
            this.txt_userIP.Name = "txt_userIP";
            this.txt_userIP.Size = new System.Drawing.Size(167, 22);
            this.txt_userIP.TabIndex = 6;
            this.txt_userIP.Text = "127.0.0.1";
            // 
            // txt_userName
            // 
            this.txt_userName.Location = new System.Drawing.Point(71, 15);
            this.txt_userName.Name = "txt_userName";
            this.txt_userName.Size = new System.Drawing.Size(167, 22);
            this.txt_userName.TabIndex = 7;
            this.txt_userName.Text = "小明";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "IP";
            // 
            // btn_newLink
            // 
            this.btn_newLink.Location = new System.Drawing.Point(244, 45);
            this.btn_newLink.Name = "btn_newLink";
            this.btn_newLink.Size = new System.Drawing.Size(75, 23);
            this.btn_newLink.TabIndex = 10;
            this.btn_newLink.Text = "建立連線";
            this.btn_newLink.UseVisualStyleBackColor = true;
            this.btn_newLink.Click += new System.EventHandler(this.btn_newLink_Click);
            // 
            // btn_closeLink
            // 
            this.btn_closeLink.Location = new System.Drawing.Point(325, 45);
            this.btn_closeLink.Name = "btn_closeLink";
            this.btn_closeLink.Size = new System.Drawing.Size(75, 23);
            this.btn_closeLink.TabIndex = 11;
            this.btn_closeLink.Text = "關閉連線";
            this.btn_closeLink.UseVisualStyleBackColor = true;
            this.btn_closeLink.Click += new System.EventHandler(this.btn_closeLink_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 344);
            this.Controls.Add(this.btn_closeLink);
            this.Controls.Add(this.btn_newLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_userName);
            this.Controls.Add(this.txt_userIP);
            this.Controls.Add(this.btn_newServer);
            this.Controls.Add(this.btn_closeServer);
            this.Controls.Add(this.btn_sendMess);
            this.Controls.Add(this.txt_sendMsg);
            this.Controls.Add(this.lbx_msg);
            this.Name = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbx_msg;
        private System.Windows.Forms.TextBox txt_sendMsg;
        private System.Windows.Forms.Button btn_sendMess;
        private System.Windows.Forms.Button btn_closeServer;
        private System.Windows.Forms.Button btn_newServer;
        private System.Windows.Forms.TextBox txt_userIP;
        private System.Windows.Forms.TextBox txt_userName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_newLink;
        private System.Windows.Forms.Button btn_closeLink;
    }
}

