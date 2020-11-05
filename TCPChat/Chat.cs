using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChat
{
   public class Chat
    {
        public string Id { get; set; }
        public string chatName { get; set; }
        public List<ClientParam> clientsOfChat { get; set; }
        public Chat(string name)
        {
            chatName = name;
            Id = Guid.NewGuid().ToString();
        }
    }
}
