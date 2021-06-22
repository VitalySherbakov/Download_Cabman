using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IgnoreFileGenerate.Models
{
    public class FileFindReadWrite
    {
        /// <summary>
        /// Кодировка Общая
        /// </summary>
        private Encoding _Code { get; set; } 

        /// <summary>
        /// Ошибки Чтения и Записи
        /// </summary>
        public string MsgError { get; set; }

        /// <summary>
        /// Ошибки Чтения и Записи
        /// </summary>
        public string MsgDetalError { get; set; }

        public FileFindReadWrite(Encoding Code)
        {
            _Code = Code;
        }

        /// <summary>
        /// Записываем Построчно Массивом
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="text">Массив Строк</param>
        public void Write(string Pathfile, string[] text)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(Pathfile, false, _Code))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        write.WriteLine(text[i]);
                    }
                    write.Close();
                    write.Dispose();
                }
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Дописываем Строку в Конец
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="str">Строка</param>
        public void AddWrite(string Pathfile, string str)
        { 
            try
            {
                using (StreamWriter write = new StreamWriter(Pathfile, true, _Code))
                {
                    write.WriteLine(str);
                    write.Close();
                    write.Dispose();
                }
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Читаем все Строки
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <returns>Возвращает Массив Строк с Файла</returns>
        public string[] GetRead(string Pathfile)
        {
            string[] mass_str = null;
            try
            {
                mass_str = File.ReadAllLines(Pathfile, _Code);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
            return mass_str;
        }

        /// <summary>
        /// Получить Весь Текст
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <returns>Возвращает Текст с Файла</returns>
        public string GetReadText(string Pathfile)
        {
            string strText = string.Empty;
            try
            {
                strText = File.ReadAllText(Pathfile, _Code);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
            return strText;
        }

        /// <summary>
        /// Получить Байты с Файла
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <returns>Возвращает Байты с Файла</returns>
        public byte[] GetByte(string Pathfile)
        {
            byte[] bytes=null;
            try
            {
                bytes = File.ReadAllBytes(Pathfile);
            }
            catch(Exception ex)
            {
                MsgError += ex.Message+"\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
            return bytes;
        }

        /// <summary>
        /// Добавить Текст
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="Text">Текст</param>
        public void WriteText(string Pathfile,string Text)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(Pathfile, false, _Code))
                {
                    write.Write(Text);
                    write.Close();
                    write.Dispose();
                }
                //File.AppendAllText(Pathfile, Text, _Code);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Добавить Строку
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="Str">Строка</param>
        public void AddWriteStr(string Pathfile, string Str)
        {
            try
            {
                File.AppendText(Pathfile).WriteLine(Str);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Шифрование Файла
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        public void EncryptFile(string Pathfile)
        {
            try
            {
                File.Encrypt(Pathfile);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Дешифровка Файла
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        public void DecryptFile(string Pathfile)
        {
            try
            {
                File.Decrypt(Pathfile);
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Изменить Строку в Файле
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="Number">Номер Строки от 0</param>
        /// <param name="str">Строка Новая</param>
        public void EditOneWrite(string Pathfile,int Number,string str)
        {
            string[] mass = GetRead(Pathfile);

            for (int i=0; i<mass.Length; i++)
            {
                if(i==Number)
                {
                    mass[i] = str;
                }
            }

            Write(Pathfile, mass);
        }

        /// <summary>
        /// Удалить Строку в Файле
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="str">Строка Удаление</param>
        public void DeleteOneWrite(string Pathfile, string str)
        {
            if (str!=string.Empty)
            {
                string[] mass = GetRead(Pathfile);

                List<string> Listmass = mass.ToList();

                for (int i = 0; i < Listmass.Count; i++)
                {
                    Listmass.Remove(str);
                }

                Write(Pathfile, Listmass.ToArray());
            }
        }

        /// <summary>
        /// Изменить Строки в Файле
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        /// <param name="EditMass">Массив Обьектов Изменения Строк</param>
        public void EditWrite(string Pathfile, EditFileStr[] EditMass)
        {
            string[] mass = GetRead(Pathfile);

            for (int j = 0; j < EditMass.Length; j++)
            {
                for (int i = 0; i < mass.Length; i++)
                {
                    if (i == EditMass[j].Number)
                    {
                        mass[i] = EditMass[j].Str;
                    }
                }
            }

            Write(Pathfile, mass);
        }

        /// <summary>
        /// Очистка Файла
        /// </summary>
        /// <param name="Pathfile">Путь к Файлу</param>
        public void ClearFile(string Pathfile)
        {
            try
            {
                using (StreamWriter write = new StreamWriter(Pathfile, false, _Code))
                {
                    write.Dispose();
                    write.Close();
                }
            }
            catch (Exception ex)
            {
                MsgError += ex.Message + "\n";
                MsgDetalError += ex.Message + "\n" + ex.StackTrace + "\n";
            }
        }

        /// <summary>
        /// Поиск с Старата, Полного Пути, Часть Пути
        /// </summary>
        /// <param name="Path">Полный Путь</param>
        /// <param name="Find">Часть Пути</param>
        /// <returns>Возвращает Полный Путь, если Часть Пути найдена, или Пустоту</returns>
        public string FindPathStart(string Path,string Find)
        {
            string _Path = string.Empty;
            string[] mass = Path.Split('\\');
            for(int i=0;i<mass.Length;i++)
            {
                if(mass[i]==Find)
                {
                    _Path = Path;
                    break;
                }
            }
            return _Path;
        }

        /// <summary>
        /// Получить Директории Старта Путей
        /// </summary>
        /// <param name="SelectDir">Общая Директория</param>
        /// <param name="FindDir">Поиск Директории</param>
        /// <returns>Директории Старта Путей</returns>
        public List<string> FindFolder(string SelectDir, string FindDir)
        {
            List<string> ListDirs = new List<string>();

            DirectoryInfo dirfind = new DirectoryInfo(FindDir);

            DirectoryInfo dir = new DirectoryInfo(SelectDir);
            DirectoryInfo[] dirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo dir_result in dirs)
            {
                string find = FindPathStart(dir_result.FullName, dirfind.Name);
                if (find != string.Empty)
                {
                    ListDirs.Add(find);
                }
            }

            //Убирает Дубликаты
            ListDirs = ListDirs.Distinct().ToList();

            return ListDirs;
        }

        /// <summary>
        /// Получить Директории Старта Путей Краткую
        /// </summary>
        /// <param name="SelectDir">Общая Директория</param>
        /// <param name="FindDir">Поиск Директории</param>
        /// <returns>Директории Старта Путей</returns>
        public List<string> FindFolderMin(string SelectDir, string FindDir)
        {
            List<string> ListDirs = new List<string>();

            DirectoryInfo dirfind = new DirectoryInfo(FindDir);

            DirectoryInfo dir = new DirectoryInfo(SelectDir);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo dir_result in dirs)
            {
                string find = FindPathStart(dir_result.FullName, dirfind.Name);
                if (find != string.Empty)
                {
                    ListDirs.Add(find);
                }
            }

            //Убирает Дубликаты
            ListDirs = ListDirs.Distinct().ToList();

            return ListDirs;
        }

        /// <summary>
        /// Получить Файлы Старта Путей
        /// </summary>
        /// <param name="SelectDir">Общая Директория</param>
        /// <param name="_FindFile">Поиск Файла</param>
        /// <returns>Файлы Старта Путей</returns>
        public List<string> FindFile(string SelectDir, string _FindFile)
        {
            FileInfo filefind = new FileInfo(_FindFile);

            List<string> ListFiles = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(SelectDir);
            FileInfo[] files = dir.GetFiles("*.*",SearchOption.AllDirectories);
            for(int i=0;i<files.Length;i++)
            {
                string find = FindPathStart(files[i].FullName, filefind.Name);
                if (find != string.Empty)
                {
                    ListFiles.Add(find);
                }
            }

            //Убирает Дубликаты
            ListFiles = ListFiles.Distinct().ToList();
            return ListFiles;
        }

    }

    /// <summary>
    /// Изменить Строку
    /// </summary>
    public class EditFileStr
    {
        /// <summary>
        /// Новая Строка
        /// </summary>
        public string Str { get; set; }
        /// <summary>
        /// Номер Строки Изменения
        /// </summary>
        public int Number { get; set; }
    }


}
