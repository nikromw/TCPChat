using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TCPChat
{
    public class ClientParam
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public List<Chat> ChatsOfClients { get; set; }

    }
}
