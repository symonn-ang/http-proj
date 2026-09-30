using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using tcp_server.Data;
using tcp_server.Helpers;
using tcp_server.Models;

namespace tcp_server.Routing
{
    internal class Routes
    {
        public static string HandleGetMessages(HttpRequest req)
        {
            using MessageContext _context = new MessageContext();

            if (Helpers.Data.messages != null)
            {
                //throw new Exception("Test exception");
                return JsonSerializer.Serialize(_context.Messages);
            }

            return JsonSerializer.Serialize("Message list is empty.");
        }

        public static string HandlePostMessage(HttpRequest req)
        {
            using MessageContext _context = new MessageContext();

            if (req.Body != null)
            {
                Message message = new Message()
                {
                    Text = req.Body
                };

                _context.Add(message);
                _context.SaveChanges();
            }
            return "Message Created!";
        }
    }
}
