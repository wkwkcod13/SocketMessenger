
using System;
using NLog;
using System.Configuration;

namespace SocketMessenger
{
    public enum LogMode
    {
        error,           //錯誤訊息
    }

    public class LogFile
    {
        private Logger loggerByDay = LogManager.GetLogger("FollowDay");      //以日期為Log紀錄區分
        private Logger loggerByHour = LogManager.GetLogger("FollowHour");    //以小時為Log紀錄區分

        public LogFile() {}

        #region Log紀錄方法

        public void Error(string text, params object[] args)
        {
            Message(LogMode.error, text, args);
        }

        #endregion

        private void Message(LogMode logMode, string text, params object[] args)
        {
            //處理字串
            text = (args.Length == 0) ? text : string.Format(text, args);

            LogEventInfo logEventInfo = new LogEventInfo();
            logEventInfo.TimeStamp = DateTime.Now;

            if (logMode != LogMode.error)
            {
                    logEventInfo.Level = LogLevel.Info;
                    logEventInfo.Properties["FolderName"] = logMode.ToString();
                    logEventInfo.Properties["LogName"] = logMode.ToString() + string.Format("_{0}", DateTime.Now.Hour.ToString());
                    loggerByHour.Log(logEventInfo);
            }
            else
            {

                logEventInfo.Properties["LogName"] = logMode.ToString();
                logEventInfo.Level = (logMode != LogMode.error) ? LogLevel.Info : LogLevel.Error;
                loggerByDay.Log(logEventInfo);
            }
        }
    }
}
