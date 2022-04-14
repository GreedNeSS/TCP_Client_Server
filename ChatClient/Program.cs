using ChatClient;

Console.WriteLine("***** Chat Client *****");

Console.WriteLine("Введите своё имя:");
string userName = Console.ReadLine();

using (Client client = new Client(userName))
{
    client.Start();
}