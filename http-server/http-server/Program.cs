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

// HTTP code ====================================================================================================================

IPHostEntry ipHost = await Dns.GetHostEntryAsync("x.com");
IPAddress address = ipHost.AddressList[0];
int port = 5000;
var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port); // .Any listens to all local network interfaces (the machine's IPv4 ifs)
                                                           // .Loopback makes server accessible from the same computer
TcpListener listener = new(ipEndPoint);                    // loopback = IPAddress.Parse("127.0.0.1")
     // ^ server-side TCP endpoint that waits for connections
try
{
    listener.Start();
    Task clientTask = testFile.CommenceTest();

    Console.WriteLine($"Listening on port {port}...");

    using var handler = await listener.AcceptTcpClientAsync(); // Accept method can infer to var // Flow 1
    Console.WriteLine("Client connected.");                    // listener accepts the client trying to connect in the same (add, port)
    await using NetworkStream stream = handler.GetStream(); // handler and client now in the same connection

    var message = $"DateTime: {DateTime.Now}";
    var dateTimeBytes = Encoding.UTF8.GetBytes(message);

    //stream.Write(dateTimeBytes);
    await stream.WriteAsync(dateTimeBytes);
    Console.WriteLine($"Sent message: {message}"); // after sending, client will read it

    await clientTask;
}
finally 
{
    listener.Stop();
}

// HTTP code ====================================================================================================================
