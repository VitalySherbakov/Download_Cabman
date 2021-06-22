using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading;
//using IWshRuntimeLibrary; //Добавить COM = Winows Script Host Object Model
using Microsoft.Win32;

namespace Autorun_net
{
    /// <summary>
    /// Запуск Программ
    /// </summary>
    public class Process_start
    {
        /// <summary>
        /// Конвентирование Исполняющего в имя процеса
        /// </summary>
        /// <param name="Path_Exe">Путь или Исполняющий Файл</param>
        /// <returns>Возврат Имени Процеса</returns>
        public string ConvertExeToProcess(string Path_Exe)
        {
            string processName = string.Empty;
            var str = Path.GetFileName(Path_Exe);
            string exe = ".exe";
            if (Path.GetExtension(str).ToLower()==exe.ToLower())
            {
                processName = str.Replace(exe.ToLower(), "");
            }
            return processName;
        }

        /// <summary>
        /// Загрузка Только Одного Процеса Программы или Окна
        /// </summary>
        /// <param name="name_file">Имя Программы</param>
        public void Diagnostik_One_Program(string name_file)
        {
            Process currentPr = Process.GetCurrentProcess();
            foreach (Process pr in Process.GetProcesses())
            {
                if ((pr.ProcessName == currentPr.ProcessName) && (pr.Id != currentPr.Id))
                {

                    //MessageBox.Show("Приложение уже запущено!!!", "Внимание",
                    //MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return;

                    string name = name_file;//"SWICH_MOD_2DLC";//процесс, который нужно убить
                    System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получим процессы
                    foreach (System.Diagnostics.Process anti in etc)//обойдем каждый процесс
                        if (anti.ProcessName.ToLower().Contains(name.ToLower())) anti.Kill();//найдем нужный и убьем
                    //ToLower() - метод для переведения всех букв в нижний регистр, или как то так
                    return;

                }
            }
        }

        protected Process[] process;
        /// <summary>
        /// Поиск Программы в Процесах
        /// </summary>
        /// <param name="name_file">Имя Программы</param>
        /// <returns>Возврат Process_File есть такая прога</returns>
        public Process_File Find_Program(string name_file)
        {
            Process_File Process_find = new Process_File();
            try
            {
                Process proc = Process.GetProcessesByName(name_file)[0];
                Process_find.ProcessName = proc.ProcessName;
                Process_find.ID_Process = proc.Id;
                Process_find.ProcessThread = proc.Threads;
                Process_find.ProcessModules = proc.Modules;
                Process_find.MemorySize = proc.PrivateMemorySize64;
                Process_find.MemorySizeOnlineBytes = ProcessPC_Memory(proc.ProcessName).MemorySizeOnlineBytes;
                Process_find.MemorySizeOnlineKBytes = ProcessPC_Memory(proc.ProcessName).MemorySizeOnlineKBytes;
                Process_find.MemorySizeOnlineMBytes = ProcessPC_Memory(proc.ProcessName).MemorySizeOnlineMBytes;
                Process_find.FlagExist = true;
            }
            catch
            {
            }
            return Process_find;
        }

        /// <summary>
        /// Получение Памяти Процеса
        /// </summary>
        /// <param name="name_file">Название Процеса</param>
        /// <returns>Возврат Памяти</returns>
        private Process_File ProcessPC_Memory(string name_file)
        {
            Process_File procNew=new Process_File();
            try
            {
                PerformanceCounter PC = new PerformanceCounter();
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = name_file;

                procNew.MemorySizeOnlineBytes = PC.NextValue();
                procNew.MemorySizeOnlineKBytes = (PC.NextValue() / 1024);
                procNew.MemorySizeOnlineMBytes = (PC.NextValue() / (1024 * 1024));
            }
            catch
            {
               
            }
            return procNew;
        }

        /// <summary>
        /// Уничтожить Процесс
        /// </summary>
        /// <param name="processFile"></param>
        /// <returns></returns>
        public bool Kill_Program(Process_File processFile)
        {
            bool Flagkill = false;
            try
            {
                Process proc = Process.GetProcessById(processFile.ID_Process);
                proc.Kill();
            }
            catch 
            {
               
            }
            return Flagkill;
        }

        /// <summary>
        /// Ограничение Запуска Окон Программы
        /// </summary>
        /// <param name="name_file">Имя Программы</param>
        /// <param name="CountStart">Количество Окон Программы</param>
        public void Limitation_Start(string name_file,int CountStart)
        {
            process = Process.GetProcessesByName(name_file);

            if (process.Length == 0)
            {
                Console.WriteLine("None");
            }
            if (process.Length == 1)
            {
                Console.WriteLine("One");
            }
            if (process.Length > 1)
            {
                if (process.Length > CountStart)
                {
                    for (int i = 0; i < (process.Length - CountStart); i++)
                    {
                        process[i].Kill();
                    }
                    Console.WriteLine("Big");
                }

            }

            
        }

        /// <summary>
        /// Автозагрузка Программы
        /// </summary>
        /// <param name="name">Имя Программы</param>
        /// <param name="autorun">Автозагрузка Вкл/Выкл</param>
        /// <returns></returns>
        public bool SetAutorunValue(string name,bool autorun)
        {
            string ExePath = $"{Environment.CurrentDirectory}\\{name}.exe";//Application.ProductName;//System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue(name, ExePath);
                else
                    reg.DeleteValue(name);

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Процесс Запуска Программы
        /// </summary>
        /// <param name="Path_Exe">Exe Программы</param>
        /// <param name="Path_Dir">Папка где находиться Exe Программы</param>
        /// <param name="Argument">Аргумент Запуска</param>
        /// <param name="Creat_Now_Win">Форма Запуска</param>
        /// <param name="Style">Стиль Окна Скрытый Не скрытый</param>
        public void Process_Start(string Path_Exe,ProcessWindowStyle Style, string Path_Dir="", string Argument="", bool Creat_Now_Win=false)
        {
            Process file=new Process();
            file.StartInfo.CreateNoWindow = Creat_Now_Win;
            file.StartInfo.FileName = Path_Exe;
            if (Path_Dir != string.Empty)
            {
                file.StartInfo.WorkingDirectory = Path_Dir;
            }
            if (Argument != string.Empty)
            {
                file.StartInfo.Arguments = Argument;
            }
            file.StartInfo.WindowStyle=Style;
            file.Start();
            file.Close();
            file.Dispose();
        }

       /* public void shortcut_file_creat(string path_name_lnk,string path_icon,string Description_lnk,string button,string path_dir,string path_exe)
        {
          WshShell shell = new WshShell();
 
          //путь к ярлыку name.lnk
          string shortcutPath = path_name_lnk;
 
           //создаем объект ярлыка
           IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
 
           //задаем свойства для ярлыка
           //описание ярлыка в всплывающей подсказке
           shortcut.Description = Description_lnk;
           //горячая клавиша "Ctrl+Shift+N"
            shortcut.Hotkey = button;
            //путь к самой программе
            shortcut.TargetPath = path_exe;
            //иконка
            shortcut.IconLocation = path_icon;
            // папка с прогой exe
            shortcut.WorkingDirectory = path_dir; //Рабочий каталог

            //Для свойства WindowStyle допустимы следующие значения:
            //1 - размер по умолчанию (default).
            //3 - максимизированное окно
            //7 - минимизированное окно
            shortcut.WindowStyle = 1; //Поведение окна при открытии


            //Создаем ярлык
            shortcut.Save();
        }

        /*
         //Сначала подключаем ссылку на библиотеку "Windows Script Host Object Model"
//(ищем её в MS VS 2010 на вкладке "COM" диалогового окна "Добавить ссылку")...
using System;
using System.IO;
//Затем добавляем "алиас" для нужного нам пространства имён:
using IWshRuntimeLibrary;
 
namespace Test {
 class Program {
  static void Main(string[] args) {
   //Каталог, в котором хотим создать ярлык
   string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
 
   IWshShell wsh = new WshShellClass();
 
   //Расширение ".lnk" указывать обязательно!
   //Если указанный lnk-файл уже существует, то объект link получит все его свойства и их
   //можно читать/редактировать.
   //Если указанный lnk-файл не существует, то он будет создан после сохранения.
   IWshShortcut link = (IWshShortcut) wsh.CreateShortcut(Path.Combine(path, "Мой ярлык.lnk"));
 
   //Настраиваем свойства ярлыка...
 
   //Для свойства WindowStyle допустимы следующие значения:
   //1 - размер по умолчанию (default).
   //3 - максимизированное окно
   //7 - минимизированное окно
   link.WindowStyle = 1;//Поведение окна при открытии
   link.TargetPath = @"%AppData%"; //полный путь к объекту (файлу или каталогу) на который должен
   //указывать ярлык
 
   link.Hotkey = "CTRL+SHIFT+N";//"Горячие" клавиши
   link.Description = "Текстовое описание моего ярлыка";//Описание
   link.WorkingDirectory = path;//Рабочий каталог
   link.IconLocation = @"%SystemRoot%\system32\SHELL32.dll, 15"; //любая иконка.
 
   //Не забываем сохранить выполненные нами изменения!
   link.Save();
  }
 }
}
         */
    }

    /// <summary>
    /// Данние Процесса
    /// </summary>
    public class Process_File
    {
        /// <summary>
        /// Имя Процеса
        /// </summary>
        public string ProcessName { get; set; } = string.Empty;
        /// <summary>
        /// Память Процеса
        /// </summary>
        public long MemorySize { get; set; } = 0;
        /// <summary>
        /// Пямять Процеса Тикущая Байти
        /// </summary>
        public float MemorySizeOnlineBytes { get; set; } = 0;
        /// <summary>
        /// Пямять Процеса Тикущая КБайти
        /// </summary>
        public float MemorySizeOnlineKBytes { get; set; } = 0;
        /// <summary>
        /// Пямять Процеса Тикущая МБайти
        /// </summary>
        public float MemorySizeOnlineMBytes { get; set; } = 0;
        /// <summary>
        /// Индитификатор Процеса
        /// </summary>
        public int ID_Process { get; set; } = 0;
        /// <summary>
        /// Результат Наличия
        /// </summary>
        public bool FlagExist { get; set; } = false;
        /// <summary>
        /// Потоки Процеса
        /// </summary>
        public ProcessThreadCollection ProcessThread { get; set; }
        /// <summary>
        /// Модули Процеса
        /// </summary>
        public ProcessModuleCollection ProcessModules { get; set; }
    }
}
