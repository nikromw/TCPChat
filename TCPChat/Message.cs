using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace TCPChat
{
    enum TypeOfMessage
    {
        StringMessage = 1,
        FileMessage
    }
    public class Message
    {
        [Key]
        public string MessageId { get; set; }
        public string ChatId { get; set; }
        public string AuthorId { get; set; }
        public string Date { get; set; }
        public string DataOfMessage { get; set; }
        public string TypeOfMessage { get; set; }
        public Message()
        {
            MessageId = Guid.NewGuid().ToString();
        }

    }
}
