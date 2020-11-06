using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace TCPChat
{
    public class ServerObject
    {
        static TcpListener tcpListener; // сервер для прослушивания
        static TcpListener tcpListenerReg; // сервер для 
        List<ClientObject> clients = new List<ClientObject>(); // все подключения
        List<ClientParam> paramClients = new List<ClientParam>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal Chat CreateNewChat(string name)
        {
            return new Chat(name);
        }

        protected internal void AddUserInChat()
        { 
        
        }

        protected internal void GetChatsOfUser(string id)
        {
            try
            {
                using (var db = new ClientContext())
                {

                }
            }
            catch(Exception e)
            { }
        }
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
        // прослушивание входящих подключений
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

        // трансляция сообщения подключенным клиентам
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
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