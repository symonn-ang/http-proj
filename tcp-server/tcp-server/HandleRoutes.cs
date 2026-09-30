using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using tcp_server.Data;
using tcp_server.Helpers;
using tcp_server.Models;
using tcp_server.Routing;

namespace tcp_server
{
    internal class HandleRoutes
    {
        public static async Task Controllers(HttpRequest parser, NetworkStream stream)
        {
            using MessageContext _context = new MessageContext();

            var routes = new Dictionary<string, Func<HttpRequest, string>>()
            {
                ["GET /messages"] = Routes.HandleGetMessages,
                ["POST /message"] = Routes.HandlePostMessage,
            }; 

            string route = $"{parser.Method} {parser.Path}";

            if (routes.TryGetValue(route, out var handler))
            {
                var body = handler(parser);
                string contentType = body.TrimStart().StartsWith('{') ? "application/json" : "text/plain";
                string response = MakeString.MakeResponse(body, 200);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
            }
            else if (parser.Method == "PUT" && parser.Path.StartsWith("/message/"))
            {
                if (int.TryParse(parser.Path.Substring("/message/".Length), out int id))
                {
                    //var message = Helpers.Data.messages.FirstOrDefault(x => x.Id == id);
                    var message = await _context.Messages.FindAsync(id);

                    if (message != null)
                    {
                        message.Text = parser.Body ?? message.Text;
                        await _context.SaveChangesAsync();
                    }
                    string response = MakeString.MakeResponse("Message Edited!", 200);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                }
                else
                {
                    string response = MakeString.MakeResponse("Invalid ID", 404);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                }
            }
            else if (parser.Method == "DELETE" && parser.Path.StartsWith("/message/"))
            {
                if (int.TryParse(parser.Path.Substring("/message/".Length), out int id))
                {
                    //var message = Helpers.Data.messages.FirstOrDefault(x => x.Id == id);
                    var message = await _context.Messages.FindAsync(id);
                    if (message != null)
                    {
                        //Helpers.Data.messages.Remove(message);
                        _context.Messages.Remove(message);
                        await _context.SaveChangesAsync();
                    }
                    string response = MakeString.MakeResponse("Message Deleted!", 200);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                }
                else
                {
                    string response = MakeString.MakeResponse("Invalid ID", 404);
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                }
            }
            else
            {
                string response = MakeString.MakeResponse("Invalid Route", 400);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
            }

        }
    }
}
