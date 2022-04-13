using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("***** TCP Server *****");

int port = 8888;
string address = "127.0.0.1";
TcpListener listener = null;

try
{
    IPAddress localAddress = IPAddress.Parse(address);
    listener = new TcpListener(localAddress, port);
    listener.Start();

    while (true)
    {
        Console.WriteLine("Ожидание подключения!");

        using TcpClient client = listener.AcceptTcpClient();

        Console.WriteLine("Клиент подключен.");

        using (NetworkStream stream = client.GetStream())
        {
            string response = "Привет мир!";
            byte[] buffer = Encoding.UTF8.GetBytes(response);
            stream.Write(buffer, 0, buffer.Length);

            Console.WriteLine($"Отправлено сообщение: {response}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    if (listener != null)
        listener.Stop();
}