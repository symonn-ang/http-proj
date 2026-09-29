using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using tcp_server.Helpers;
using tcp_server.Models;

namespace tcp_server.Middleware
{
    internal class Middlewares
    {
        public static async Task Pipeline(HttpRequest request, NetworkStream stream)
        {
            try
            {
                LoggingMiddleware(request);

                await HandleRoutes.Controllers(request, stream);
            }
            catch (Exception e) // Exception Handling Middleware
            {
                Console.WriteLine(e.Message);   
                string response = MakeString.MakeResponse($"Error: {e}", 500);
                await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
            }
        }
        static void LoggingMiddleware(HttpRequest req)
        {
            Console.WriteLine();
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}, {req.Method} {req.Path}");
        }
    }
}
