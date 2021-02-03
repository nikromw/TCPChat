using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for CreateChatW.xaml
    /// </summary>
    public partial class CreateChatW : Window
    {
        public CreateChatW()
        {
            InitializeComponent();
        }

       
        private void CreateChatBtnSmall(object sender, RoutedEventArgs e)
        {
            ClientConnection.SendMessage("#Server: _Create_Chat " + ClientConnection.Name + " " + NameChatBox.Text);
            this.Close();
        }
    }
}
