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
            await client.ConnectAsync(IPAddress.Parse(Data.address), Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("GET", "/messages");
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task PostMessage(string message)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Data.address), Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("POST", "/message", message);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task EditMessage(string message, int id)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Data.address), Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("PUT", $"/message/{id}", message);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }

        public static async Task DeleteMessage(int id)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(Data.address), Data.port);
            using NetworkStream stream = client.GetStream();

            string request = MakeString.MakeRequest("DELETE", $"/message/{id}");
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}\n");
        }
    }
}
