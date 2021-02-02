using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Bot;

namespace WpfAppBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TelegramUser> Users;
        TelegramBotClient bot;

        public MainWindow()
        {
            InitializeComponent();

            Users = new ObservableCollection<TelegramUser>();

            usersList.ItemsSource = Users; //установка источника данных

            string token = File.ReadAllText("token");

            bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10) };

            //обработка входящих сообщений
            bot.OnMessage += delegate (object sender, Telegram.Bot.Args.MessageEventArgs e)
            {
                string msg = $"{DateTime.Now}: {e.Message.Chat.FirstName} {e.Message.Chat.Id} {e.Message.Text}";

                File.AppendAllText("data.log", $"{msg}\n"); //логирование запросов

                Debug.WriteLine(msg);

                //обработка добавления данных в UI
                this.Dispatcher.Invoke(() =>
                {
                    var person = new TelegramUser(e.Message.Chat.FirstName, e.Message.Chat.Id);
                    if (!Users.Contains(person)) Users.Add(person);
                    Users[Users.IndexOf(person)].AddMessage($"{person.Nick}: {e.Message.Text}");
                });
            };

            bot.StartReceiving(); //запуск сервиса обработки входящих запросов

            btnSendMsg.Click += delegate { SendMsg(); };
            txtBxSendMsg.KeyDown += (s, e) => { if (e.Key == Key.Return) { SendMsg(); } };


        }

        public void SendMsg()
        {
            var conreteUser = Users[Users.IndexOf(usersList.SelectedItem as TelegramUser)];
            string responseMsg = $"Support: {txtBxSendMsg.Text}";
            conreteUser.Messages.Add(responseMsg);

            bot.SendTextMessageAsync(conreteUser.Id, txtBxSendMsg.Text);
            string logText = $"{DateTime.Now}: >> {conreteUser.Id} {conreteUser.Nick} {responseMsg}\n";
            File.AppendAllText("data.log", logText);

            txtBxSendMsg.Text = String.Empty;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
