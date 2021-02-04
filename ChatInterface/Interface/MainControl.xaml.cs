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
using System.Threading.Tasks;
using ChatInterface.ViewModels;
using ChatInterface.Models;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        ClientConnection connection;

        public MainControl()
        {

            InitializeComponent();
            DataContext = new ChatControlViewModel();
        }
        private void MainRegClick(object sender, RoutedEventArgs e)
        {
            this.Content = new RegControl();
        }

        private void EnterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //connection = Client.GetConnection(LoginInput.Text, PassInput.Text);
                // connection.Connection("#Server: _Enter" + " " + LoginInput.Text + " " + PassInput.Text);
                MessageBox.Show(CheckIpBox.TextCheck.ToString());

                while (true)
                {
                    if (ClientConnection.registered == "registered")
                    {
                       // connection.StartReceive();
                        this.Content = new ChatControl();
                        break;
                    }
                    if (ClientConnection.registered == "unregistered")
                    {
                        MessageBox.Show("Такой пользовательно не зарегестрирован.");
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
