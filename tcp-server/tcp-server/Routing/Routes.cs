using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using tcp_server.Helpers;
using tcp_server.Models;

namespace tcp_server.Routing
{
    internal class Routes
    {
        public static string HandleGetMessages(HttpRequest req)
        {
            if (Data.messages != null)
            {
                return JsonSerializer.Serialize(Data.messages);
            }

            return JsonSerializer.Serialize("Message list is empty.");
        }

        public static string HandlePostMessage(HttpRequest req)
        {
            if (req.Body != null)
            {
                Message message = new Message()
                {
                    Id = Data.id,
                    Text = req.Body
                };
                Data.messages.Add(message);
                Data.id++;
            }
            return "Message Created!";
        }
    }
}
