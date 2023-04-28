using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

class RPSClient
{
    static void Main(string[] args)
    {
        try
        {
            string serverAddress = "127.0.0.1";
            int serverPort = 8080;

            TcpClient client = new TcpClient(serverAddress, serverPort);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            Thread readThread = new Thread(() => ReadMessages(reader));
            readThread.Start();

            while (true)
            {
                string input = Console.ReadLine();
                writer.WriteLine(input);

                if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ReadMessages(StreamReader reader)
    {
        try
        {
            while (true)
            {
                string message = reader.ReadLine();
                if (message == null)
                {
                    break;
                }

                Console.WriteLine(message);
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Disconnected from the server.");
        }
    }
}
