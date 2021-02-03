using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Data;

namespace ChatClient
{
    class Program
    {
        static string input;
        private const string host = "127.0.0.1";
        private static int port = 8000;
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            
            Connection();
        }

        public static void Connection()
        {
            try
            {
                client = new TcpClient();
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток
                Console.Write("Введите логин: ");
                input = Console.ReadLine();
                Console.Write("Введите пароль: ");
                input += Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(input);
                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока
                stream.Write(data, 0, data.Length);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // отправка сообщений
        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

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
                Console.WriteLine(builder);
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
                        Thread receive = new Thread(new ThreadStart(Receive));
                        receive.Start();
                        SendMessage();
                    }
                    if (message == "Not find. Registration." || message == "This login or name already used.")
                    {
                        Console.WriteLine("Введите логин : ");
                        message = Console.ReadLine();
                        Console.WriteLine("Введите пароль : ");
                        message +=" "+ Console.ReadLine();
                        Console.WriteLine("Введите никнейм : "); 
                        message +=" "+ Console.ReadLine();
                        data = Encoding.Unicode.GetBytes(message);
                        stream.Write(data, 0, data.Length);
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