using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.Threading.Tasks.Dataflow;

namespace TCPChat
{
    public class ServerObject
    {
        static TcpListener tcpListener; 
        List<ClientObject> clients = new List<ClientObject>(); 

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal Chat CreateNewChat(string ChatData , string name ,  ClientObject Admin)
        {
            Chat newChat = new Chat(ChatData, name , Admin.Id);
            using (var db = new ClientContext())
            {
                var entity = db.clients.Where(i => i.Id == Admin.Id).First();
                newChat.clientsOfChat.Add(entity);
                db.chats.Add(newChat);
                db.SaveChanges();
            }
            return newChat;
        }

        public  void FindSizeOfMessage(string message , string Id)
        {
            
            byte[] data = Encoding.Unicode.GetBytes(message);
            int lenghtOfMessage = data.Length;
            RegOrEnterResponce("#Server: _Size_Of_Next_Message" + " " + lenghtOfMessage, Id);
        }

        //Remove connection
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
                clients.Remove(client);
        }

        //create connection
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8000);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }


        //mass sending of messages
        public void BroadcastMessage(Message message, string id, ClientObject sender , Chat chat)
        {
            string outdata = "#Server: _Chat_Message: " + message.ChatId +" "+ sender.userName +
                " "+ message.Date +" "+ message.DataOfMessage;

            byte[] data = Encoding.Unicode.GetBytes(outdata);
            
            foreach (var receiver in chat.clientsOfChat)
            {
                foreach (var client in clients)
                {
                    if (client.Id == receiver.Id)
                    {
                        FindSizeOfMessage(outdata, receiver.Id);
                        client.Stream.Write(data, 0, data.Length);
                    }
                }
            }
        }


        protected internal void RegOrEnterResponce(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (var client in clients)
            {
                if (client.Id == id)
                {
                    client.Stream.Write(data, 0, data.Length);
                }
            }
        }

       

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }
    }
}