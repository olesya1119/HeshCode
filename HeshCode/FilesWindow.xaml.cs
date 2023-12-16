using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HeshCode
{
    /// <summary>
    /// Логика взаимодействия для FilesWindow.xaml
    /// </summary>
    public partial class FilesWindow : Window
    {
        private List<string> access, files;
        private string login;

        public FilesWindow(string login)
        {
            this.login = login;

            InitializeComponent();
            File();
            CreateButtons();
        }

        private void File() {
            string textFromFile;

            using (FileStream fstream = new FileStream("../../files/accessMatrix.txt", FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fstream.Length];
                // считываем данные
                fstream.Read(buffer, 0, buffer.Length);
                // декодируем байты в строку
                textFromFile = Encoding.Default.GetString(buffer);    
            }
            string[] helperArray = textFromFile.Split('\n');
            files = helperArray[0].Split(' ').Cast<string>().ToList();
            files.RemoveAt(0);

            for (int i = 1; i < helperArray.Length; i++)
            {
                if (helperArray[i].Split(' ')[0] == login)
                {
                    access = helperArray[i].Split(' ').Cast<string>().ToList();
                    access.RemoveAt(0);
                    break;
                }

            }
        }



        private void CreateButtons()
        {
            for (int i = 0; i < files.Count; i++)
            {
                Button button = new Button();
                button.Content = files[i];
                button.Uid = i.ToString();
                button.Click += ButtonClick;
                stackPanel.Children.Add(button);          
            }
        }


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int i = System.Convert.ToInt32((sender as Button).Uid);
            if (access[i].Trim() == "r") {
                string path = @"" + System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\files\\" + files[i].Trim();
                System.Diagnostics.Process.Start(path);  
            }

            else
            {
                textBox.Text = "Вам запрещен доступ для этого файла!";
            }
        }


    }
}
