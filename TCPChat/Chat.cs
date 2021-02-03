using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace TCPChat
{
   public class Chat
    {
        [Key]
        public string Id { get; set; }
        public string chatName { get; set; }
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public List<ClientObject> clientsOfChat { get; set; }
        public List<Message> MessagesOfChat { get; set; }
        public Chat()
        {
            clientsOfChat = new List<ClientObject>();
            MessagesOfChat = new List<Message>();
        }
        public Chat(string name , string _AdminName , string _AdminId)
        {
            chatName = name;
            AdminName = _AdminName;
            AdminId = _AdminId;
            Id = Guid.NewGuid().ToString();
            MessagesOfChat = new List<Message>();
            clientsOfChat = new List<ClientObject>();
        }

    }
}
