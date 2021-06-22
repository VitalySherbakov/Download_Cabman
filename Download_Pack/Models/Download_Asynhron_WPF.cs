using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Download_Pack.Models
{
    /// <summary>
    /// Загрузка Файлов из Интернета
    /// </summary>
    public static class Download_Asynhron_WPF
    {
        public delegate void MethodStartProgress(DownloadProgressChangedEventArgs e);
        public delegate void MethodStartCompleted(AsyncCompletedEventArgs e);
        /// <summary>
        /// Передача Метода Конечного Сообщения
        /// </summary>
        public static MethodStartCompleted MethodCompleted { get; set; }
        /// <summary>
        /// Передача Метода Полосы
        /// </summary>
        public static MethodStartProgress MethodProgress { get; set; }

        /// <summary>
        /// Загрузка Файла в Потоке
        /// </summary>
        /// <param name="URL_Download">Ссылка Прямая</param>
        /// <param name="Path_Local_File">Локальное положение Файла</param>
        /// <returns>Возврат Результатов</returns>
        public static Result_Download DownloadThread(Uri URL_Download, string Path_Local_File)
        {
            Result_Download result = new Result_Download();
            WebClient webClient = new WebClient();
            try
            {
                Thread thread = new Thread(() =>
                {
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileAsync(URL_Download, Path_Local_File);
                });
                thread.Start();
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ERROR_DOWNLOAD = true;
                result.ERROR_DOWNLOAD_MSG = ex.Message;
                result.ERROR_DOWNLOAD_MSG_DETAL = ex.StackTrace;
            }

            if (SelectResult != null)
            {
                result = SelectResult;
            }
            return result;
        }

        /// <summary>
        /// Загрузка Файла
        /// </summary>
        /// <param name="URL_Download">Ссылка Прямая</param>
        /// <param name="Path_Local_File">Локальное положение Файла</param>
        /// <returns>Возврат Результатов</returns>
        public static Result_Download Download(Uri URL_Download, string Path_Local_File)
        {
            Result_Download result = new Result_Download();
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(URL_Download, Path_Local_File);
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ERROR_DOWNLOAD = true;
                result.ERROR_DOWNLOAD_MSG = ex.Message;
                result.ERROR_DOWNLOAD_MSG_DETAL = ex.StackTrace;
            }

            if (SelectResult != null)
            {
                result = SelectResult;
            }
            return result;
        }

        private static Result_Download SelectResult = null;
        /// <summary>
        /// Исполнение Методов Загрузки Полоса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Result_Download result = new Result_Download();
            try
            {
                MethodProgress(e);
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ERROR_DOWNLOAD = true;
                result.ERROR_DOWNLOAD_MSG = ex.Message;
                result.ERROR_DOWNLOAD_MSG_DETAL = ex.StackTrace;
            }

            SelectResult = result;
        }

        /// <summary>
        /// Исполнение Методов Загрузки Вывода Сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Result_Download result = new Result_Download();
            try
            {
                MethodCompleted(e);
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ERROR_DOWNLOAD = true;
                result.ERROR_DOWNLOAD_MSG = ex.Message;
                result.ERROR_DOWNLOAD_MSG_DETAL = ex.StackTrace;
            }
            SelectResult = result;
        }

        /// <summary>
        /// Изменение Кодировки Строки
        /// </summary>
        /// <param name="utfLine">Строка</param>
        /// <param name="old_codes">Старая Кодировка</param>
        /// <param name="new_codes">Новая Кодировка</param>
        /// <returns></returns>
        private static string CodingStr(string utfLine, Encoding old_codes, Encoding new_codes)
        {
            //Encoding utf = Encoding.UTF8;
            //Encoding win = Encoding.GetEncoding(1251);

            byte[] utfArr = old_codes.GetBytes(utfLine);
            byte[] winArr = Encoding.Convert(new_codes, old_codes, utfArr);

            string winLine = new_codes.GetString(winArr);
            return winLine;
        }

        /// <summary>
        /// Чтение Файла из Ссылки
        /// </summary>
        /// <param name="URL_File">Ссылка</param>
        /// <param name="CodingRead">Кодировка Четения</param>
        /// <returns></returns>
        public static Result_Download URL_File_Info_Get(Uri URL_File, Encoding CodingRead)
        {
            Result_Download result = new Result_Download();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL_File);
                //форма запроса
                request.Method = "GET";
                //отправка и получение ответа
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("Error Code: " + response.StatusCode.ToString());
                    }
                    //ответ
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream, CodingRead))
                            {
                                result.File_Size = response.ContentLength;
                                result.File_DateTime = Convert.ToDateTime(response.Headers.Get("Date"));
                                result.File_Read = reader.ReadToEnd();
                                result.File_Encoding = reader.CurrentEncoding;
                            }
                        }
                    }
                }
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ERROR_DOWNLOAD = true;
                result.ERROR_DOWNLOAD_MSG = ex.Message;
                result.ERROR_DOWNLOAD_MSG_DETAL = ex.StackTrace;
            }
            return result;
        }
    }

    /// <summary>
    /// Результаты Роботы
    /// </summary>
    public class Result_Download
    {
        /// <summary>
        /// Результат Виполнения
        /// </summary>
        public bool Result { get; set; } = false;
        /// <summary>
        /// Размер Файла
        /// </summary>
        public long File_Size { get; set; } = 0;
        /// <summary>
        /// Чтение Файла
        /// </summary>
        public string File_Read { get; set; } = string.Empty;
        /// <summary>
        /// Дата Создания файла
        /// </summary>
        public DateTime File_DateTime { get; set; }
        /// <summary>
        /// Кодировка Файла
        /// </summary>
        public Encoding File_Encoding { get; set; }
        /// <summary>
        /// Ошибка Результат
        /// </summary>
        public bool ERROR_DOWNLOAD { get; set; } = false;
        /// <summary>
        /// Ошибка Сообщение
        /// </summary>
        public string ERROR_DOWNLOAD_MSG { get; set; } = string.Empty;
        /// <summary>
        /// Детальная Ошибка
        /// </summary>
        public string ERROR_DOWNLOAD_MSG_DETAL { get; set; } = string.Empty;
    }
}
