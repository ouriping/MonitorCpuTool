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
using System.Timers;

/// <summary>
/// 欧日平 2014.10.28
/// </summary>
namespace MonitorCpuTool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            button4_Click(null, null);
        }
        //delegate void delAppendTxt(string str);
        public void AppendText(string str)
        {
            if (!textBox1.InvokeRequired)
            {
                textBox1.AppendText(str);
                textBox1.AppendText("\n");
                Utils.Log.WriteLog(str);//写入日志 
            }
            else
            {
                Action<string> tempAction = new Action<string>((a) => AppendText(a));
                textBox1.Invoke(tempAction, new object[] { str });
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (Process item in Process.GetProcesses())
            {

                try
                {
                    this.listBox1.Items.Add(item.ProcessName);//+"*"+item.MainModule.FileName);
                }
                catch (Win32Exception t)
                {
                    listBox1.Items.Add(item.ProcessName);// this.listBox1.Items.Add(t.Message);
                }
            }

        }
        System.Timers.Timer t = new System.Timers.Timer();
        /// <summary>
        /// 开始监听CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            if (listBox2.Items.Count == 0)
            {
                MessageBox.Show("监听列表没有数据");
                return;
            }
            //Monitor();
            Thread th = new Thread(Monitor);
            th.Start();
            int i = 60;
            if (!int.TryParse(textBox2.Text, out i))
            {
                textBox2.Text = i.ToString();
            }
            t.Stop();
            t.Interval = i * 1000;
            t.Elapsed -= new ElapsedEventHandler(t_Elapsed);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
            this.Text = "监听中...";
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Monitor();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Monitor()
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.Items.Count > i)
                {
                    Monitor(listBox2.Items[i].ToString());
                }
            }
        }
        /// <summary>
        /// 进程名,,该名不包含exe后缀
        /// </summary>
        /// <param name="pn"></param>
        public void Monitor(string pn)
        {
            List<Tuple<string, string>> list = CpuPerformance.GetCpuByProcessName(pn);
            AppendText("[" + pn + "]------------------------Start----------------------" + DateTime.Now.ToString());
            foreach (var item in list)
            {
                AppendText("CPU:" + item.Item1);
                AppendText("Path:" + item.Item2);
            }
            AppendText("[" + pn + "]-----------------------end---------------------\n");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            t.Stop();
            string pn = listBox1.Text.ToString();
            pn = pn.Split('*')[0];
            if (string.IsNullOrEmpty(pn))
            {
                return;
            }
            if (!listBox2.Items.Contains(pn))
            {
                listBox2.Items.Add(pn);
            }
            button5_Click(null, null);//开始监听CPU
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            t.Stop();
            this.Text = "已停止";
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(Utils.Log.DirectionPath))
            {
                System.Diagnostics.Process.Start(Utils.Log.DirectionPath);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Utils.Log.DirectionPath == "" ? AppDomain.CurrentDomain.BaseDirectory : Utils.Log.DirectionPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.Directory.Exists(fbd.SelectedPath))
                {
                    Utils.Log.DirectionPath = fbd.SelectedPath + "\\";
                }
                else
                {
                    MessageBox.Show("文件夹不存在");
                }
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Stop();
            Application.ExitThread();
            Application.Exit();
            this.Dispose();
            this.Close();
        }


    }
}
