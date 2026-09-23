using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace http_server
{
    internal class testFile
    {
        private static string address = "127.0.0.1";
        private static int port = 5000;
        private static int id = 0;

        public static async Task PostMessage(string message)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(address), port);
            using NetworkStream stream = client.GetStream();

            var data = new
            {
                id = id++,
                message = message
            };

            string body = JsonSerializer.Serialize(data);

            string request =
                "POST /message HTTP/1.1\r\n" +
                $"Host: {address}:{port}\r\n" +
                "Content-Type: text/plain\r\n" +
                $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
                "Connection: close\r\n" +
                "\r\n" +
                body;

            var reqByte = Encoding.UTF8.GetBytes(request);
            await stream.WriteAsync(reqByte);

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);

            string response = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine();
            Console.WriteLine($"Received:\n{response}");
        }

        public static async Task GetMessages()
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(address), port);
            using NetworkStream stream = client.GetStream();
            string request =
                "GET /messages HTTP/1.1\r\n" +
                $"Host: {address}:{port}\r\n" +
                $"Connection: close\r\n" +
                "\r\n";

            byte[] resByte = Encoding.UTF8.GetBytes(request);
            await stream.WriteAsync(resByte);

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);

            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"Received:\n{response}");

        }

        public static async Task DeleteMessage(int id)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(address), port);
            using NetworkStream stream = client.GetStream();

            string request =
                $"DELETE /message/{id} HTTP/1.1\r\n" +
                $"Host: {address}:{port}\r\n" +
                $"Connection: close\r\n" +
                "\r\n";
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);

            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}");

        }

        public static async Task EditMessage(int id, string message)
        {
            var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Parse(address), port);
            using NetworkStream stream = client.GetStream();

            var data = new
            {
                id = id,
                message = message
            };

            string body = JsonSerializer.Serialize(data);

            string request =
                $"PUT /message/{id} HTTP/1.1\r\n" +
                $"Host: {address}:{port}\r\n" +
                "Connection: close\r\n" +
                "\r\n" +
                body;
            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

            var buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);

            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"\nReceived:\n{response}");

        }

        //public static async Task CommenceTest()
        //{
        //    using TcpClient client = new TcpClient(); // TCP client establishes a TCP connection == ConnectAsync establishes the connection
        //    await client.ConnectAsync("127.0.0.1", 5000); // flow 2, this only exists, 127.0.0.1 is default for loopback
        //    await using NetworkStream stream = client.GetStream(); // network stream is for reading/writing bytes

        //    var buffer = new byte[1024]; // init 0 == null in ascii // buffer is where the bytes will be stored later in read
        //    //foreach (byte bit in buffer)
        //    //{
        //    //    Console.Write($"{bit} ");
        //    //}                             // read async takes bytes from stream and puts each in buffer arr
        //    int received = await stream.ReadAsync(buffer); // think of buffer as length or total amount of bytes in a msg(check main)
        //    Console.WriteLine(received);                   // received waits for Write
        //    string message = Encoding.UTF8.GetString(buffer, 0, received); // GetBytes = Encoding, GetString = Decoding
        //                                                        // received actually - 1 cause index
        //                                                        // so im assuming buffer arr, start index 0, less than received
        //    Console.WriteLine($"Received: {message}");
        //}

        //public static async Task CommenceTest()
        //{
        //    using TcpClient client = new TcpClient();
        //    await client.ConnectAsync("127.0.0.1", 5000);
        //    using NetworkStream stream = client.GetStream();

        //    byte[] buffer = new byte[1024];
        //    int receieved = await stream.ReadAsync(buffer);

        //    string message = Encoding.UTF8.GetString(buffer, 0, receieved);

        //    Console.WriteLine($"Message Received: {message}");
        //}

        //public static async Task ConnectTest()
        //{
        //    using TcpClient client = new TcpClient();
        //    await client.ConnectAsync(IPAddress.Parse(address), port);
        //    using NetworkStream stream = client.GetStream();

        //    string request =
        //        "GET HTTP/1.1\r\n" +
        //        $"Host: {address}\r\n" +
        //        "Connection: close\r\n" +
        //        "\r\n";

        //    byte[] reqByte = Encoding.UTF8.GetBytes(request);
        //    await stream.WriteAsync(reqByte);

        //    byte[] buffer = new byte[4096];
        //    var received = await stream.ReadAsync(buffer);

        //    string res = Encoding.UTF8.GetString(buffer, 0, received);
        //    Console.WriteLine($"From testFile:\n{res}");

        //}


        //public static async Task testClient(string message, int id)
        //{
        //    var client = new TcpClient();
        //    await client.ConnectAsync(IPAddress.Parse(address), port);
        //    using NetworkStream stream = client.GetStream();

        //    //string body = "Hello from testClient";

        //    //string request =
        //    //    "POST /echo HTTP/1.1\r\n" +
        //    //    $"Host: {address}:{port}\r\n" +
        //    //    "Content-Type: text/plain\r\n" +
        //    //    $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
        //    //    "Connection: close\r\n" +
        //    //    "\r\n" +
        //    //    body;

        //    var data = new
        //    {
        //        id = id,
        //        message = message
        //    };

        //    string body = JsonSerializer.Serialize(data);

        //    string request =
        //        "POST /message HTTP/1.1\r\n" +
        //        $"Host: {address}:{port}\r\n" +
        //        "Content-Type: text/plain\r\n" +
        //        $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
        //        "Connection: close\r\n" +
        //        "\r\n" +
        //        body;

        //    var reqByte = Encoding.UTF8.GetBytes(request);
        //    await stream.WriteAsync(reqByte);

        //    byte[] buffer = new byte[4096];
        //    int received = await stream.ReadAsync(buffer);

        //    string response = Encoding.UTF8.GetString(buffer, 0, received);
        //    Console.WriteLine();
        //    Console.WriteLine($"Received:\n{response}");
        //}

    }
}
