using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using tcp_server.Helpers;

namespace tcp_server
{
    internal class Connection
    {
        public async Task StartConnection()
        {
            var listener = new TcpListener(IPAddress.Parse(Helpers.Data.address), Helpers.Data.port);

            try
            {
                listener.Start();
                Console.WriteLine($"Listening in... {Helpers.Data.address}:{Helpers.Data.port}");
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = HandleConnection.Handler(client);
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
    }
}
