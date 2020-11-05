using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace TCPChat
{
    class ClientContext : DbContext
    {
        public ClientContext() : base("ConnectionDB")
        { }

        public DbSet<Chat> chats { get; set; }
        public DbSet<ClientParam> clients { get; set; }
    }
}
