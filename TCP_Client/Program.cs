using System.Net.Sockets;
using System.Text;

Console.WriteLine("***** TCP Client *****");

int port = 8888;
string address = "127.0.0.1";

using (TcpClient client = new TcpClient())
{
    try
    {
        client.Connect(address, port);
        byte[] buffer = new byte[256];
        StringBuilder response = new StringBuilder();

        using (NetworkStream stream = client.GetStream())
        {
            do
            {
                int bytes = stream.Read(buffer, 0, buffer.Length);
                response.Append(Encoding.UTF8.GetString(buffer));
            }
            while (stream.DataAvailable);

            Console.WriteLine(response);
        }
    }
    catch (SocketException ex)
    {
        Console.WriteLine("SocketException: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }

    Console.WriteLine("Запрос завершён!");
    Console.ReadLine();
}