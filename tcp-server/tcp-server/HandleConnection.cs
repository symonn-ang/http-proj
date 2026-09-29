using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using tcp_server.Helpers;
using tcp_server.Middleware;
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
                string request = await NetStream.Read(stream);

                HttpRequest parser = Parser.HttpParse(request);
                //Console.WriteLine();
                //CWParse.PrintParse(parser);  

                await Middlewares.Pipeline(parser, stream);
            }
        }
    }
}
