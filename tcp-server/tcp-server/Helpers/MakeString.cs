using System;
using System.Collections.Generic;
using System.Text;

namespace tcp_server.Helpers
{
    internal class MakeString
    {
        public static string MakeResponse(string body, int statusCode, string contentType = "text/plain")
        {
            
            string SCodeTxt = statusCode switch
            {
                200 => "OK",
                400 => "Bad Request",
                404 => "Not Found",
                405 => "Method Not Allowed",
                500 => "Internal Server Error",
                _ => "Unknown"
            };

            string response =
                $"HTTP/1.1 {statusCode} {SCodeTxt}\r\n" +
                $"Content-Type: {contentType}\r\n" +
                $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
                "\r\n" +
                body;

            return response;
        }

        public static string MakeRequest(string method, string path, string body = "", int id = -1)
        {
            string request = "";
            if (method == "GET" || method == "DELETE")
            {
                request =
                    $"{method} {path} HTTP/1.1\r\n" +
                    $"Host: {Data.address}:{Data.port}\r\n" +
                    $"Connection: close\r\n" +
                    "\r\n";
            }
            else if (method == "POST" || method == "PUT")
            {
                request =
                    $"{method} {path} HTTP/1.1\r\n" +
                    $"Host: {Data.address}:{Data.port}\r\n" +
                    $"Connection: closed\r\n" +
                    $"\r\n" +
                    body;
            }
            return request;
        }
    }
}
