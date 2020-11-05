using System;
using System.Collections.Generic;
using System.Text;

namespace TCPChat
{
    public class ClientParam
    {
        
        public int Id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public List<Chat> ChatsOfClients { get; set; }

    }
}
