using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace APMWaitPoll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //www.youtube.pl
            IAsyncResult result = Dns.BeginGetHostEntry("www.netflix.com", null, null);
            Console.WriteLine("Przetwarzanie twojego rządania...");

            while(!result.IsCompleted)
            {
                UpdateUserInterface();
            }

            Console.WriteLine();
            try
            {
                IPHostEntry host = Dns.EndGetHostEntry(result);
                var aliases = host.Aliases;
                if (aliases.Any())
                {
                    foreach(var alias in aliases)
                    {
                        Console.WriteLine($"alias: {alias}");
                    }
                }

                var addresses = host.AddressList;
                if (addresses.Any())
                {
                    foreach(var address in addresses)
                    {
                        Console.WriteLine($"address: {address}");
                    }
                }

                Console.ReadLine();
            }
            catch(SocketException ex)
            {
                Console.WriteLine($"Podczas przetwarzania rządania wystąpił wyjątek {ex}");
            }
        }

        private static void UpdateUserInterface()
        {
            Console.Write(".");
        }
    }
}
