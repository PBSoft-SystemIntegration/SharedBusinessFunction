using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;

TcpListener server = new TcpListener(IPAddress.Any, 8080);
server.Start();
while (true)
{
    TcpClient client = server.AcceptTcpClient();
    NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[1024];
    int bytesRead = stream.Read(buffer, 0, buffer.Length);
    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    var message = JsonConvert.DeserializeObject<dynamic>(receivedData);
    Console.WriteLine($" godt : Number1={message.Number1}, " +
      $"Number2={message.Number2} from client");

    var responseData = new {Result = message.Number1 + message.Number2 };
    string responseString = JsonConvert.SerializeObject(responseData); 
    byte[] resposeBytes = Encoding.UTF8.GetBytes(responseString);

    stream.Write(resposeBytes, 0, resposeBytes.Length);
    stream.Close();
    client.Close();
}
