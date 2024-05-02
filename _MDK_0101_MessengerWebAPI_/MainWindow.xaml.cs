using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Servicebastard;



namespace _MDK_0101_MessengerWebAPI_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        static UInt64 ID = 0;
        const string nameWindow = "WebMessager";
        string logFilePath = @"../Logs/Logs.txt";
        static private Service.Service Service =
            new Service.Service("localhost", 5250);
        public MainWindow()
        {
            InitializeComponent();
            this.Title = nameWindow;
            userVerification();
            if(!Directory.Exists(logFilePath)) logFilePath = createLogFile();
            Closing += MainWindow_Closing;

        }

        public void userVerification()
        {
            if (File.Exists(logFilePath) == true)
            {
                use_Button.IsEnabled = false;
                string name = null;
                string searchString = "User:";

                try
                {
                    foreach (string line in File.ReadLines(logFilePath))
                    {
                        if (line.Contains(searchString))
                        {
                            int userIndex = line.IndexOf(searchString);

                            if (userIndex != -1)
                            {
                                string userName = line.Substring(userIndex + searchString.Length).Trim();
                                name = userName;
                                break;
                            }
                        }
                    }
                    this.Title = nameWindow + " - Пользователь - " + name;
                    inputUserName.Text = name;
                    inputUserName.IsEnabled = false;
                    rename_Button.IsEnabled = true;
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                rename_Button.IsEnabled = false;
                return;
            }

        }
        public static string createLogFile()
        {
            string directoryPath = @"..\Logs";
            Directory.CreateDirectory(directoryPath);
            string fileName = $"Logs.txt";
            string filePath = Path.Combine(directoryPath, fileName);
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"Start of work: {DateTime.Now.ToString("HH:mm:ss")} ");
            }
            return filePath;
        }

        private void use_Button_Click(object sender, RoutedEventArgs e)
        {

            string userName = Convert.ToString(inputUserName.Text);
            this.Title += " - Пользователь - " + userName;
            use_Button.IsEnabled = false;
            inputUserName.IsEnabled = false;
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"ID: {userName}");
                writer.WriteLine($"User: {userName} ");
            }
        }

        private void send_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Service.SetNewAddress(this.IPBox.Text, this.PortBox.Text);
            }
            catch
            {
                MessageBox.Show("Неверный адрес сервера", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"End of work: {DateTime.Now.ToString("HH:mm:ss")} ");
            }
        }

        private void rename_Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Вы точно хотите изменить меня?\nЕсли да, то ваши данные будут удалены", "Предупреждение!", MessageBoxButton.YesNo,MessageBoxImage.Warning);
            //MessageBoxResult boxResultYes = MessageBoxResult.Yes;

            //if (boxResultYes == MessageBoxResult.Yes)
            //{
               
                
                    Directory.Delete(logFilePath);
                
               
            //}
            ////else return;

        }
        
        }

    }


