using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        static string input;
        private const string host = "127.0.0.1";
        private static int port = 8000;
        static TcpClient client;
        static NetworkStream stream;
        private static bool registered = false;
        public MainControl()
        { 
            client = new TcpClient();
            client.Connect(host, port);
            InitializeComponent();
        }
        private void MainRegClick(object sender, RoutedEventArgs e)
        {
            input = LoginInput.Text;
            input += " " + PassInput;
            Connection();
            if (registered)
            {
                this.Content = new RegControl();
            }
        }

        public static void Connection()
        {
            try
            {
                //подключение клиента
                stream = client.GetStream(); // получаем поток
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
                        registered = true;
                        Thread receive = new Thread(new ThreadStart(Receive));
                        receive.Start();
                        SendMessage();
                        
                    }
                    if (message == "Not find. Registration." || message == "This login or name already used.")
                    {
                        MessageBox.Show("Нет такого пользователя.");
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
