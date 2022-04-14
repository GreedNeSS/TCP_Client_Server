using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatServer
{
    internal class ClientSocket
    {
        private string userName = null!;
        private TcpClient client = null!;
        private ServerSocket server = null!;
        protected internal string Id { get; set; } = null!;
        protected internal NetworkStream Stream { get; set; } = null!;

        public ClientSocket(TcpClient tcpClient, ServerSocket serverSocket)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverSocket;
            serverSocket.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string message = GetMessage();
                userName = message;
                message += "вошёл в чат!";
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                    }
                    catch (Exception e)
                    {
                        message = $"{userName}: покинул чвт";
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        private string GetMessage()
        {
            byte[] buffer = new byte[64];
            string message = string.Empty;
            int bytes = 0;

            do
            {
                bytes = Stream.Read(buffer, 0, buffer.Length);
                message += Encoding.UTF8.GetString(buffer, 0, bytes);
            }
            while (Stream.DataAvailable);

            return message;
        }

        protected internal void Close()
        {
            if (Stream != null)
            {
                Stream.Close();
            }

            if (client != null)
            {
                client.Close();
            }
        }
    }
}
