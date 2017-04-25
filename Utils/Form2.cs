using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading; 

namespace MonitorCpuTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {  
           //新建一个Stopwatch变量用来统计程序运行时间
           Stopwatch watch = Stopwatch.StartNew();
           //获取本机运行的所有进程ID和进程名,并输出哥进程所使用的工作集和私有工作集
           foreach (Process ps in Process.GetProcesses())
           {
               PerformanceCounter pf1 = new PerformanceCounter("Process", "Working Set - Private", ps.ProcessName);
               PerformanceCounter pf2 = new PerformanceCounter("Process", "Working Set", ps.ProcessName);
             AppendText(string.Format("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集(进程类)", ps.WorkingSet64 / 1024));
               AppendText(string.Format("{0}:{1}  {2:N}KB", ps.ProcessName, "工作集        ", pf2.NextValue() / 1024));
               //私有工作集
             AppendText(string.Format("{0}:{1}  {2:N}KB", ps.ProcessName, "私有工作集    ", pf1.NextValue() / 1024));

           }

           watch.Stop();
           Console.WriteLine(watch.Elapsed); 
             
        }


        delegate void delAppendTxt(string str);
        public void AppendText(string str)
        {
            //textBox1.AppendText(str);
             //   textBox1.AppendText("\n");


                if (!textBox1.InvokeRequired)
                {
                    textBox1.AppendText(str);
                    textBox1.AppendText("\n");
                }
                else
                {
                    //textBox1.Invoke(new Action<string>((pa)AppendText(pa)));
                    Action<string> tempAction = new Action<string>((a) => AppendText(a));
                    textBox1.Invoke(tempAction, new object[] { str });
                    //tempAction.Invoke(str);
                    //textBox1.AppendText(str);
                    //textBox1.AppendText("\n");
                }
        }


        delegate void ShowProgressDelegate(string str);
        private void ShowProgres(string str)
        {
            // 判断是否在线程中访问
            if (!textBox1.InvokeRequired)  //.InvokeRequired)
            {
                // 不是的话直接操作控件
               // textBox1.Text = newPos;
                textBox1.AppendText(str);
                textBox1.AppendText("\n");
                
            }
            else
            {
                // 是的话启用delegate访问
                ShowProgressDelegate showProgress = new ShowProgressDelegate(ShowProgres);
                // 如使用Invoke会等到函数调用结束，而BeginInvoke不会等待直接往后走
               // this.BeginInvoke(showProgress, new object[] { newPos });
                this.BeginInvoke(showProgress, new object[] { str });
            }
        }
        Thread th = null;
        private void button2_Click(object sender, EventArgs e)
        {
            //if (th != null)
            //{
            //    th.Abort();
            //    th = new Thread(ProcessMonitor);
            //    th.Start();
            //}
            //else
            //{
            //    th = new Thread(ProcessMonitor);
            //    th.Start();
            //}
           // Test();
        }

        //void ProcesssMonitor()
        //{
        //    Process[] ProcessArr = Process.GetProcesses().Clone() as Process[];
        //    while (true)
        //    {
               
        //        foreach (Process item in ProcessArr)
        //        {
        //            ProcessMonitor(item);
        //            AppendText("************************************\n");
        //            //Thread.Sleep(1000 * 30);
        //        }
        //        Thread.Sleep(1000 * 15);
        //    }
        //}
        //void ProcessMonitor()
        //{
        //    Process cur = Process.GetCurrentProcess();
        //    do
        //    {
        //         ProcessMonitor(cur);
        //         break;
        //    } while (true);
           
        //}
        //void ProcessMonitor(Process cur)
        //{
        //    //获取当前进程对象
        //    //Process cur = Process.GetCurrentProcess();

        //    PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
        //    PerformanceCounter curpc = new PerformanceCounter("Process", "Working Set", cur.ProcessName);
        //    PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

        //    //上次记录CPU的时间
        //    TimeSpan prevCpuTime = TimeSpan.Zero;
        //    //Sleep的时间间隔
        //    int interval = 1000;

        //    PerformanceCounter totalcpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        //    SystemInfo sys = new SystemInfo();
        //    const int KB_DIV = 1024;
        //    const int MB_DIV = 1024 * 1024;
        //    const int GB_DIV = 1024 * 1024 * 1024;

        //    //第一种方法计算CPU使用率
        //    //当前时间
        //    TimeSpan curCpuTime = cur.TotalProcessorTime;
        //    //计算
        //    double value = (curCpuTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;
        //    prevCpuTime = curCpuTime;

        //    AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集(进程类)", cur.WorkingSet64 / 1024, value));//这个工作集只是在一开始初始化，后期不变
        //    AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集        ", curpc.NextValue() / 1024, value));//这个工作集是动态更新的
        //    //第二种计算CPU使用率的方法
        //    AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024, curtime.NextValue() / Environment.ProcessorCount));
        //    //Thread.Sleep(interval);

        //    //第一种方法获取系统CPU使用情况
        //    AppendText(string.Format("\r系统CPU使用率：{0}%", totalcpu.NextValue()));
        //    //Thread.Sleep(interval);

        //    //第二章方法获取系统CPU和内存使用情况
        //    AppendText(string.Format("\r系统CPU使用率：{0}%，系统内存使用大小：{1}MB({2}GB)", sys.CpuLoad, (sys.PhysicalMemory - sys.MemoryAvailable) / MB_DIV, (sys.PhysicalMemory - sys.MemoryAvailable) / (double)GB_DIV));
        //    //Thread.Sleep(interval);


        //    AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集        ", curpc.NextValue() / 1024, value));//这个工作集是动态更新的
        //    AppendText("--------------------------------------------------------------------------");
        //    //Thread.SpinWait(5000);
        //    //break; 
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            if (th.IsAlive)
            {
                th.Abort();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (Process item in Process.GetProcesses())
            {

                try
                {
                    this.listBox1.Items.Add(item.ProcessName+"*"+item.MainModule.FileName);
                }
                catch (Win32Exception t)
                {
                    listBox1.Items.Add(item.ProcessName);// this.listBox1.Items.Add(t.Message);
                }
                //item.Modules.
                //listBox1.Items.Add(item.ProcessName);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            string pn = listBox1.Text.ToString();
            pn = pn.Split('*')[0];

            Process prc = Process.GetProcessesByName(pn)[0];
          ProcessMonitor(prc);
        }


        //public void Test()
        //{ 
        // //获取当前进程对象 Process cur = Process.GetCurrentProcess();

        //   while (true)
        //   {
        //       Process cur = Process.GetCurrentProcess();
        //       ProcessMonitor(cur);
        //      monitor(cur);
        //      break;
        //   }

        //   return;// 
 
        //  // Console.ReadLine();
        //}


        //public void monitor(Process cur)
        //{


        //    #region 

        //    Process[] p = Process.GetProcessesByName("WindowsFormsApplication1");//获取指定进程信息
        //    // Process[] p = Process.GetProcesses();//获取所有进程信息
        //    string cpu = string.Empty;
        //    string info = string.Empty;

        //    PerformanceCounter pp = new PerformanceCounter();//性能计数器
        //    pp.CategoryName = "Process";//指定获取计算机进程信息  如果传Processor参数代表查询计算机CPU 
        //    pp.CounterName = "% Processor Time";//占有率
        //    //如果pp.CategoryName="Processor",那么你这里赋值这个参数 pp.InstanceName = "_Total"代表查询本计算机的总CPU。
        //    pp.InstanceName = "WindowsFormsApplication1";//指定进程 
        //    pp.MachineName = ".";//pp.
        //    if (p.Length > 0)
        //    {
        //        foreach (Process pr in p)
        //        {
        //            while (true)//1秒钟读取一次CPU占有率。
        //            {
        //                //Thread.Sleep(1000);
        //                info = pr.ProcessName + "内存：" +(Convert.ToInt64(pr.WorkingSet64.ToString()) / 1024).ToString();//得到进程内存
        //                string ss = info + "    CPU使用情况：" + (Math.Round(pp.NextValue(), 2) / Environment.ProcessorCount).ToString() + "%";
        //                AppendText(ss);
        //                //Thread.Sleep(10000);
        //               // break;
        //            }
        //        }
        //    }
        //    #endregion 


        //    PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
        //    PerformanceCounter curpc = new PerformanceCounter("Process", "Working Set", cur.ProcessName);
        //    PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

        //    //上次记录CPU的时间
        //    TimeSpan prevCpuTime = TimeSpan.Zero;//ur.TotalProcessorTime;

        //    TimeSpan start = cur.TotalProcessorTime;
        //    Thread.Sleep(1000);
        //    TimeSpan stop = cur.TotalProcessorTime;

        //    double i = (stop - start).TotalMilliseconds;

        //    //Sleep的时间间隔
        //    int interval = 1000;

        //    PerformanceCounter totalcpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        //    SystemInfo sys = new SystemInfo();
        //    const int KB_DIV = 1024;
        //    const int MB_DIV = 1024 * 1024;
        //    const int GB_DIV = 1024 * 1024 * 1024;
        //    while (true)
        //    {
        //        //第一种方法计算CPU使用率
        //        //当前时间
        //        TimeSpan curCpuTime = cur.TotalProcessorTime;
        //        //计算
        //        double value = (curCpuTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;
        //        prevCpuTime = curCpuTime;

        //       AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集(进程类)", cur.WorkingSet64 / 1024,value));//这个工作集只是在一开始初始化，后期不变
        //        AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集        ", curpc.NextValue() / 1024,value));//这个工作集是动态更新的
        //        //第二种计算CPU使用率的方法
        //       AppendText(string.Format("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024,curtime.NextValue()/Environment.ProcessorCount));
        //        //Thread.Sleep(interval);

        //        //第一种方法获取系统CPU使用情况
        //        AppendText(string.Format("\r系统CPU使用率：{0}%", totalcpu.NextValue()));
        //        //Thread.Sleep(interval);

        //        //第二章方法获取系统CPU和内存使用情况
        //        AppendText(string.Format("\r系统CPU使用率：{0}%，系统内存使用大小：{1}MB({2}GB)", sys.CpuLoad, (sys.PhysicalMemory - sys.MemoryAvailable) / MB_DIV, (sys.PhysicalMemory - sys.MemoryAvailable) / (double)GB_DIV));

        //        Thread.Sleep(interval);
        //        AppendText("*****************************");
        //        break;
        //    }
        //}
         


             
        
    }
}
