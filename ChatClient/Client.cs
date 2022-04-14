using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatClient
{
    internal class Client : IDisposable
    {
        private string? userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        private TcpClient client;
        private NetworkStream stream;

        public Client(string userName)
        {
            this.userName = userName;
            client = new TcpClient();
        }

        public void Start()
        {
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();
                string message = userName;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                Task.Run(() => ReceiveMessage());
                Console.WriteLine($"Добро подаловать, {userName}");
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }

        private void SendMessage()
        {
            Console.WriteLine("Введите сообщение:");

            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[64];
                    string message = string.Empty;
                    int bytes = 0;

                    do
                    {
                        bytes = stream.Read(buffer, 0, buffer.Length);
                        message += Encoding.UTF8.GetString(buffer);
                    }
                    while (stream.DataAvailable);

                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Соединение разорванно");
                    Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
            }

            if (client != null)
            {
                client.Close();
            }

            Console.WriteLine("Клиент завершил работу!");
            Environment.Exit(0);
        }
    }
}
