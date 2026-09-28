using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Models;

namespace tcp_server.Helpers
{
    internal class CWParse
    {
        public static void PrintParse(HttpRequest req)
        {
            Console.WriteLine($"Method: {req.Method}");
            Console.WriteLine($"Path: {req.Path}");
            Console.WriteLine($"Version: {req.Version}");
            foreach (KeyValuePair<string, string> entry in req.Headers)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            Console.WriteLine($"Body: {req.Body}");
        }
    }
}
