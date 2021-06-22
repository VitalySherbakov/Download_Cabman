using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Coding.Models;
using Control_Send.Models;
using Download_Cabman.Models;
using Download_Pack.Models;
using IgnoreFileGenerate.Models;

namespace Download_Cabman
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static FileFindReadWrite file = new FileFindReadWrite(Encoding.Default);
        private Option_Download SelectOperationLoad { get; set; }
        private History_Versions Select_History { get; set; }
        private Option_Update Select_Download_Json { get; set; }
        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            SelectOperationLoad = Program_Functions.LoadingOptions("Option_Download.json");
            this.Title = $"Download {SelectOperationLoad.Name}";
            Label_Process1.Content = "Процес: Пусто!";
            
        }

        
        private void GridLoad1_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadUpdateTest();
            //Thread myThread = new Thread(new ThreadStart(LoadOperationOption));
            //myThread.Start();
        }

        

        private void LoadOperationOption()
        {
            var txt = file.GetReadText(SelectOperationLoad.Server_Download);
            var json = Coding_Information.Decrypt(txt, "vitaly");
            
        }

        private void LoadTestingDownloadJson()
        {
            var json = file.GetReadText(SelectOperationLoad.Server_Download);
            if (Select_Download_Json!=null)
            {
                Select_Download_Json = JSON_Convert<Option_Update>.To_Object(json);
                Select_History = Program_Functions.HistoryConfiguration();
                var list_update = Select_History.HistoryVersions;
                var list_download = Select_Download_Json.Operation_Updates;
                switch (SelectOperationLoad.Status_Down)
                {
                    case Option_Status_Down.Version:
                        //Option_Update sel_obj =null;
                        //Если Есть обновления в истории
                        if (list_update.Length > 0)
                        {
                            //наличия обновления из загрузки
                            if (list_download.Length > 0)
                            {
                                var version_max_down = list_download.ToList().Max(x => x._Version);
                                var version_max_up = list_update.ToList().Max(x => x._Version);
                                if (version_max_up < version_max_down)
                                {
                                    //Поиск последней версии и загрузка
                                    var obj_down = list_download.FirstOrDefault(x => x._Version == version_max_down);

                                    //Download_Asynhron_WPF.Download()
                                }
                                else
                                {
                                    MessageBox.Show($"Обновлений Нету!", "INFO", MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Обновлений Нету!", "INFO", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            //наличия обновления из загрузки
                            if (list_download.Length > 0)
                            {

                            }
                            else
                            {
                                MessageBox.Show($"Обновлений Нету!", "INFO", MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                            }
                        }

                        break;
                    case Option_Status_Down.Consistently_Version:
                        break;
                    case Option_Status_Down.DataReal:
                        break;
                    case Option_Status_Down.Consistently_DataReal:
                        break;
                    default:
                        break;
                }
            }
        }

        
        private void Window1_ContentRendered_1(object sender, EventArgs e)
        {
            Label_Process1.Content = "Процес: Инициализация!";
            //Thread myThread = new Thread(new ThreadStart(LoadOperationOption));
            Thread myThread = new Thread(new ThreadStart(LoadTestingDownloadJson));
            myThread.Start();         
        }


        /// <summary>
        /// Тестовое Создание
        /// </summary>
        private void LoadUpdateTest()
        {
            string upd = "Update.json";
            DateTime DataUpdate=DateTime.Now;

            var files=new File_Download[]
            {
                new File_Download(new Uri("https://www.dropbox.com/s/eibg0g82afrfa96/Option_text.txt?dl=1"),"file.txt" ),
                new File_Download(new Uri("https://www.dropbox.com/s/eibg0g82afrfa96/Option_text.txt?dl=1"),"koop.txt" ),
                new File_Download(new Uri("https://www.dropbox.com/s/eibg0g82afrfa96/Option_text.txt?dl=1"),"doop.txt" ),
                new File_Download(new Uri("https://www.dropbox.com/s/eibg0g82afrfa96/Option_text.txt?dl=1"),"moop.txt" ),
                new File_Download(new Uri("https://www.dropbox.com/s/eibg0g82afrfa96/Option_text.txt?dl=1"),"zkop.txt" ),
            };

            var files_removes=new File_Delete[]
            {
                new File_Delete("file.txt"),
                new File_Delete("goom.txt"),
                new File_Delete("good\\file.txt"),
                new File_Delete("voop.txt"),
            }; 

            var obj1 = new Update_Download_Operation("обновление 4",2.4f,files,files_removes,DataUpdate);
            var obj2 = new Update_Download_Operation("обновление 3", 2.3f, files, files_removes, DataUpdate);
            var obj3 = new Update_Download_Operation("обновление 2", 2.2f, files, files_removes, DataUpdate);
            var obj4 = new Update_Download_Operation("обновление 1", 2.1f, files, files_removes, DataUpdate);

            Option_Update update = new Option_Update()
            {
                Operation_Updates = new Update_Download_Operation[] {obj1,obj2,obj3,obj4}
            };

            string json = JSON_Convert<Option_Update>.To_Json(update);
            file.WriteText(upd,json);
        }
    }
}
