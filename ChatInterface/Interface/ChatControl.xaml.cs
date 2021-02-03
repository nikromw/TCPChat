using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatInterface
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        ClientConnection connection;
        object obj = new object();
        string SelectedChatId ="";
        public ChatControl()
        {
            connection = ClientConnection.GetInstance();
            InitializeComponent();
        }

        private void CreateChatBtn(object sender, RoutedEventArgs e)
        {
            Window chatCreateW = new CreateChatW();
            chatCreateW.Show();
        }


        private void ChatOptionsBtn(object sender, RoutedEventArgs e)
        {
            // Add user in chat 
            // Delete user from chat 
            // block user 
            // Change name Chat
            // ShowUsers
        }

        private void FindSelectedChat(object sender, MouseEventArgs e)
        {
            foreach (var chat in ClientConnection.Chats)
            {
               
            }
        }

        private void SendMessageBox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClientConnection.SendMessage("Server: _Chat_Message:" + " " + SelectedChatId + " " + "_text_message" + " " + MessageBox.Text);
            }
        }
    }
}
