using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace http_server
{
    internal class Connection
    {
        string address = "127.0.0.1";
        int port = 5000;

        public async Task StartConnection()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            Console.WriteLine(IPAddress.Parse(address));
            var listener = new TcpListener(ipEndPoint);

            try
            {
                listener.Start();
                Console.WriteLine($"Listening in: {address}:{port}");
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = HandleConnection(client);
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
                var buffer = new byte[4096];
                int received = await stream.ReadAsync(buffer);

                string request = Encoding.UTF8.GetString(buffer, 0, received);
                //Console.WriteLine("Request: ");
                //Console.WriteLine(request);
                HttpRequest httpParse = ParseHttpRequest(request);
                Console.WriteLine();
                Console.WriteLine($"Method: {httpParse.Method}");
                Console.WriteLine($"Path: {httpParse.Path}");
                Console.WriteLine($"Version: {httpParse.Version}");
                foreach (KeyValuePair<string, string> entry in httpParse.Headers)
                {
                    Console.WriteLine($"  {entry.Key}: {entry.Value}");
                }
                Console.WriteLine();
                
                if (httpParse.Method == "GET" && httpParse.Path == "/")
                {
                    string body = $"Welcome to LocalHost";
                    string response = MakeResponse(body, 200);

                    var resByte = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resByte);

                    Console.WriteLine($"Sent Message: {body}");
                }
                else if (httpParse.Method == "GET" && httpParse.Path == "/favicon.ico")
                {
                    string body = "{\"message\": \"Welcome to Favicon\"}";

                    string response = MakeResponse(body, 200, "application/json");

                    var resByte = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resByte);
                    
                    Console.WriteLine($"Sent Message: {body}");
                }
                else if (httpParse.Method == "GET" && httpParse.Path == "/time")
                {
                    //string body = $"{{\"DateTime1\": \"{DateTime.Now:yyyy-MM-dd HH:mm:ss tt}\", \"DateTime2\": \"{DateTime.Now}\"}}"; // or {DateTime.Now} non custom

                    var data = new
                    {
                        DateTime1 = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss t") + " GMT", // tried to get close to asp.net
                        DateTime2 = DateTime.Now
                    };

                    string body = JsonSerializer.Serialize(data);

                    string response = MakeResponse(body, 200, "application/json");

                    var resByte = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resByte);

                    Console.WriteLine($"Sent Message: {body}");
                }
                else if (httpParse.Method == "POST" && httpParse.Path == "/echo")
                {
                    var data = new
                    {
                        message = httpParse.Body ?? "",
                    };

                    string body = JsonSerializer.Serialize(data);

                    string response = MakeResponse(body, 200, "application/json");

                    var resBytes = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resBytes);

                    Console.WriteLine($"Sent Message: {response}");
                }
                else
                {
                    string response = MakeResponse("404 Not Found", 404);

                    var resByte = Encoding.UTF8.GetBytes(response);

                    await stream.WriteAsync(resByte);

                    Console.WriteLine("404 Not Found");
                }


            }
        }

        public HttpRequest ParseHttpRequest(string rawReq)
        {
            HttpRequest parse = new HttpRequest();
            var parts = rawReq.Split("\r\n\r\n" , 2); // is equiv to rawReq.Split(new[] { "\r\n\r\n" } , 2, StringSplitOptions.None);
            string headerPart = parts[0];
            string bodyPart = parts.Length > 1 ? parts[1] : ""; // always check potential errs

            var lines = headerPart.Split("\r\n");

            if (lines.Length == 0) return parse;

            var reqLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (reqLine.Length >= 3)
            {
                parse.Method = reqLine[0];
                parse.Path = reqLine[1].TrimEnd('/');
                if (parse.Path == "") parse.Path = "/";
                parse.Version = reqLine[2];
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    break;
                }

                int colonIndex = lines[i].IndexOf(':');
                if (colonIndex > 0)
                {
                    string headerName = lines[i].Substring(0, colonIndex).Trim();
                    string headerBody = lines[i].Substring(colonIndex + 1).Trim();
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
                _ => "Unknown"
            };

            return
                $"HTTP/1.1 {statusCode} {statusText}\r\n" +
                $"Content-Type: {contentType}\r\n" +
                $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
                "\r\n" +
                body;
        }
    }
}
