using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            MessageBoxEx.time = args.Length == 0 ? 0 : 5000;
            bool createdNew = false;
            Mutex instance = new Mutex(true, Application.ExecutablePath.Replace("\\", "/"), out createdNew);
            if (createdNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                UpdateCheck check = new UpdateCheck();
                var localTime = Convert.ToDateTime(ConfigurationManager.AppSettings["UpdateTime"]);
                var remoteTime = check.GetUpdateTime();
                var needUpdate = args.Length == 0 ? true : localTime < remoteTime;
                if (needUpdate)
                {
                    var frm = new MainForm();
                    //无参启动不更新时间
                    frm.remoteTime = args.Length == 0 ? localTime : remoteTime;
                    Application.Run(frm);
                }
                if (args.Length == 0)
                    MessageBoxEx.Show("自动更新完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + args[0], "AutoUpdate " + needUpdate.ToString());
                //检查更新自身
                UpdateSelf();
            }
            else
            {
                MessageBoxEx.Show("自动更新正在运行中!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        static void UpdateSelf()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.exe",
                fileTempPath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.exe.tmp",
                batPath = AppDomain.CurrentDomain.BaseDirectory + "AutoUpdate.bat";
            if (!File.Exists(fileTempPath))
                return;
            File.WriteAllText(batPath, string.Format(@"{0}
{1}
{2}",
"del " + filePath,
"ren " + fileTempPath + " AutoUpdate.exe",
"del " + batPath));
            Process.Start(batPath);
        }
    }
}
