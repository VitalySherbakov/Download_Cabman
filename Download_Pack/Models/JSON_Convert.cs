using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Control_Send.Models
{
    /// <summary>
    /// Конвентация JSON
    /// </summary>
    /// <typeparam name="T">Обьект</typeparam>
    public static class JSON_Convert<T>
    {
        /// <summary>
        /// Конвентировать Листок Обьектов в Список JSON Текстов
        /// </summary>
        /// <param name="list">Листок Обьектов</param>
        /// <returns>Список JSON Текстов</returns>
        public static string To_TextJsons(List<T> list)
        {
            string Textjson = string.Empty;
            try
            {
                Textjson = JsonConvert.SerializeObject(list);
            }
            catch
            {
                
            }
            return Textjson;
        }

        /// <summary>
        /// Конвентировать TEXT_JSONS_LIST в Листок Обьектов 
        /// </summary>
        /// <param name="json_list"></param>
        /// <returns></returns>
        public static List<T> To_ListObjects(string json_list)
        {
            List<T> list = new List<T>();
            try
            {
                list = JsonConvert.DeserializeObject<List<T>>(json_list);
            }
            catch 
            {
            }
            return list;
        }

        /// <summary>
        /// Конвентировать JSON в Обьект
        /// </summary>
        /// <param name="json">JSON</param>
        /// <returns>Возврат Обьект</returns>
        public static T To_Object(string json)
        {
            T obj = default(T);
            try
            {
                obj = JsonConvert.DeserializeObject<T>(json);
            }
            catch 
            {

            }
            return obj;
        }
        /// <summary>
        /// Конвентировать Обьект в JSON
        /// </summary>
        /// <param name="obj">Обьект</param>
        /// <returns>Возврат JSON</returns>
        public static string To_Json(T obj)
        {
            string json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(obj);
            }
            catch
            {
            }
            return json;
        }
    }
}
