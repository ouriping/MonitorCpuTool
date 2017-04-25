//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MonitorCpuTool.sysApi
//{
//   public  class SystemInfo
//    {
//    }

//}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;

namespace MonitorCpuTool
{

    /// <summary>
    /// cpu信息
    /// </summary>
    public class CpuPerformance
    {

        /// <summary>
        /// 获取cpu使用率
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        public static List<Tuple<string, string>> GetCpuByProcessName(string ProcessName)
        {
            //Tuple<string, string> aa = new Tuple<string, string>();
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            Process[] p = Process.GetProcessesByName(ProcessName);//获取指定进程信息
            if (p.Length == 0)
            {
                return list;// return "";
            }
            // Process[] p = Process.GetProcesses();//获取所有进程信息
            string cpu = string.Empty;
            string info = string.Empty;

            PerformanceCounter pp = new PerformanceCounter();//性能计数器
            pp.CategoryName = "Process";//指定获取计算机进程信息  如果传Processor参数代表查询计算机CPU 
            pp.CounterName = "% Processor Time";//占有率
                                                //如果pp.CategoryName="Processor",那么你这里赋值这个参数 pp.InstanceName = "_Total"代表查询本计算机的总CPU。
            pp.InstanceName = ProcessName;// "WindowsFormsApplication1";//指定进程 
            pp.MachineName = ".";//pp.
            pp.NextValue();

            if (p.Length > 0)
            {
                foreach (Process pr in p)
                {
                    pp.NextValue();
                    string useRate = ""; ;// pp.NextValue();
                    Thread.Sleep(1000);//间隔一秒,误差还可以接受
                    useRate = (Math.Round(pp.NextValue(), 4) / Environment.ProcessorCount).ToString() + "%";
                    // list.Add(Math.Round(useRate, 2).ToString());
                    //string Domain = pr.StartInfo.Domain;
                    string FileName = string.Empty;
                    try
                    {
                        FileName = pr.MainModule.FileName;// pr.StartInfo.FileName;
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                    Tuple<string, string> temp = new Tuple<string, string>(useRate, FileName);
                    list.Add(temp);
                }
            }
            return list;
        }
    }

}


