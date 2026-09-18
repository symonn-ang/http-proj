using System;
using System.Collections.Generic;
using System.Text;

namespace http_server
{
    internal class HttpRequest
    {
        public string Method { get; set; } = "";
        public string Path { get; set; } = "";
        public string Version { get; set; } = "";
        public Dictionary<string, string> Headers { get; set; } = new();
        public string? Body { get; set; }

    }
}
