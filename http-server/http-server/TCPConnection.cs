using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace http_server
{
    internal class TCPConnection
    {
        string address = "127.0.0.1";
        int port = 5000;
        static List<string> messages = new();
        public async Task StartConnection()
        {
            var listener = new TcpListener(IPAddress.Parse(address), port);
            
            try
            {
                listener.Start();
                Console.WriteLine($"Listening in {address}:{port}...");
                while (true)
                {
                    TcpClient handler = await listener.AcceptTcpClientAsync();
                    _ = HandleConnection(handler);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
            }
            finally
            {
                listener.Stop();
            }
        }

        public async Task HandleConnection(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                var routes = new Dictionary<string, Func<HttpRequest, string>>()
                {
                    ["GET /"] = HandleHome,
                    ["GET /time"] = HandleTime,
                    ["GET /messages"] = HandleGetMessages,
                    ["POST /echo"] = HandleEcho,
                    ["POST /message"] = HandlePostMessage,
                    //[$"DELETE /message/{id}"] = HandleDeleteMessage,
                    //["PUT /message/{id}"] = HandleEditMessage,
                };

                byte[] buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer);

                string request = Encoding.UTF8.GetString(buffer, 0, received);

                HttpRequest httpParse = ParseHttpRequest(request);

                PrintReq(httpParse);

                string route = $"{httpParse.Method} {httpParse.Path}";

                if (routes.TryGetValue(route, out var handler))
                {
                    string body = handler(httpParse);
                    string contentType = body.TrimStart().StartsWith('{') ? "application/json" : "text/plain";
                    string response = MakeResponse(body, 200, contentType);
                    var resByte = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(resByte);
                }
                else if (httpParse.Method == "DELETE" && httpParse.Path.StartsWith("/message/"))
                {
                    string idPart = httpParse.Path.Substring("/message/".Length);

                    if (int.TryParse(idPart, out int id) && id >= 0 && id < messages.Count)
                    {
                        messages.RemoveAt(id);
                        string body = JsonSerializer.Serialize(new { message = "Message Deleted." });
                        string response = MakeResponse(body, 200, "application/json");
                        await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                    }

                }
                else
                {
                    string response = MakeResponse("404 Not Found", 404);
                    var resByte = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resByte);
                }

                


            }
        }

        public HttpRequest ParseHttpRequest(string rawReq)
        {
            var parse = new HttpRequest();
            var parts = rawReq.Split("\r\n\r\n", 2);
            string headerPart = parts[0];
            string bodyPart = parts.Length >= 1 ? parts[1]:"";

            var lines = headerPart.Split("\r\n");
            if (lines.Length == 0)
            {
                return parse;
            }

            string[] reqLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (reqLine.Length >= 3)
            {
                parse.Method = reqLine[0];
                parse.Path = reqLine[1].TrimEnd('/');
                if (parse.Path == "") parse.Path = "/";
                parse.Version = reqLine[2];
            }

            for (int i = 1; i < lines.Length; i++)
            {
                int colonIndex = lines[i].IndexOf(':');

                if (colonIndex > 0)
                {
                    var headerName = lines[i].Substring(0, colonIndex).Trim();
                    var headerBody = lines[i].Substring(colonIndex + 1).Trim();
                    parse.Headers[headerName] = headerBody;
                }
            }

            parse.Body = bodyPart;

            return parse;
        }

        public string MakeResponse(string body, int statusCode, string contentType = "text/plain")
        {
            string statusText = statusCode switch
            {
                200 => "OK",
                404 => "Not Found",
                405 => "Method Not Allowed",
                _ => "Unknown"
            };

            string response =
                $"HTTP/1.1 {statusCode} {statusText}\r\n" +
                $"Content-Type: {contentType}\r\n" +
                $"Content-Lenght: {Encoding.UTF8.GetByteCount(body)}\r\n" +
                "\r\n" +
                body;

            return response;
        }

        public void PrintReq(HttpRequest req)
        {
            Console.WriteLine();
            Console.WriteLine($"Method: {req.Method}");
            Console.WriteLine($"Path: {req.Path}");
            Console.WriteLine($"Version: {req.Version}");
            foreach (KeyValuePair<string, string> entry in req.Headers)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }
            string body = req.Body == "" ? "empty" : req.Body!;
            Console.WriteLine($"Body: {body}");
            Console.WriteLine();
        }

        string HandleHome(HttpRequest req)
        {
            return "Welcome to LocalHost";

        }
        string HandleTime(HttpRequest req)
        {
            var data = new
            {
                UTCTime = DateTime.Now.ToString("ddd dd yyyy HH:mm:ss") + " GMT",
                DateTime = DateTime.Now,
            };
            string body = JsonSerializer.Serialize(data);
            return body;
        }
        string HandleEcho(HttpRequest req)
        {
            var data = new
            {
                echo = "Hello World"
            };
            string body = JsonSerializer.Serialize(data);
            return body;
        }
        string HandlePostMessage(HttpRequest req)
        {
            if (req.Body != null)
            {
                messages.Add(req.Body);
            }
            return "Message Created!";
        }
        string HandleGetMessages(HttpRequest req)
        {
            var stringConcat = new StringBuilder();
            foreach (string message in messages)
            {
                stringConcat.Append($"{message}");
            }
            return stringConcat.ToString();
        }

    }
}
