using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPChat
{
    public enum TypeOfUser
    {
        admin,
        moderator,
        user
    }
    public class ClientObject
    {

        #region clients fields
        public string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }

        [MaxLength(254)]
        [Column(TypeName = "VARCHAR")]
        public string userName { get; set; }

        [MaxLength(254)]
        [Column(TypeName = "VARCHAR")]
        public string userPass { get; set; }

        [MaxLength(254)]
        [Column(TypeName = "VARCHAR")]
        public string userLogin { get; set; }

        [MaxLength(254)]
        [Column(TypeName = "VARCHAR")]

        public TcpClient client;
        public ServerObject server;
        private bool registered = false;
        public string Usertype { get; set; }
        public List<Chat> clientChats { get; set; }
        #endregion

        #region ctors
        public ClientObject()
        {
            clientChats = new List<Chat>();
        }

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            client = tcpClient;
            server = serverObject;
            Usertype = "user";
            clientChats=new List<Chat>();
        }
        #endregion

        public async void Commands(string message)
        {
            string[] command = message.Split(' ');
            switch (command[1])
            {
                case "_Create_Chat":
                    message = String.Format("{0}: {1}", userName, message);
                    clientChats.Add(server.CreateNewChat(command[3], this.userName,  this));
                    SendChats();
                    break;
                case "_Delete_Chat":
                    break;
                case "_Get_Chats":
                    break;
                case "_Chat_Message:":
                    await Task.Run(() => SetMessageInChat(command));
                    break;
            }
        }

        //Get message and put it in the chat
        private async void SetMessageInChat(string[] command)
        {
            foreach (Chat chat in clientChats)
            {
                if (chat.Id == command[2])
                {
                    Message msg = new Message();
                    msg.AuthorId = this.Id;
                    msg.Date = DateTime.Today.ToString("dd.MM.yyyy hh:mm:ss:fff");
                    msg.TypeOfMessage = command[3];
                    msg.ChatId = command[2];
                    msg.DataOfMessage = command[4];
                    chat.MessagesOfChat.Add(msg);
                    await Task.Run(() => server.BroadcastMessage(msg , chat.Id , this , chat));
                    using (var db = new ClientContext())
                    {
                        var entity = db.chats.Where(i => i.Id == command[2]).First();
                        entity.MessagesOfChat.Add(msg);
                        db.SaveChanges();
                    }
                }
            }

        }

        public void SendChats()
        {
            if (this.clientChats.Count() >= 1)
            {
                foreach (var chat in clientChats)
                {
                    string ChatsInfo = "";
                    string MessagesInfo = "";
                    string ClientsInfo = "";

                    // Colect info about chat 
                    if (chat?.MessagesOfChat != null)
                    {
                        foreach (var message in chat?.MessagesOfChat)
                        {
                            MessagesInfo = message + " ";
                        }
                    }

                    // Collect info about cliets in chat
                    foreach (var client in chat.clientsOfChat)
                    { 
                    
                    }
                    ChatsInfo += chat.Id + " " + chat.chatName + " " + chat.AdminId +
                        " " + chat.AdminName + " " + MessagesInfo;

                    server.FindSizeOfMessage("#Server: _Get_Chats" + " " + ChatsInfo , this.Id);
                    server.RegOrEnterResponce("#Server: _Get_Chats" + " " + ChatsInfo, this.Id);
                }
            }
        }

        public void Process()
        {
            try
            {
                string message;
                Stream = client.GetStream();
                do
                {
                    message = GetMessage();
                    //проверяем тип сообщения (регистрация , вход , файл , видео )
                    string[] RegInput = message.Split(' ');
                    if (RegInput[1] == "_Enter")
                    {
                        dbCheck(RegInput);
                    }
                    else if (RegInput[1] == "_Registration")
                    {
                        Registration(message);
                    }
                } while (!registered);
                message = userName;
                Console.WriteLine($"{userName}: вошел в чат");
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        // Добавить реализацию действия пользователя 
                        Commands(message);
                    }
                    catch (Exception e)
                    {
                        message = String.Format($"{userName}: покинул чат");
                        Console.WriteLine(message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        public void Registration(string inputData)
        {
            if (!registered)
            {
                string command;
                server.AddConnection(this);
                while (!registered)
                {
                    string[] RegInput = inputData.Split(' ');
                    if (dbCheck(RegInput) == "#Server: _User_Not_In_Db")
                    {
                        this.userLogin = RegInput[2];
                        this.userPass = RegInput[3];
                        this.userName = RegInput[4];
                        using (var db = new ClientContext())
                        {
                            db.clients.Add(this);
                            db.SaveChanges();
                        }

                        Id = Guid.NewGuid().ToString();
                        command = "#Server: _Successful_Registration";
                        registered = !registered;
                        server.FindSizeOfMessage(command + userName, this.Id);
                        server.RegOrEnterResponce(command + " " + userName, Id);
                        break;
                    }
                    else
                    {
                        server.FindSizeOfMessage("#Server: _Login_Or_Password_Already_Used", this.Id);
                        server.RegOrEnterResponce("#Server: _Login_Or_Password_Already_Used", this.Id);
                    }
                }
            }
        }

        public string dbCheck(string[] RegInput)
        {
            server.AddConnection(this);
            using (var db = new ClientContext())
            {
                foreach (ClientObject dbclient in db.clients.Include(i=>i.clientChats))
                {
                    if ((dbclient.userLogin == RegInput[2] && dbclient.userPass == RegInput[3]) || (dbclient.userLogin == RegInput[0] && dbclient.userName == RegInput[2]))
                    {
                        server.RegOrEnterResponce($"#Server: _Success_Enter" + " " + dbclient.userName, Id);
                        registered = !registered;
                        userName = dbclient.userName;
                        Id = dbclient.Id;
                        userLogin = dbclient.userLogin;
                        userPass = dbclient.userPass;
                        clientChats.AddRange(dbclient.clientChats.ToList());
                        this.SendChats();
                        return "#Server: _Success_Enter";
                    }
                }
            }
            server.RegOrEnterResponce("#Server: _User_Not_In_Db", this.Id);
            return "#Server: _User_Not_In_Db";
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}