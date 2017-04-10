using System;
using System.Windows.Forms;

namespace SocketMessenger
{
    public partial class Main : Form
    {
        private Server server;
        private Client client;
        private Status status;
        private delegate void ShowForm();//視窗事件
        public Main(string[] args)
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            status = Status.None;
        }

        #region 發送訊息

        //
        private void btn_sendMess_Click(object sender, EventArgs e)
        {
            SendMsg(txt_sendMsg.Text);
        }

        //
        private void SendMsgByKeyDown(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                SendMsg(txt_sendMsg.Text);
            }
        }

        //發送訊息
        private void SendMsg(string msg)
        {
            if (string.IsNullOrWhiteSpace(txt_sendMsg.Text))
                return;
            switch (status)
            {
                case Status.None:
                    break;
                case Status.Client:
                    client.SendMsg(txt_sendMsg.Text);
                    break;
                case Status.Server:
                    server.SendMsg(txt_sendMsg.Text);
                    break;
            }
        }

        #endregion

        #region 伺服器端

        //建立伺服器連線
        private void btn_newServer_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                this.Text = "Server";
                status = Status.Server;
                server = new Server();
                server.ShowErrMsg += ServerErr;
                server.ShowMsg += ShowStr;
                server.Listen(txt_userIP.Text,txt_userName.Text);
            }
        }

        //顯示伺服器錯誤訊息
        private void ServerErr(string msg, bool isClose)
        {
            ShowMsg(msg);
            
            //是否關閉伺服器
            if (isClose)
            {
                //關閉
                server = null;
                status = Status.None;
            }
        }

        //關閉伺服器端連線
        private void btn_closeServer_Click(object sender, EventArgs e)
        {
            if(status != Status.Server)
            {
                return;
            }
            server.Close();
            status = Status.None;
            server = null;
        }

        #endregion

        #region 用戶端

        //建立用戶端連線
        private void btn_newLink_Click(object sender, EventArgs e)
        {
            if(client == null)
            {
                this.Text = "Client";
                status = Status.Client;
                client = new Client();
                client.ShowErrMsg += ClientErr;
                client.ShowMsg += ShowStr;
                client.Connect(txt_userIP.Text, txt_userName.Text);
            }
        }

        //關閉用戶端
        private void ClientErr(string msg, bool isClose)
        {
            ShowMsg(msg);
            //是否關閉伺服器
            if (isClose)
            {
                client = null;
                status = Status.None;
            }
        }

        //關閉用戶端連線
        private void btn_closeLink_Click(object sender, EventArgs e)
        {
            if(status != Status.Client)
            {
                return;
            }
            client.Close();
            client = null;
            status = Status.None;
        }

        #endregion

        #region 其他

        //新更介面
        private void ShowStr(string msg)
        {
            Invoke(new ShowForm(() =>
            {
                lbx_msg.Items.Add(msg);
                lbx_msg.SelectedIndex = lbx_msg.Items.Count - 1;
            }));
        }

        //顯示錯誤視窗
        private void ShowMsg(string msg)
        {
            MessageBox.Show(msg);
        }

        public enum Status
        {
            None = 0,
            Server = 1,
            Client = 2
        }

        #endregion
    }
}
