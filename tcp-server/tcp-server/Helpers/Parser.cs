using System;
using System.Collections.Generic;
using System.Text;
using tcp_server.Models;

namespace tcp_server.Helpers
{
    internal class Parser
    {
        public static HttpRequest HttpParse(string rawReq)
        {
            HttpRequest parser = new HttpRequest();
            var reqParts = rawReq.Split("\r\n\r\n", 2);
            string headerPart = reqParts[0];
            string bodyPart = reqParts.Length >= 1 ? reqParts[1]:"";
            string[] lines = headerPart.Split("\r\n");

            string[] reqLine = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (reqLine.Length >= 3)
            {
                parser.Method = reqLine[0];
                parser.Path = reqLine[1].TrimEnd('/');
                if (parser.Path == "") { parser.Path = "/"; }
                parser.Version = reqLine[2];
            }

            for (int i = 1; i < lines.Length; i++)
            {
                int colonIndex = lines[i].IndexOf(':');

                if (colonIndex > 0)
                {
                    string headerName = lines[i].Substring(0, colonIndex).Trim();
                    string headerBody = lines[i].Substring(colonIndex + 1).Trim();
                    parser.Headers[headerName] = headerBody;
                }
            }

            parser.Body = bodyPart;
            
            return parser;
        }
    }
}
