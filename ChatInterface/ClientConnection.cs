using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Documents;

namespace ChatInterface
{
    public class ClientConnection
    {
        static string input;
        private const string host = "127.0.0.1";
        private static int port = 8000;
        static TcpClient client;
        static NetworkStream stream;
        static Socket socetStream;
        public static bool registered = false;
        private string login, password, name;
        private static ClientConnection singleClient;
        //private List<> chats = new List<T>(); 

        protected ClientConnection() { }

        protected ClientConnection(string _login, string _password)
        {
            login = _login;
            password = _password;
            client = new TcpClient();
        }
        protected ClientConnection(string _login, string _password, string _name)
        {
            login = _login;
            password = _password;
            name = _name;
            client = new TcpClient();
        }

        public ClientConnection GetInstance(string pass, string login)
        {
            if (singleClient == null)
            {
                singleClient= new ClientConnection(pass, login);
                return singleClient;
            }
            else
            {
                return singleClient;
            }
        }
        //Open connection and send login and password
        public void Connection(string inputData)
        {
            client = new TcpClient();
            client.Connect(host, port);
            stream = client.GetStream();
            Thread receiveThread = new Thread(new ThreadStart(EnterOrRegistrationCallBack));
            receiveThread.Start();
            byte[] data = Encoding.Unicode.GetBytes(inputData);
            stream.Write(data, 0, data.Length);

        }

        public void EnterOrRegistrationCallBack()
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                byte[] data = new byte[128]; // буфер для получаемых данных
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                string message = builder.ToString();
                if (message == "Successful login." || message == "Server: successfully found.")
                {
                    registered = true;
                    Thread receive = new Thread(new ThreadStart(Receive));
                    receive.Start();
                    SendMessage();

                }
                if (message == "Not find. Registration." || message == "This login or name already used.")
                {

                }
            }
        }

        // запускаем новый поток для получения данных
        public void StartReceive()
        {
            Thread receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start(); //старт потока
        }

        // отправка сообщений
        static void SendMessage()
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        //получение данных
        static void Receive()
        {
            byte[] data = new byte[128]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            while (true)
            {
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
            }

        }



        // получение сообщений
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[128]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                    if (message == "Successful login." || message == "Server: successfully found.")
                    {
                        registered = true;
                        Thread receive = new Thread(new ThreadStart(Receive));
                        receive.Start();
                        SendMessage();

                    }
                    if (message == "Not find. Registration." || message == "This login or name already used.")
                    {

                    }
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
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
