using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Control_Send.Models;
using IgnoreFileGenerate.Models;
using Install_Update;

namespace Download_Cabman.Models
{
    public static class Program_Functions
    {
        private static FileFindReadWrite file = new FileFindReadWrite(Encoding.Default);

        public static Option_Install LoadingOptions(string option= "Option_Install.json")
        {
            Option_Install opt = new Option_Install();
            try
            {
                if (File.Exists(option))
                {
                    string json = file.GetReadText(option);
                    opt = JSON_Convert<Option_Install>.To_Object(json);
                }
                else
                {
                    opt = new Option_Install()
                    {
                       

                    };
                    string json = JSON_Convert<Option_Install>.To_Json(opt);
                    file.WriteText(option, json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Не вірний конфігуратор!");
                var error = $"ERROR: Не вірний конфігуратор: {ex.Message}";
                file.AddWrite("Error_Logs.txt", error);
                //MessageBox.Show($"ERROR: Не вірний конфігуратор!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return opt;
        }
    }
}
