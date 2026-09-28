using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Models;

namespace tcp_server.Helpers
{
    internal class Data
    {
        public static string address = "127.0.0.1";
        public static int port = 5000;
        public static List<Message> messages = new List<Message>();
        public static int id = 1;
    }
}
