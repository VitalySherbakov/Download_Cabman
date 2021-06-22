using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Install_Update
{
    public class Option_Install
    {
        public string[] ProcessKill { get; set; } = new string[]{ "Update_Sw_Controls", "Download_Cabman" };
        public string FolderDownload { get; set; } = "Down";
        public string FolderCopys { get; set; } = string.Empty;
        public string[] StartPrograms { get; set; } = new string[] { "Update_Sw_Controls.exe" };
    }
}
