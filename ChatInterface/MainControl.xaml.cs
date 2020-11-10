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
        ClientConnection connection ;
        Client client = new Client();

        public MainControl()
        {
            
            InitializeComponent();
        }
        private void MainRegClick(object sender, RoutedEventArgs e)
        {
            //this.Content = new RegControl();
            this.Content = new ChatControl();
        }

        private void EnterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = client.GetConnection(LoginInput.Text, PassInput.Text);
                connection.Connection(LoginInput.Text+ " " + PassInput.Text);
                while (true)
                {
                    if (ClientConnection.registered)
                    {
                        connection.StartReceive();
                        break;
                    }
                }
                this.Content = new ChatControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
