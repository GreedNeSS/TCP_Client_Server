using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    internal class ServerSocket: IDisposable
    {
        private TcpListener tcpListener = null!;
        private List<ClientSocket> clients = new List<ClientSocket>();
        private int port = 8888;

        public ServerSocket()
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void Listen()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключения...");

                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    ClientSocket clientSocket = new ClientSocket(client, this);
                    Task.Run(() => clientSocket.Process());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        protected internal void AddConnection(ClientSocket client)
        {
            clients.Add(client);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientSocket clientSocket = clients.FirstOrDefault(c => c.Id == id);

            if (clientSocket != null)
            {
                clients.Remove(clientSocket);
            }
        }

        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);

            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    client.Stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        protected internal void Disconnect()
        {
            if (tcpListener != null)
            {
                tcpListener.Stop();
            }

            foreach (var client in clients)
            {
                client.Close();
            }

            Console.WriteLine("Сервер остановлен!");
            Environment.Exit(0);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
