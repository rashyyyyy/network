using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

                // Validate the IP address
                IPAddress ipAddress;
                if (!IPAddress.TryParse(ipAddressString, out ipAddress))
                {
                    Console.WriteLine("Invalid IP address format. Please try again.");
                    continue;
                }

                Console.WriteLine("Starting port scan for IP address: " + ipAddress + "\n");

                // Iterate through ports 1 to 65535
                for (int port = 1; port <= 65535; port++)
                {
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        try
                        {
                            // Set the connect timeout to 500 milliseconds
                            tcpClient.SendTimeout = 500;
                            tcpClient.ReceiveTimeout = 500;

                            // Attempt to connect to the IP address and port
                            tcpClient.Connect(ipAddress, port);

                            // If connection successful, port is open
                            Console.WriteLine($"Port {port} is open");

                            // Attempt to identify the service
                            IdentifyService(tcpClient);
                        }
                        catch (Exception)
                        {
                            // If connection fails, port is closed
                            // Console.WriteLine($"Port {port} is closed");
                        }
                    }
                }

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

    static void IdentifyService(TcpClient tcpClient)
    {
        try
        {
            // Read response from the port
            NetworkStream stream = tcpClient.GetStream();
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string response = Encoding.ASCII.GetString(data, 0, bytesRead);

            // Print the response
            Console.WriteLine($"Service on port: {response.Trim()}");
        }
        catch (Exception)
        {
            // Failed to identify service
            // Console.WriteLine("Unable to identify service.");
        }
    }
}
// you will need to run in visual studio if you want to build it for yourself, let me know if you have any issues 
