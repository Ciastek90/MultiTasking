using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace APMBlockingByEndMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAsyncResult result = Dns.BeginGetHostEntry("www.wp.pl", null, null);
            Console.WriteLine("Przetwarzanie twojego rządania...");

            try
            {
                IPHostEntry host = Dns.EndGetHostEntry(result);
                
                string[] aliases = host.Aliases;
                if (aliases.Any())
                {
                    foreach (var alias in aliases)
                    {
                        Console.WriteLine($"alias: {alias}");
                    }
                }

                IPAddress[] addresses = host.AddressList;
                if (addresses.Any())
                {
                    foreach(IPAddress address in addresses)
                    {
                        Console.WriteLine($"address: {address}");
                    }
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Wyjątek miał miejsce podczas przetwarzania twojego rządania: {ex.Message}");
            }
        }
    }
}
