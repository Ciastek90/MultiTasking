using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace APMWithCallback
{
    /*
     * Stworz na bazie tego progamu nową aplikację konsolową która czyta pliki w danym folderze i wyśwetli ich zawartość
     * na ekranie.
     */


    internal class Program
    {
        static int requestCounter;
        static ArrayList hostData = new ArrayList();
        static ArrayList fileData = new ArrayList();
        static StringCollection hostNames = new StringCollection();
        static StringCollection paths = new StringCollection();

        class ReadDataModel
        {
            public string Path { get; set; }

            public FileStream Stream { get; set; }

            public byte[] Data { get; set; }
        }

        static void Main(string[] args)
        {
            AsyncCallback processCallback = new AsyncCallback(ProcessInformation);
            AsyncCallback readDataCallback = new AsyncCallback(ReadData);
            string host;
            do
            {
                Console.WriteLine("Wprowadź nazwę lub adres strony aby pobrać dane: ");
                host = Console.ReadLine();

                if(host.Length > 0)
                {
                    Interlocked.Increment(ref requestCounter);
                    var files = Directory.GetFiles(host, "*.cs");
                    foreach (var file in files)
                    {
                        var fs = new FileStream(file, FileMode.Open);
                        var buffer = new byte[4096];
                        fs.BeginRead(buffer, 0, 4096, readDataCallback, new ReadDataModel { Path = file, Stream = fs, Data = buffer });
                    }
                    Dns.BeginGetHostEntry(host, processCallback, host);
                }

                while (requestCounter > 0)
                {
                    Thread.Sleep(1000);
                    UpdateUserInterface();
                }

            } while(host.Length > 0);

            foreach(var data in fileData)
            {
                Console.WriteLine(System.Text.Encoding.Default.GetString((byte[])data));
            }

            //for(int i = 0; i < hostNames.Count; i++)
            //{
            //    var data = hostData[i];
            //    var message = data as string;
            //    if(message != null)
            //    {
            //        Console.WriteLine($"Rządanie dla {hostNames[i]} zwróciło wiadomość: {message}");
            //        continue;
            //    }

            //    IPHostEntry hostEntry = (IPHostEntry)data;
            //    var aliases = hostEntry.Aliases;
            //    if (aliases.Any())
            //    {
            //        Console.WriteLine($"Aliasy dla {hostNames[i]}");
            //        foreach(var alias in aliases)
            //        {
            //            Console.WriteLine($"alias: {alias}");
            //        }
            //    }

            //    IPAddress[] addresses = hostEntry.AddressList;
            //    if (addresses.Any())
            //    {
            //        Console.WriteLine($"Adresy dla {hostNames[i]}");
            //        foreach(IPAddress address in addresses)
            //        {
            //            Console.WriteLine($"adres: {address}");
            //        }
            //    }
            //}

            Console.ReadLine();
        }

        private static void ReadData(IAsyncResult asyncResult)
        {
            var data = (ReadDataModel)asyncResult.AsyncState;
            paths.Add(data.Path);

            data.Stream.EndRead(asyncResult);

            fileData.Add(data.Data);

            //Interlocked.Increment(ref requestCounter);
        }

        private static void UpdateUserInterface()
        {
            Console.WriteLine($"{requestCounter} pozostałe rządania.");
        }

        private static void ProcessInformation(IAsyncResult asyncResult)
        {
            var hostName = (string)asyncResult.AsyncState;
            hostNames.Add(hostName);
            try
            {
                IPHostEntry host = Dns.EndGetHostEntry(asyncResult);
                Thread.Sleep(3000);
                hostData.Add(host);
            }
            catch(SocketException ex)
            {
                hostNames.Add(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }
    }
}
