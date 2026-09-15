using System.Net;
using System.Text;

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
var hostName = Dns.GetHostName();
IPHostEntry ipHost = await Dns.GetHostEntryAsync("x.com");
IPAddress[] ipAdd = ipHost.AddressList;
IPAddress ipAddress = ipHost.AddressList[0];

//Console.WriteLine(ipAdd);

Console.WriteLine($"Hostname: {ipHost.HostName}");
Console.WriteLine($"Addresses found: {ipHost.AddressList.Length}");

foreach (var adds in ipAdd)
{
    Console.WriteLine(adds);
}

IPEndPoint endPoint = new(ipAddress, 11_000);