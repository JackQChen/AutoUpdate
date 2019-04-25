using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace AutoUpdate
{
    public class UpdateCheck
    {
        string ftpUserName, ftpPassword;
        string configPath, updateTimeName = "UpdateTime.dat";

        public UpdateCheck()
        {
            this.ftpUserName = ConfigurationManager.AppSettings["UserName"];
            this.ftpPassword = ConfigurationManager.AppSettings["Password"];
            this.configPath = ConfigurationManager.AppSettings["ConfigPath"];
        }

        public DateTime GetUpdateTime()
        {
            return this.GetUpdateTime(this.configPath, updateTimeName);
        }

        public DateTime GetUpdateTime(string remoteFilePath, string remoteFileName)
        {
            FtpWebRequest reqFTP;
            DateTime dtUpdate = DateTime.MinValue;
            try
            {
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}//{1}", remoteFilePath, remoteFileName)));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(ftpStream, Encoding.UTF8);
                dtUpdate = Convert.ToDateTime(streamReader.ReadToEnd());
                streamReader.Close();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "自动更新失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dtUpdate;
        }
    }
}
