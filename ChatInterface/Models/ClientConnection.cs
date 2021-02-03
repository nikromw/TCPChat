using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;

namespace ChatInterface
{
    public class ClientConnection : INotifyPropertyChanged
    {
        static string input;
        private const string host = "127.0.0.1";
        private static int port = 8000;
        private static int SizeStreamRead = 256;
        public static string response = "";
        static TcpClient client;
        static NetworkStream stream;

        static List<char> listdata = new List<char>();
        public static string registered = "registered";
        private string login, password;
        private static ClientConnection singleClient;
        public static ObservableCollection<Chat> Chats = new ObservableCollection<Chat>();
        public event PropertyChangedEventHandler PropertyChanged;
        public static Chat SelectedChat { get; set; }
        public static string Name { get; set; }

        protected ClientConnection()
        {
            Chats.CollectionChanged += ChangedChatsCollection;
        }

        protected ClientConnection(string _login, string _password)
        {
            login = _login;
            password = _password;
            client = new TcpClient();
            Chats.CollectionChanged += ChangedChatsCollection;
        }
        protected ClientConnection(string _login, string _password, string _name)
        {
            login = _login;
            password = _password;
            Name = _name;
            client = new TcpClient();
            Chats.CollectionChanged += ChangedChatsCollection;
        }

        public static void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public static ClientConnection GetInstance(string pass = "", string login = "", string name = "")
        {
            if (singleClient != null)
            {
                return singleClient;
            }
            if (singleClient == null & pass != "" & login != "")
            {
                singleClient = new ClientConnection(pass, login);
                return singleClient;
            }
            else if (singleClient == null & pass == "" & login == "")
            {
                singleClient = new ClientConnection(pass, login);
                return singleClient;
            }
            return singleClient;
        }
        //Open connection and send login and password
        public void Connection(string inputData)
        {
            client = new TcpClient();
            client.Connect(host, port);
            stream = client.GetStream();
            Thread receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start();
            byte[] data = Encoding.Unicode.GetBytes(inputData);
            stream.Write(data, 0, data.Length);
        }
       
        // запускаем новый поток для получения данных
        public void StartReceive()
        {
            Thread receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start(); //старт потока
        }

        //получение данных
         async void  Receive()
        {
            // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            StringBuilder BuilderForSize = new StringBuilder();
            object lockobj = new object();
            while (true)
            {
                int bytes = SizeStreamRead/2;
                string message;
                byte[] data = new byte[SizeStreamRead];
                do
                {
                    data = new byte[SizeStreamRead];
                    bytes = stream.Read(data, 0, SizeStreamRead);
                    await Task.Run(
                        () =>
                        {
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                            message = builder.ToString();
                            var split = message.Split(" ");
                                CheckCommands(message);
                            message = "";
                            builder.Clear();
                            bytes = 0;
                        })  ;
                   
                }
                while (stream.DataAvailable);
            }
        }


         async void CheckCommands(string command)
        {
            try
            {
                string[] commands = command.Split(' ');
                switch (commands[1])
                {
                    case "_Get_Chats":
                        UpdateChats(commands);
                        break;
                    case "_Size_Of_Next_Message":
                        SetSize(commands);
                        break;
                    case "_Success_Enter":
                        //EnterInitialize(commands);
                        registered = "registered";
                        break;
                    case "_Successful_Registration":
                        break;
                    case "#Server: _User_Not_In_Db  ":
                        registered = "unregistered";
                        break;
                }
            }
            catch (Exception e)
            { }
        }

        void EnterInitialize(string[] inputData)
        {
            Name = inputData[2];
            if (inputData.Length > 5)
            {
                for (int i = 3; i < inputData.Length-1; i += 4)
                {
                    Chat tmpChat = new Chat();
                    tmpChat.ChatId = inputData[i];
                    tmpChat.ChatName = inputData[i + 1];
                    tmpChat.AdminId = inputData[i + 2];
                    tmpChat.AdminName = inputData[i + 3];
                    Chats.Add(tmpChat);
                }
            }
        }

        static void SetSize(string[] commands)
        {
            try
            {
                if (Convert.ToInt32(commands[2]) >= 128)
                {
                    SizeStreamRead = 2*Convert.ToInt32(commands[2]);
                }
            }
            catch (Exception e)
            { }
        }

        static void UpdateChats(string[] chats)
        {

            string ChatsId = "";
            foreach (var chat in Chats)
            {
                ChatsId += chat.ChatId + " ";
            }
            string[] IdArr = ChatsId.Split(" ");
            for (int i = 2; i < chats.Length - 1; i += 4)
            {
                bool ChatIdCheck = IdArr.Contains(chats[i]);
                if (!ChatIdCheck)
                {
                    Chat NewChat = new Chat();
                    NewChat.ChatName = chats[i + 1];
                    NewChat.ChatId = chats[i];
                    NewChat.AdminName = chats[i + 3];
                    NewChat.AdminId = chats[i + 2];

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Chats.Add(NewChat);
                    });
                }
            }
        }
        private void ChangedChatsCollection(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.Action.ToString()));
        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();
            // Environment.Exit(0); //завершение процесса
        }
    }
}
