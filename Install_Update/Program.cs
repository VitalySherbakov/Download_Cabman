using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Autorun_net;
using Download_Cabman.Models;
using IgnoreFileGenerate.Models;

namespace Install_Update
{
    class Program
    {
        private static FileFindReadWrite file = new FileFindReadWrite(Encoding.Default);
        private static Process_start process=new Process_start();
        private const float Version = 1.00f;
        private static Option_Install SelectOption { get; set; }
        static void Main(string[] args)
        {
           
            string Dir_Update = "Down";
            ConsoleTitle($"Установка Оновлення ({Version.ToString("0.00")})");
            try
            {
                SelectOption = Program_Functions.LoadingOptions();
                Console.WriteLine("Конфигуратор Загружен...");
                ProgramsKills(SelectOption.ProcessKill);

                SelectOption.FolderDownload = SelectOption.FolderDownload.Replace(" ", "");
                SelectOption.FolderCopys = SelectOption.FolderCopys.Replace(" ", "");
                DirectoryInfo dir_add, dir_up;

                //FolderDownload Источник
                if (SelectOption.FolderDownload!=string.Empty)
                {
                    dir_up = new DirectoryInfo(SelectOption.FolderDownload);
                }
                else
                {
                    dir_up = null;
                    Console.WriteLine("ERROR: НЕТУ ИСТОЧНИКА - ДИРЕКТОРИИ ИЗЬЯТИЯ!");
                }

                //FolderCopys куда копировать
                if (SelectOption.FolderCopys == string.Empty)
                {
                    dir_add = new DirectoryInfo(Environment.CurrentDirectory);
                }
                else
                {
                    dir_add = new DirectoryInfo(SelectOption.FolderCopys);
                }
                

                if (dir_up!=null && dir_up.Exists)
                {
                    //Удаление Файлов

                    //Копирование Файлов
                    string[] files = Directory.GetFiles(dir_up.FullName, "*.*",SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var path_old = file;
                        var path_new = file.Replace(dir_up.FullName, dir_add.FullName);
                        CreateFolder(path_new);
                        Copy(path_old,path_new);                        
                    }
                    //Чистка Папки Загрузки
                    ClearFolder(dir_up.FullName);
                    //Запуск програм и Виход из Тикущей
                    StartProgramsShow(SelectOption.StartPrograms);
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine($"ERROR: Ошибка Оновлення!");
                var error = $"ERROR: Ошибка Оновлення: {ex.Message}";
                file.AddWrite("Error_Logs.txt", error);
            }
            Pause();
        }

        static void SetDeletionFiles(string[] DeleteFiles)
        {
            foreach (var filekill in DeleteFiles)
            {
                try
                {
                    File.Delete(Path.GetFullPath(filekill));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR Удаления {filekill}");
                    var error = $"ERROR: ERROR Удаления {filekill}: {ex.Message}";
                    file.AddWrite("Error_Remove_Logs.txt", error);
                }
            }
        }

        /// <summary>
        /// Запуск Программ
        /// </summary>
        /// <param name="startprograms">Список Программ</param>
        static void StartProgramsShow(string[] startprograms)
        {

                foreach (var prost in startprograms)
                {
                    try
                    {
                       process.Process_Start(prost, ProcessWindowStyle.Normal);
                       Console.WriteLine($"Запущена Программа: {prost}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR Запуска {prost}");
                       var error = $"ERROR: ERROR Запуска {prost}: {ex.Message}";
                       file.AddWrite("Error_Logs.txt", error);
                    }
                
                }
                Console.WriteLine("Выход из Программы!");
                Environment.Exit(0);
        }

        /// <summary>
        /// Остановка Программ
        /// </summary>
        /// <param name="processkills">Список Процесов</param>
        static void ProgramsKills(string[] processkills)
        {
                foreach (var pro in processkills)
                {
                    try
                    {
                       var processfile = process.Find_Program(pro);
                       var flag = process.Kill_Program(processfile);
                       if (flag)
                       {
                          Console.WriteLine($"Остановка Программы: {pro}");
                       }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR Остановка Программы: {pro}");
                        var error = $"ERROR: Остановка Программы: {ex.Message}";
                        file.AddWrite("Error_Logs.txt", error);
                    }
               
                }               
        }

        /// <summary>
        /// Копирование Файла и Очистка
        /// </summary>
        /// <param name="path_old">Старое Место</param>
        /// <param name="path_new">Нове Место</param>
        static void Copy(string path_old, string path_new)
        {
            try
            {
                string pa_old = Path.GetFullPath(path_old);
                string pa_new = Path.GetFullPath(path_new);
                File.Copy(pa_old, pa_new, true);
                Console.WriteLine($"Copy: {pa_old} -> {pa_new}");
            }
            catch (Exception ex)
            {
                var error = $"ERROR: Copy {path_old} -> {path_new}";
                Console.WriteLine(error);
                file.AddWrite("Error_Copy_Logs.txt", error);
            }
            
        }

        /// <summary>
        /// Чистка Папки
        /// </summary>
        /// <param name="path_folder"></param>
        static void ClearFolder(string path_folder)
        {
            try
            {
                Directory.Delete(path_folder,true);
                Directory.CreateDirectory(path_folder);
                Console.WriteLine("Установка Завершена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {path_folder} CLEAR FOLDER!");
                var error = $"ERROR: ERROR: {path_folder} CLEAR FOLDER: {ex.Message}";
                file.AddWrite("Error_Logs.txt", error);
            }
        }

        /// <summary>
        /// Создание Каталога
        /// </summary>
        /// <param name="path_dir"></param>
        static void CreateFolder(string path_dir)
        {
            try
            {
                string dir = Path.GetDirectoryName(path_dir);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                var error = $"ERROR: Create {path_dir}";
                Console.WriteLine(error);
                file.AddWrite("Error_Folder_Logs.txt", error);
            }
        }

        /// <summary>
        /// Пауза
        /// </summary>
        static void Pause()
        {
            Console.ReadKey(true);
        }

        /// <summary>
        /// Титулка
        /// </summary>
        /// <param name="Name">Имя</param>
        static void ConsoleTitle(string Name)
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = Name;
        }
    }
}
