using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for RegControl.xaml
    /// </summary>
    public partial class RegControl : UserControl
    {
        ClientConnection connection;
        public RegControl()
        {
            InitializeComponent();
        }

        private void RegClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainControl();
        }

        private void RegClickBtn(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text.Length > 8 && PassBox.Text.Length > 8 && NameBox.Text.Length > 2)
            {
                connection = ClientConnection.GetInstance();
                connection.Connection("#Server: _Registration" + " " + LoginBox.Text + " " + PassBox.Text + " " + NameBox.Text);
                while (true)
                {
                    if (ClientConnection.registered == "registered")
                    {
                        this.Content = new ChatControl();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Login box lengh should be more than 8 symbols.");
            }
        }
    }
}
