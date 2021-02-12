using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ChatInterface.ViewModels
{
    class ChatControlViewModel : INotifyPropertyChanged
    {
        private Chat selectedChat;
        public ObservableCollection<Chat> Chats { get; set; }
        private RelayCommand addCommand;
        // Так как получение чатов пока не налажено, создаю тестовый набор чатов 
        public ChatControlViewModel()
        {
            Chats = new ObservableCollection<Chat>
            {
                new Chat { ChatName = "TestChat1" , AdminId = "AdminId1", AdminName = "Admin1" , ChatId ="ChatId1" , ChatMessages = new List<Message>() , ChatUsers = new List<string> { "testUser1"} },
                new Chat { ChatName = "TestChat2" , AdminId = "AdminId2", AdminName = "Admin2" , ChatId ="ChatId2" , ChatMessages = new List<Message>() , ChatUsers = new List<string> { "testUser2"} },
                new Chat { ChatName = "TestChat3" , AdminId = "AdminId3", AdminName = "Admin3" , ChatId ="ChatId3" , ChatMessages = new List<Message>() , ChatUsers = new List<string> { "testUser3"} },
                new Chat { ChatName = "TestChat4" , AdminId = "AdminId4", AdminName = "Admin4" , ChatId ="ChatId4" , ChatMessages = new List<Message>() , ChatUsers = new List<string> { "testUser4"} },
                new Chat { ChatName = "TestChat5" , AdminId = "AdminId5", AdminName = "Admin5" , ChatId ="ChatId5" , ChatMessages = new List<Message>() , ChatUsers = new List<string> { "testUser5"} },
            };
        }

        public RelayCommand AddNewChat
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        string name = obj as string;
                        if (name != "" && name != null)
                        {
                            Chat newChat = new Chat();
                            newChat.ChatName = (name);
                            Chats.Add(newChat);
                        }
                    }));
            }
        }

        public Chat SelectedChat
        {
            get 
            { 
                return selectedChat; 
            }
            set
            {
                selectedChat = value;
                OnPropertyChanged("SelectedChat");
            
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
