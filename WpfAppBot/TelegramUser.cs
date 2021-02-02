using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppBot
{
    class TelegramUser: INotifyPropertyChanged, IEquatable<TelegramUser>
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="Nickname"></param>
        /// <param name="ChatId"></param>
        /// 
        public TelegramUser(string Nickname, long ChatId)
        {
            this.nick = Nickname;
            this.id = ChatId;
            Messages = new ObservableCollection<string>();
        }

        private string nick;
        public string Nick
        {
            get { return this.nick; }
            set
            {
                this.nick = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Nick)));
            }
        }

        private long id;
        public long Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;//оповещение внешних агентов

        /// <summary>
        /// Метод сравнения двух пользователей
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TelegramUser other) => other.Id == this.id;

        /// <summary>
        /// Все сообщения
        /// </summary>
        public ObservableCollection<string> Messages { get; set; }

        /// <summary>
        /// метод добавления сообщения
        /// </summary>
        /// <param name="Text"></param>
        public void AddMessage(string Text) => Messages.Add(Text);
    }
}
