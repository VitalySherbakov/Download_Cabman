using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Download_Cabman.Models
{
    /// <summary>
    /// Настройки Программи
    /// </summary>
    public class Option_Download
    {
        /// <summary>
        /// Дополняющее Имя
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Файл Чтения и Загрузки
        /// </summary>
        public string Server_Download { get; set; } = "loading.txt";
        /// <summary>
        /// Статус Тригер Указатель
        /// </summary>
        public Option_Status_Down Status_Down { get; set; } = Option_Status_Down.Version;
    }

    /// <summary>
    /// Тригери Указатели
    /// </summary>
    public enum Option_Status_Down
    {
        /// <summary>
        /// За Версией Старше
        /// </summary>
        Version,
        /// <summary>
        /// Последовательно по Версии
        /// </summary>
        Consistently_Version,
        /// <summary>
        /// Последняя Версия по Дате
        /// </summary>
        DataReal,
        /// <summary>
        /// Последовательно по Датам
        /// </summary>
        Consistently_DataReal
    }

    /// <summary>
    /// Обновление Настройки
    /// </summary>
    public class Option_Update
    {
        public Update_Download_Operation[] Operation_Updates { get; set; } = new Update_Download_Operation[]{ };
    }

    /// <summary>
    /// Данние о Загружаймом Файле
    /// </summary>
    public class File_Download
    {
        /// <summary>
        /// Ссилка Файла
        /// </summary>
        public Uri _URL_File;
        /// <summary>
        /// Файл
        /// </summary>
        public string _Path_Local;
        /// <summary>
        /// Данние о Файле
        /// </summary>
        /// <param name="URL_File">Ссилка Файла</param>
        /// <param name="Path_Local">Файл</param>
        public File_Download(Uri URL_File, string Path_Local)
        {
            this._URL_File = URL_File;
            this._Path_Local = Path_Local;
        }
    }

    /// <summary>
    /// Данние Удаления Файла Не нужного
    /// </summary>
    public class File_Delete
    {
        /// <summary>
        /// Файл Удаления
        /// </summary>
        public string _Path_Local;
        /// <summary>
        /// Данние о Файле
        /// </summary>
        /// <param name="Path_Local">Файл Удаления</param>
        public File_Delete(string Path_Local)
        {
            this._Path_Local = Path_Local;
        }
    }

    /// <summary>
    /// Обновлениие с коментарием
    /// </summary>
    public class Update_Download_Operation
    {
        /// <summary>
        /// Коментарий
        /// </summary>
        public string _Comentar;
        /// <summary>
        /// Версия
        /// </summary>
        public string _Version;
        /// <summary>
        /// Дата Обновления
        /// </summary>
        public DateTime _DataUpdate;
        /// <summary>
        /// Файли Обновления
        /// </summary>
        public File_Download[] _Files;
        /// <summary>
        /// Файли Удаления Мусора
        /// </summary>
        public File_Delete[] _Deletes;
        /// <summary>
        /// Данние Загрузки Обновления
        /// </summary>
        /// <param name="Comentar">Коментарий</param>
        /// <param name="Version">Версия</param>
        /// <param name="Files">Файли Обновления</param>
        /// <param name="File_Deletes">Файли Удаления</param>
        public Update_Download_Operation(string Comentar,string Version,File_Download[] Files,File_Delete[] File_Deletes,DateTime DataUpdate)
        {
            this._Comentar = Comentar;
            this._Version = Version;
            this._Files = Files;
            this._Deletes = File_Deletes;
            this._DataUpdate = DataUpdate;
        }
    }
}
