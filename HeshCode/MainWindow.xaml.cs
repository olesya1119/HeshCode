using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeshCode
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] profiles;
            using (FileStream fstream = new FileStream("../../files/loginDetails.txt", FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fstream.Length];
                // считываем данные
                fstream.Read(buffer, 0, buffer.Length);
                // декодируем байты в строку
                string textFromFile = Encoding.Default.GetString(buffer);
                profiles = textFromFile.Split(new char[] {' ', '\n'});         
            }
            for (int i = 0; i < profiles.Length; i+=2)
            {
                if (profiles[i] == textbox_login.Text && profiles[i+1] == CreateSHA512(passwordbox_password.Password))
                {
                    new FilesWindow(profiles[i]).Show();
                    Close();
                }
            }

            error_message.Text = "Пароль или логин введины неверно. Попробуйте заново!";
            
        }



        public static string CreateSHA512(string source)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                return hash;
            }
        }
    }
}
