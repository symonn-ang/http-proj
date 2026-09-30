using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tcp_server.Models
{
    internal class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; } = "";
    }
}
