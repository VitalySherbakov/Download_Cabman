using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Autorun_net;
using Download_Pack.Models;

namespace Update_Controls
{
    class Program
    {
        /*[DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;*/
        private static Process_start process = new Process_start();
        private static Timer_Sender_Server timerSender;
        static void Main(string[] args)
        {
            //timerSender = new Timer_Sender_Server(TimeSpan.FromSeconds(10), Test,true);
            //timerSender.Start();
            while (true)
            {
                
           
            Console.Write("Process Name: ");
            string processname = Console.ReadLine();
            var obj=process.Find_Program(processname.ToLower());
            Console.WriteLine($"Mem: {obj.MemorySize}");
            
            /*
            long size = 0;
            Console.Write("Size: ");
            size = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Size add: {size}");

            double kof = (double)obj.MemorySize / size;

            Console.WriteLine($"Kof: {kof}");*/
                
            Pause();
                Console.Clear();
            }
        }

        static void Test()
        {
            Console.WriteLine($"SENDER METHOD!");
        }

        /// <summary>
        /// Pauuse
        /// </summary>
        static void Pause()
        {
            Console.ReadKey(true);
        }
    }
}
