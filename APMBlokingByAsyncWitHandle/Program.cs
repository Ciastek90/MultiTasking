using System;
using System.Net;
using System.Net.Sockets;

namespace APMBlokingByAsyncWitHandle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAsyncResult result = Dns.BeginGetHostEntry("www.google.pl", null, null);

            Console.WriteLine("Rozpoczęcie pobierania informacji...");

            result.AsyncWaitHandle.WaitOne();

            try
            {
                IPHostEntry host = Dns.EndGetHostEntry(result);

                string[] aliases = host.Aliases;
                if (aliases.Length > 0)
                {
                    Console.WriteLine("Aliases");
                    foreach (string alias in aliases)
                    {
                        Console.WriteLine($"alias: {alias}");
                    }
                }

                IPAddress[] addresses = host.AddressList;
                if (addresses.Length > 0)
                {
                    Console.WriteLine("Adresses");
                    foreach (IPAddress address in addresses)
                    {
                        Console.WriteLine($"address: {address.ToString()}");
                    }
                }

                Console.ReadLine();
            }
            catch(SocketException ex)
            {
                Console.WriteLine($"Został wyrzucony wyjątek podczas przetwarzania rządania: {ex.Message}");
            }
        }
    }
}
