using System;
using System.Collections.Generic;
using System.Text;

namespace ChatInterface
{
    public class Chat
    {
        public string ChatName { get; set; }
        public string ChatId { get; set; }
        public string AdminName { get; set; }
        public string AdminId { get; set; }
        public List<Message> ChatMessages = new List<Message>();
        public List<string> ChatUsers = new List<string>();

    }
}
