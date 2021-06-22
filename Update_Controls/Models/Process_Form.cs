using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Update_Controls.Models
{
    public static class Process_Form
    {
        public static void FormLoad(string Path_Local_Exe)
        {
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            p.StartInfo.FileName = Path_Local_Exe;
            p.Start();
            p.WaitForExit();
        }
    }
}
