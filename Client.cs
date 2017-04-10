
using System;
using System.Text;
using System.Net.Sockets;

namespace SocketMessenger
{
    public class Client: BaseClass
    {
        public SocketInfo clientInfo = new SocketInfo();//使用者連線資訊
        public Socket clientConn;//使用者連線
        public SocketAsyncEventArgs sendArgs;//使用者發送物件
        public SocketAsyncEventArgs connectArgs;//使用者連線物件

        /// <summary>
        /// 建立連線(使用者端)
        /// </summary>
        /// <param name="ip"></param>
        public void Connect(string ip, string userName)
        {
            clientInfo.Name = userName;
            clientInfo.ClientSocket = clientConn;

            if (SetAddress(ip))
            {
                return;
            }
            clientConn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            connectArgs = socketEvent.Pop();
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(CompletedConn);
            connectArgs.RemoteEndPoint = endPoint;
            connectArgs.UserToken = clientInfo;
            if (!clientConn.ConnectAsync(connectArgs))
            {
                CompletedConn(null, connectArgs);
            }
        }

        //連線成功時處理
        private void CompletedConn(object obj, SocketAsyncEventArgs connArgs)
        {
            if (connArgs.SocketError == SocketError.Success)
            {
                Socket connectSocket = connArgs.ConnectSocket;
                clientConn = connArgs.ConnectSocket;
                connArgs.SetBuffer(new byte[8192], 0, 8192);

                SocketInfo clcientInfo = (SocketInfo)connArgs.UserToken;
                clientInfo.ReceiveArgs = connArgs;
                clientInfo.ClientSocket = connectSocket;

                connArgs.Completed -= CompletedConn;
                connArgs.Completed += new EventHandler<SocketAsyncEventArgs>(CompleteReceive);
                clientConn.ReceiveAsync(connArgs);
            }
            else
            {
                //關閉連線
                Close();
            }
        }

        //接收訊息時處理
        private void CompleteReceive(object obj, SocketAsyncEventArgs receiveArgs)
        {
            try
            {
                if (receiveArgs.BytesTransferred > 0)
                {
                    if (receiveArgs.SocketError == SocketError.Success)
                    {
                        string str = Encoding.UTF8.GetString(receiveArgs.Buffer);//讀取資料流
                        OnShowMsg(str);//更新介面

                        //重新設定串流空間，繼續監聽
                        SocketInfo clientInfo = (SocketInfo)receiveArgs.UserToken;
                        Socket receive = clientInfo.ClientSocket;
                        byte[] msg = new byte[8192];
                        receiveArgs.SetBuffer(msg, 0, msg.Length);
                        receive.ReceiveAsync(receiveArgs);
                    }
                    else
                    {
                        ProcessErr(receiveArgs);
                    }
                }
                else
                {
                    Close();
                }
            }
            catch(Exception ex)
            {
                logs.Error(ex.Message + ex.StackTrace);//輸出錯誤訊息
            }
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            byte[] str = Encoding.UTF8.GetBytes(clientInfo.Name + ":" + msg);
            if(sendArgs == null)
            {
                sendArgs = socketEvent.Pop();
            }
            sendArgs.SetBuffer(str, 0, str.Length);
            clientConn.SendAsync(sendArgs);
        }

        /// <summary>
        /// 關閉連線
        /// </summary>
        /// <param name="errArgs"></param>
        public void ProcessErr(SocketAsyncEventArgs errArgs)
        {
            SocketInfo clientInfo = (SocketInfo)errArgs.UserToken;
            Socket err = clientInfo.ClientSocket;
            try
            {
                if (err.Connected)
                {
                    err.Shutdown(SocketShutdown.Both);//關閉連線
                }
            }
            catch (Exception ex)
            {
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 關閉連線
        /// </summary>
        public void Close()
        {
            try
            {
                clientConn.Shutdown(SocketShutdown.Both);
                clientConn.Close();
            }
            catch (Exception ex)
            {
                logs.Error(ex.Message + ex.StackTrace);
            }
        }
    }
}
