using System.Collections.Generic;

namespace AutoUpdate
{
    public class UpdateInfo
    {
        public UpdateInfo()
        {
            this.FileList = new List<FileItem>();
        }

        public string RootPath { get; set; }
        public string InfoPath { get; set; }

        public List<FileItem> FileList { get; set; }
    }

    public class FileItem
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string MD5 { get; set; }
    }
}
