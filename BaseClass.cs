using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SocketMessenger
{
    public class BaseClass
    {
        #region 事件

        public delegate void showErrMsg(string msg, bool isClose);
        /// <summary>
        /// 事件丟出錯誤訊息
        /// </summary>
        public event showErrMsg ShowErrMsg;

        public delegate void showMsg(string msg);
        /// <summary>
        /// 事件更新介面
        /// </summary>
        public event showMsg ShowMsg;

        #endregion

        protected LogFile logs = new LogFile();//紀錄錯誤日誌
        protected const int port = 3059;//連線Port
        protected Stack<SocketAsyncEventArgs> socketEvent;//連線時使用的物件
        protected IPAddress address;//IP
        protected IPEndPoint endPoint;//端口


        public BaseClass()
        {
            socketEvent = new Stack<SocketAsyncEventArgs>();
            for (int i = 0; i < 100; i++)
            {
                socketEvent.Push(new SocketAsyncEventArgs());
            }
        }

        /// <summary>
        /// 丟出錯誤訊息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isClose"></param>
        public void OnShowErrMsg(string msg, bool isClose)
        {
            ShowErrMsg(msg, isClose);
        }

        /// <summary>
        /// 更新介面
        /// </summary>
        /// <param name="msg"></param>
        public void OnShowMsg(string msg)
        {
            ShowMsg(msg);
        }

        /// <summary>
        /// 設定連線資訊
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool SetAddress(string ip)
        {
            bool isClose = false;
            if (!IPAddress.TryParse(ip, out address))
            {
                isClose = true;
                //丟出錯誤訊息
                OnShowErrMsg("請輸入正確IP", isClose);
            }
            else
            {
                endPoint = new IPEndPoint(address, port);
            }
            return isClose;
        }
    }

    public class SocketInfo
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 連線ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Socket物件
        /// </summary>
        public Socket ClientSocket { get; set; }

        /// <summary>
        /// 接收
        /// </summary>
        public SocketAsyncEventArgs ReceiveArgs { get; set; }

        /// <summary>
        /// 傳送
        /// </summary>
        public SocketAsyncEventArgs SendArgs { get; set; }
    }
}
