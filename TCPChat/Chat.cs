using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TCPChat
{
   public class Chat
    {
        [key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ChatId { get; set; }
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
