using System.Net;
using System.Text;
using System.Net.Sockets;
using http_server;

//string uriString = "https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets?view=net-10.0";

//Uri canonicalUri = new(uriString);

//Console.WriteLine($"Host: {canonicalUri.Host}");
//Console.WriteLine($"Path&Query: {canonicalUri.PathAndQuery}");
//Console.WriteLine($"Fragment: {canonicalUri.Fragment}");


//string text = "Hello World\nHello Life";

//var reqBytes = Encoding.UTF8.GetBytes(text);
//var reqString = Encoding.UTF8.GetString(reqBytes);

//foreach (var bytes in reqBytes)
//{
//    Console.WriteLine(bytes);
//}

//foreach (var words in reqString)
//{
//    Console.Write(words);
//}
//var hostName = Dns.GetHostName();
//IPHostEntry ipHost = await Dns.GetHostEntryAsync(hostName); // httpbin.org
//IPAddress[] ipAdd = ipHost.AddressList;
//IPAddress ipAddress = ipHost.AddressList[0];

////Console.WriteLine(ipAdd);

//Console.WriteLine($"Hostname: {ipHost.HostName}");
//Console.WriteLine($"Addresses found: {ipHost.AddressList.Length}");

//foreach (var adds in ipAdd)
//{
//    Console.WriteLine(adds);
//}

//var ipEndPoint = new IPEndPoint(ipAddress, 13);

//using TcpClient client = new TcpClient(); // when doing tcp client u need to put something in it
//await client.ConnectAsync(ipEndPoint);
//await using NetworkStream stream = client.GetStream(); // Since the client knows that the message is small, the entire message can be read into
//                                                       // the read buffer in one operation. With larger messages, or messages with an
//                                                       // indeterminate length, the client should use the buffer more appropriately and read in
//                                                       // a while loop.

//var buffer = new byte[1_024];
//int received = await stream.ReadAsync(buffer);

//var message = Encoding.UTF8.GetString(buffer, 0, received);
//Console.WriteLine($"Message: {message}"); 

// TCP Server code ====================================================================================================================

//IPHostEntry ipHost = await Dns.GetHostEntryAsync("x.com");
//IPAddress address = ipHost.AddressList[0];
//int port = 5000;
//var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); // .Any listens to all local network interfaces (the machine's IPv4 ifs)
//                                                           // .Loopback makes server accessible from the same computer
//TcpListener listener = new(ipEndPoint);                    // loopback = IPAddress.Parse("127.0.0.1")
//     // ^ server-side TCP endpoint that waits for connections
//try
//{
//    listener.Start();
//    Task clientTask = testFile.CommenceTest();

//    Console.WriteLine($"Listening on port {port}...");

//    using var handler = await listener.AcceptTcpClientAsync(); // Accept method can infer to var // Flow 1
//    Console.WriteLine("Client connected.");                    // listener accepts the client trying to connect in the same (add, port)
//    await using NetworkStream stream = handler.GetStream(); // handler and client now in the same connection

//    var message = $"DateTime: {DateTime.Now}";
//    var dateTimeBytes = Encoding.UTF8.GetBytes(message);

//    //stream.Write(dateTimeBytes);
//    await stream.WriteAsync(dateTimeBytes);
//    Console.WriteLine($"Sent message: {message}"); // after sending, client will read it

//    await clientTask;
//}
//finally 
//{
//    listener.Stop();
//}

// TCP Server code ====================================================================================================================
// TCP Server that speaks HTTP protocl code ===========================================================================================

//IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
//TcpListener listener = new TcpListener(ipEndPoint);

//try
//{
//    listener.Start();
//    //Task clientReq = testFile.CommenceTest();

//    using TcpClient handler = await listener.AcceptTcpClientAsync(); // wait for connection
//    using NetworkStream stream = handler.GetStream();

//    byte[] buffer = new byte[4096];
//    int received = await stream.ReadAsync(buffer);

//    string request = Encoding.UTF8.GetString(buffer, 0, received); // read and decode done here, after that is option to parse

//    Console.WriteLine("Request: ");                                 // HTTP request parsing
//    Console.WriteLine(request);                                     // print out the connection's request

//    string body = 
//        "HTTP/1.1 200 OK\r\n" +
//        "Content-Type: text/plain; charset=utf-8\n" +
//        $"Content-Length: {Encoding.UTF8.GetByteCount(request)}\r\n" +
//        $"DateTime: {DateTime.Now}";                      // send out a response after

//    string response =   // response construction, can add different status, headers and body here // rn its a string, but if client is http ==
//        "HTTP/1.1 200 OK\r\n" +                                     // HTTP status // --> metadata
//        "Content-Type: text/plain\r\n" +                            // HTTP header // --> metadata
//        $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" + // HTTP header // --> metadata
//        "Connection: Close\r\n" +                                   // HTTP header // --> metadata // all of them can have a different res
//        "\r\n" +
//        body;                                                       // body        // --> content to display
//        // HTTP specifies that headers are separated by CRLF (\r\n), and that an empty CRLF separates the headers from the body.
//        // response doesn't execute anythin unless the application does/can do something about it, like say text/html it will interpret it as such
//        // ^ response/body

//    byte[] resBytes= Encoding.UTF8.GetBytes(response);
//    await stream.WriteAsync(resBytes);

//    Console.WriteLine($"Sent Message: {body}");

//    //await clientReq;
//}
//finally
//{
//    listener.Stop();
//}

// TCP Server that speaks HTTP protocl code ===========================================================================================
// speaks the HTTP protocol (you manually create the HTTP response headers + body)
// make it loop

static async Task StartConnection()
{
    string address = "127.0.0.1";
    int port = 5000;
    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
    TcpListener listener = new TcpListener(ipEndPoint);

    try
    {
        listener.Start();
        Console.WriteLine($"Listening on http://{address}:{port}");
        while (true)
        {
            TcpClient handler = await listener.AcceptTcpClientAsync(); // tried to do using here, but let the discard op dispose it
            _ = HandleConnection(handler);
        }
    }
    finally
    {
        listener.Stop();
    }
}

static async Task HandleConnection(TcpClient client)
{
    using (client)                                    // It’s the safe and recommended way to make sure you don’t leak connections/sockets,
    using (NetworkStream stream = client.GetStream()) // even if something goes wrong inside the block. (Resource Cleanup (hover))
    {
        byte[] buffer = new byte[4096];
        int received = await stream.ReadAsync(buffer);

        string request = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine("Request:");
        Console.Write(request);

        string body = $"DateTime: {DateTime.Now}";

        string response =
            "HTTP/1.1 200 OK\r\n" +
            "Content-Type: text/plain\r\n" +
            $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
            "\r\n" +
            body;

        byte[] resByte = Encoding.UTF8.GetBytes(response);
        await stream.WriteAsync(resByte);

        Console.WriteLine($"Sent Message: {body}\n");
    }

}

var serverTask = StartConnection();

await Task.Delay(300);

await testFile.ConnectTest();

await serverTask;