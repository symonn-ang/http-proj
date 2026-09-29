using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace tcp_server.Helpers
{
    internal class NetStream
    {
        public static async Task<string> Read(NetworkStream stream)
        {
            byte[] buffer = new byte[4096];
            int received = await stream.ReadAsync(buffer);
            string request = Encoding.UTF8.GetString(buffer, 0, received);

            return request;
        }
    }
}
