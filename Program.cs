using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// orp 2015.10.28 
/// </summary>
namespace MonitorCpuTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Diagnostics;
//using System.Threading;

//namespace CSharpPerformance
//{//该程序可以实时监控所有进程或者指定进程的工作集、私有工作集
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            //新建一个Stopwatch变量用来统计程序运行时间
//            Stopwatch watch = Stopwatch.StartNew();
//            //获取本机运行的所有进程ID和进程名,并输出哥进程所使用的工作集和私有工作集
//            foreach (Process ps in Process.GetProcesses())
//            {
//                PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);
//                PerformanceCounter pf2 = new PerformanceCounter("Process", "Working Set", ps.ProcessName);
//                Console.WriteLine("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集(进程类)", ps.WorkingSet64 / 1024);
//                Console.WriteLine("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集        ", pf2.NextValue() / 1024);
//                //私有工作集
//                Console.WriteLine("{0}:{1}  {2:N}KB", ps.ProcessName, "私有工作集    ", pf1.NextValue() / 1024);

//            }

//            watch.Stop();
//            Console.WriteLine(watch.Elapsed);
//            Console.ReadLine();
//        }
//    }
//}