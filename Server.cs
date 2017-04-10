using System;
using System.Collections.Concurrent;
using System.Text;
using System.Net.Sockets;

namespace SocketMessenger
{
    public class Server:BaseClass
    {
        public ConcurrentDictionary<long, SocketInfo> connClient = new ConcurrentDictionary<long, SocketInfo>();//存使用者連線資訊的容器
        public Socket serverListen;//伺服器監聽
        public string serverName;//伺服器名稱
        public long connectNumber = 0;//連線ID流水號
        public SocketAsyncEventArgs listenArgs;//伺服器監聽物件

        /// <summary>
        /// 監聽新連線
        /// </summary>
        /// <param name="ip"></param>
        public void Listen(string ip, string userName)
        {
            //設定IP失敗，則不往下處理
            if (SetAddress(ip))
            {
                return;
            }
            serverName = userName;

            serverListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverListen.SendTimeout = 1000;
            serverListen.Bind(endPoint);
            serverListen.Listen(2);

            listenArgs = socketEvent.Pop();//取出args物件
            listenArgs.Completed += new EventHandler<SocketAsyncEventArgs>(CompletedAccept);//榜定監聽事件

            //異步監聽
            if (!serverListen.AcceptAsync(listenArgs))
            {
                CompletedAccept(null, listenArgs);
            }
        }

        //監聽到新連線時
        private void CompletedAccept(object obj, SocketAsyncEventArgs acceptArgs)
        {
            try
            {
                if (acceptArgs.SocketError == SocketError.Success)
                {
                    SocketAsyncEventArgs receiveArgs = socketEvent.Pop();
                    Socket receive = acceptArgs.AcceptSocket;

                    //建立連線client 資訊
                    SocketInfo clientInfo = new SocketInfo();
                    clientInfo.ID = connectNumber;
                    clientInfo.ClientSocket = receive;
                    clientInfo.ReceiveArgs = receiveArgs;
                    connClient.TryAdd(clientInfo.ID, clientInfo);
                    connectNumber++;

                    byte[] msg = new byte[8192];
                    receiveArgs.SetBuffer(msg, 0,msg.Length);
                    receiveArgs.Completed += CompletedReceive;//榜定事件
                    receiveArgs.UserToken = clientInfo;//封裝clientInfo物件

                    //監聽有無傳送資料
                    if (!receive.ReceiveAsync(receiveArgs))
                    {
                        CompletedReceive(null, receiveArgs);
                    }

                    //繼續監聽新連線
                    acceptArgs.AcceptSocket = null;
                    if (!serverListen.AcceptAsync(acceptArgs))
                    {
                        CompletedAccept(null, listenArgs);
                    }
                }
                else
                {
                    //關閉監聽
                    if (serverListen.Connected)
                    {
                        serverListen.Close();
                    }
                    RecyclingEvent(acceptArgs);
                }
            }
            catch (Exception ex)
            {
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        //接收訊息事件
        private void CompletedReceive(object obj, SocketAsyncEventArgs receiveArgs)
        {
            try
            {
                if (receiveArgs.BytesTransferred > 0)
                {
                    if (receiveArgs.SocketError == SocketError.Success)
                    {
                        string str = Encoding.UTF8.GetString(receiveArgs.Buffer);//讀取資料流
                        OnShowMsg(str);//更新介面

                        //廣播到其他用戶端
                        SendToAll(str);

                        //重新設定串流空間，繼續監聽
                        SocketInfo clientInfo = (SocketInfo)receiveArgs.UserToken;
                        Socket receive = clientInfo.ClientSocket;

                        byte[] msg = new byte[8192];
                        receiveArgs.SetBuffer(msg, 0, msg.Length);
                        receive.ReceiveAsync(receiveArgs);
                    }
                    else
                    {
                        ErrProcess(receiveArgs);
                    }
                }
                else
                {
                    CloseClient(receiveArgs);//關閉連線
                }
            }
            catch (Exception ex)
            {
                //輸出錯誤訊息
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 連線錯誤時
        /// </summary>
        /// <param name="errArgs"></param>
        public void ErrProcess(SocketAsyncEventArgs errArgs)
        {
            SocketInfo clientInfo = (SocketInfo)errArgs.UserToken;
            Socket err = clientInfo.ClientSocket;
            try
            {
                if (err.Connected)
                {
                    err.Shutdown(SocketShutdown.Both);//關閉連線
                }
                CloseClient(errArgs);
            }
            catch (Exception ex)
            {
                //印出錯誤訊息
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 關閉連線的客戶端，並釋放資源
        /// </summary>
        public void CloseClient(SocketAsyncEventArgs closeArgs)
        {
            SocketInfo clientInfo = (SocketInfo)closeArgs.UserToken;//取出要關閉的clientInfo
            Socket client = clientInfo.ClientSocket;//取出要關閉的client
            try
            {
                if (client.Connected)
                {
                    client.Close();
                }
                connClient.TryRemove(clientInfo.ID, out clientInfo);//移除伺服器的客戶端資訊

                //釋放資源
                clientInfo.ClientSocket.Dispose();
                clientInfo.ClientSocket = null;

                //釋放 發送的資源
                if (clientInfo.SendArgs != null)
                {
                    SocketAsyncEventArgs args = clientInfo.SendArgs;
                    RecyclingEvent(args);
                }

                //釋放 接收的資源
                if(clientInfo.ReceiveArgs != null)
                {
                    SocketAsyncEventArgs args = clientInfo.ReceiveArgs;
                    RecyclingEvent(args);
                }
            }
            catch (Exception ex)
            {
                //印出錯誤訊息
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 回收不使用的物件
        /// </summary>
        /// <param name="args"></param>
        public void RecyclingEvent(SocketAsyncEventArgs args)
        {
            args.UserToken = null;
            args.AcceptSocket = null;
            args.RemoteEndPoint = null;
            args.Completed -= CompletedAccept;
            args.Completed -= CompletedReceive;
            args.SetBuffer(null, 0, 0);
            socketEvent.Push(args);
        }

        /// <summary>
        /// 伺服器關閉
        /// </summary>
        public void Close()
        {
            try
            {
                //關閉所有使用端連線
                foreach (long clientID in connClient.Keys)
                {
                    connClient[clientID].ClientSocket.Shutdown(SocketShutdown.Both);
                    connClient[clientID].ClientSocket.Close();
                }
                //關閉監聽連線
                serverListen.Close();
            }
            catch (Exception ex)
            {
                logs.Error(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 廣播給所有人
        /// </summary>
        /// <param name="msg"></param>
        public void SendToAll(string str)
        {
            foreach (long clientID in connClient.Keys)
            {
                byte[] msg = Encoding.UTF8.GetBytes(str);//字串編碼

                //檢察發送物件是否存在
                if (connClient[clientID].SendArgs == null)
                {
                    connClient[clientID].SendArgs = socketEvent.Pop();
                }

                //設定buffer
                connClient[clientID].SendArgs.SetBuffer(msg, 0, msg.Length);
                //開始發送訊息
                connClient[clientID].ClientSocket.SendAsync(connClient[clientID].SendArgs);
            }
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="str"></param>
        public void SendMsg(string str)
        {
            OnShowMsg(serverName + ":" + str);
            SendToAll(serverName + ":" + str);
        }
    }
}
