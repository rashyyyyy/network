using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static bool runAgain = true;

    static void Main(string[] args)
    {
        while (runAgain)
        {
            try
            {
                Console.WriteLine("Enter the target IP address:");
                string ipAddressString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(ipAddressString))
                {
                    Console.WriteLine("No IP address entered. Please try again.");
                    continue;
                }

                if (!IPAddress.TryParse(ipAddressString, out IPAddress ipAddress))
                {
                    Console.WriteLine("Invalid IP address format. Please try again.");
                    continue;
                }

                Console.WriteLine($"Starting port scan for IP address: {ipAddress}\n");

                Parallel.For(1, 65536, async port =>
                {
                    await ScanPort(ipAddress, port);
                });

                Console.WriteLine("\nPort scan complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPress Ctrl+C to restart the scan with a different IP address or any other key to exit...");
                if (Console.ReadKey(true).Key != ConsoleKey.C)
                {
                    runAgain = false;
                }
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static async Task ScanPort(IPAddress ipAddress, int port)
    {
        using (TcpClient tcpClient = new TcpClient())
        {
            try
            {
                var connectTask = tcpClient.ConnectAsync(ipAddress, port);
                if (await Task.WhenAny(connectTask, Task.Delay(500)) == connectTask)
                {
                    if (tcpClient.Connected)
                    {
                        Console.WriteLine($"Port {port} is open");
                        IdentifyService(tcpClient);
                    }
                }
            }
            catch
            {
                // Handle exceptions as needed
            }
        }
    }

    static void IdentifyService(TcpClient tcpClient)
    {
        try
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string response = Encoding.ASCII.GetString(data, 0, bytesRead);

            Console.WriteLine($"Service on port: {response.Trim()}");
        }
        catch
        {
            // Handle exceptions as needed
        }
    }
}
