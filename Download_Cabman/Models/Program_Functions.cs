using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Coding.Models;
using Control_Send.Models;
using IgnoreFileGenerate.Models;

namespace Download_Cabman.Models
{
    public static class Program_Functions
    {
        private static FileFindReadWrite file = new FileFindReadWrite(Encoding.Default);

        public static Option_Download LoadingOptions(string option= "Option_Download.json")
        {
            Option_Download opt = new Option_Download();
            try
            {
                if (File.Exists(option))
                {
                    string json = file.GetReadText(option);
                    opt = JSON_Convert<Option_Download>.To_Object(json);
                }
                else
                {
                    opt = new Option_Download();
                    string json = JSON_Convert<Option_Download>.To_Json(opt);
                    file.WriteText(option, json);
                }
            }
            catch
            {
                MessageBox.Show($"ERROR: Не вірний конфігуратор!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return opt;
        }

        /// <summary>
        /// История Версий
        /// </summary>
        /// <param name="option">Файл Историй</param>
        /// <returns></returns>
        public static History_Versions HistoryConfiguration(string option = "History_Versions.bin")
        {
            History_Versions history=new History_Versions();
            try
            {
                if (File.Exists(option))
                {
                    string json = file.GetReadText(option);
                    json = Coding_Information.Decrypt(json, "vitaly");
                    history = JSON_Convert<History_Versions>.To_Object(json);
                }
                else
                {
                    history = new History_Versions();
                    string json = JSON_Convert<History_Versions>.To_Json(history);
                    json = Coding_Information.Encrypt(json, "vitaly");
                    file.WriteText(option, json);
                }
            }
            catch
            {
                MessageBox.Show($"ERROR: Не вірний конфігуратор Історії!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return history;
        }
    }
}
