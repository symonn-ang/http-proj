using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using tcp_server.Helpers;
using tcp_server.Models;
using tcp_server.Routing;

namespace tcp_server
{
    internal class HandleConnection
    {
        public static async Task Handler(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                var routes = new Dictionary<string, Func<HttpRequest, string>>()
                {
                    ["GET /messages"] = Routes.HandleGetMessages,
                    ["POST /message"] = Routes.HandlePostMessage,
                };

                byte[] buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer);
                string request = Encoding.UTF8.GetString(buffer, 0, received);

                HttpRequest parser = Parser.HttpParse(request);
                Console.WriteLine();
                CWParse.PrintParse(parser);

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
                        var message = Data.messages.FirstOrDefault(x => x.Id == id);
                        if (message != null)
                        {
                            message.Text = parser.Body ?? message.Text;
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
                        var message = Data.messages.FirstOrDefault(x => x.Id == id);
                        if (message != null)
                        {
                            Data.messages.Remove(message);
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
}
