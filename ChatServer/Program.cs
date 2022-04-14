using ChatServer;

Console.WriteLine("***** Chat Server *****");

using (ServerSocket server = new ServerSocket())
{
    try
    {
        Task.Run(() => server.Listen());
        Console.WriteLine("Для остановки сервера нажмите Enter");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}