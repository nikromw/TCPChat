using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }

        public string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        public string userName;
        public string userPass;
        public string userLogin;
        TcpClient client;
        ServerObject server;
        private bool registered = false;// объект сервера
        public TypeOfUser type = TypeOfUser.user;
        public List<Chat> clientChats { get; set; }

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
        }

        public void Commands(string message)
        {
            switch (message)
            {
                case "#Server: _Create_Chat":
                    message = GetMessage();
                    message = String.Format("{0}: {1}", userName, message);
                    clientChats.Add(server.CreateNewChat(message));
                    SendChats();
                    break;
                case "#Server: _Delete_Chat":
                    break;
                case "#Server: _Get_Chats":

                    break;
            }
        }

        public void SendChats()
        {
            string Chats = "";
            if (clientChats.Count() > 1)
            {
                foreach (var chat in clientChats)
                {
                    Chats += chat.Id + " " + chat.chatName + " ";
                }
            }
            server.RegOrEnterResponce(Chats, this.Id);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // получаем имя пользователя
                string message = GetMessage();
                string[] RegInput = message.Split(' ');
                dbCheck(RegInput);
                Registration();
                message = userName;
                // посылаем сообщение о входе в чат всем подключенным пользователям
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = String.Format("{0}: {1}", userName, message);
                        Console.WriteLine(message);
                        Commands(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch (Exception e)
                    {
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
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

        public void Registration()
        {
            if (!registered)
            {
                string message;
                server.AddConnection(this);
                server.RegOrEnterResponce("#Server: _User_Not_Found", this.Id);
                while (!registered)
                {
                    message = GetMessage();
                    string[] RegInput = message.Split(' ');
                    if (dbCheck(RegInput) == "Server: not found.")
                    {
                        ClientParam newClient = new ClientParam();
                        newClient.login = RegInput[0];
                        newClient.password = RegInput[1];
                        newClient.name = RegInput[2];
                        using (var db = new ClientContext())
                        {
                            db.clients.Add(newClient);
                            db.SaveChanges();
                        }
                        message = "#Server: _Successful_Login";
                        server.RegOrEnterResponce(message, this.Id);
                        break;
                    }
                    else
                    {
                        server.RegOrEnterResponce("#Server: _Fail_Registration", this.Id);
                    }
                }

            }
        }

        public string dbCheck(string[] RegInput)
        {
            string msg = "";
            using (var db = new ClientContext())
            {
                foreach (var dbclient in db.clients.ToList())
                {
                    if ((dbclient.login == RegInput[0] && dbclient.password == RegInput[1]) || (dbclient.login == RegInput[0] && dbclient.name == RegInput[2]))
                    {
                        registered = !registered;
                        userName = dbclient.name;
                        clientChats.AddRange(dbclient.ChatsOfClients.ToList());
                        server.AddConnection(this);
                        foreach (var chat in clientChats)
                        {
                            msg += chat.Id + " " + chat.chatName;
                        }
                        server.RegOrEnterResponce("#Server: _Success_Enter", this.Id);
                        server.RegOrEnterResponce($"#Server: _Get_Info_User {userName} {msg}", this.Id);
                        return "#Server: _Success_Enter";
                    }
                }
            }
            return "Server: not found.";
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