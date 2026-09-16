using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace http_server
{
    internal class testFile
    {
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

        public static async Task CommenceTest()
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);
            using NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int receieved = await stream.ReadAsync(buffer);

            string message = Encoding.UTF8.GetString(buffer, 0, receieved);

            Console.WriteLine($"Message Received: {message}");
        }

    }
}
