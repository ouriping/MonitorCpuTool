using System;
using System.IO;
//using SocketNet;
namespace Utils
{

    public class LogInfoPath
    {
        public static long MaxFileLength = 1024 * 1024 * 1;

        public static string _directionPath = AppDomain.CurrentDomain.BaseDirectory;//默认路径
        private static string _logPath = string.Empty;

        //public static string GetLogPath()
        //{
        //    if (string.IsNullOrEmpty(_logPath))
        //    {
        //        _logPath = (_directionPath == "" ? AppDomain.CurrentDomain.BaseDirectory : _directionPath);
        //            }
        //}

        public static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                {
                    _logPath = _directionPath + "CpuLog/Log_" + System.DateTime.Now.ToString("yyyy-MM-dd_hhmmss") + ".txt";
                    //_directionPath = Path.GetDirectoryName(_logPath);
                }
                else
                {
                    FileInfo fInfo = new FileInfo(_logPath);
                    if (fInfo.Exists)
                    {
                        if (fInfo.Length > MaxFileLength)
                        {
                            _logPath = string.Empty;
                            _logPath = LogPath;
                            // LogPath = string.Empty;//注意是LogPath
                        }
                    }
                    else
                    {
                        _logPath = string.Empty;
                    }
                }
                if (!Directory.Exists(_directionPath))
                {
                    Directory.CreateDirectory(_directionPath);
                }

                return _logPath;
            }
            set
            {
                _logPath = value;
            }
        }
    }

    /// <summary>
    /// 日志处理对象,包含对日志的相关处理过程
    /// </summary>
    public class Log
    {
        public static string _directionPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 日志目录 最后以/号结尾
        /// </summary>
        public static string DirectionPath
        {
            get
            {
                return _directionPath;// string.Empty;
            }
            set
            {
                _directionPath = value;
                _logPath = string.Empty;
            }
        }
        private static string _logPath = string.Empty;
        public static string LogPath
        {
            get
            {
                if (string.IsNullOrEmpty(_logPath))
                {
                    _logPath = DirectionPath + "CpuLog\\Log_" + System.DateTime.Now.ToString("yyyy-MM-dd_hhmmss") + ".txt";
                    //_directionPath = Path.GetDirectoryName(_logPath);
                }
                else
                {
                    FileInfo fInfo = new FileInfo(_logPath);
                    if (fInfo.Exists)
                    {
                        if (fInfo.Length > (1024 * 1024))
                        {
                            _logPath = string.Empty;
                            _logPath = LogPath;
                            // LogPath = string.Empty;//注意是LogPath
                        }
                    }
                    else
                    {//找不到文件
                        string direPath = Path.GetDirectoryName(_logPath);
                        if (!Directory.Exists(direPath))
                        {
                            Directory.CreateDirectory(direPath);
                        }
                    }
                }
                //if (!Directory.Exists(_directionPath))
                //{
                //    Directory.CreateDirectory(_directionPath);
                //}

                return _logPath;
            }
            //set
            //{
            //    _logPath = value;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void WriteLog(string str)
        {
            try
            {
                StreamWriter sw = new StreamWriter(LogPath, true);
                sw.Write(str + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("写入日志错误!" + ex.Message);
            }

        }

        /*
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="str">要写入的内容</param>
        /// <param name="level">日志等级 1普通 2详细 3错误</param>
        public static void Write(string str, int level)
        {
            //if (level == 1 || level == 2)
            //{
                try
                {
                    //Console.ForegroundColor = Config.msgColor[level - 1];
                    //Console.WriteLine(str);
                    //Console.Write(">");
                }
                catch
                { }
            //}
            //if (level == 3)//功能日志
            //    WriteF(str);
            if (level == 4)//错误日志
            {
                try
                {
                    StreamWriter sw = new StreamWriter("Log/GameLog_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true);
                    sw.Write("时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + str + "\r\n");
                    sw.Flush();
                    sw.Close();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 写消息收发日志
        /// </summary>
        /// <param name="str"></param>
        public static void WriteC(string str)
        {
            try
            {
                StreamWriter sw = new StreamWriter("Log/MsgLog_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true);
                sw.Write(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + str + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch
            { }
        }

        /// <summary>
        /// 写功能记录日志
        /// </summary>
        /// <param name="str"></param>
        public static void WriteF(string str)
        {
            try
            {
                StreamWriter sw = new StreamWriter("Log/FunLog_" + System.DateTime.Now.ToString("yyyyMMdd_") + DateTime.Now.Hour + ".txt", true);
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    " + str + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="str">>要写入的内容 日志等级默认为1普通</param>
        public static void Write(string str)
        {
            //Console.ForegroundColor = Config.msgColor[0];
            //Console.WriteLine(str);
            //Console.Write(">");
        }

        /// <summary>
        /// 写一个进度
        /// </summary>
        /// <param name="i">进度位置</param>
        public static void WriteWait(int w)
        {
            //Console.ForegroundColor = Config.msgColor[1];
            //Console.Write("\r" + getWaitStr(w / 2) + "已完成" + w + "%");
            //System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// 简易的进度条
        /// </summary>
        /// <param name="w">当前进度</param>
        /// <returns></returns>
        private static string getWaitStr(int w)
        {
            string str = "";
            for (int i = 0; i < w; i++)
            {
                str += "=";
            }
            for (int i = w; i < 50; i++)
            {
                str += " ";
            }
            return str;
        }

        /// <summary>
        /// 初始化日志管理器
        /// </summary>
        public static void Start(string title)
        {
            Console.Title = title;
            Log.Write(title);
            Log.Write("<CTRL+C> Exit Server");
            Log.Write("                                        ");
            Log.Write("CopyRight Masklord Entertainmant");
            Log.Write("\n\n");
        }
         */
    }
}
