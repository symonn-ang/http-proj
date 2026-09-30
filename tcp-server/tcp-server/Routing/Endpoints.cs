using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using tcp_server.Helpers;

namespace tcp_server.Routing
{
    internal class Endpoints
    {
        public static async Task GetMessages()
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Helpers.Data.address), Helpers.Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("GET", "/messages");
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            string response = await NetStream.Read(stream);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task PostMessage(string message)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Helpers.Data.address), Helpers.Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("POST", "/message", message);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            string response = await NetStream.Read(stream);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task EditMessage(string message, int id)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Helpers.Data.address), Helpers.Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("PUT", $"/message/{id}", message);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            string response = await NetStream.Read(stream);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task DeleteMessage(int id)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Helpers.Data.address), Helpers.Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("DELETE", $"/message/{id}");
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            string response = await NetStream.Read(stream);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }
    }
}
