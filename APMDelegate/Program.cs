using System;
using System.Threading;
using static APMDelegate.AsyncDelegate;

namespace APMDelegate
{
    /* Napisz program który uruchomi 4 różne delegaty na różnych wątkach jako bazy użyj aktualnego programu */

    public class AsyncDelegate
    {
        public string Run(int callDuration, out int threadId)
        {
            Console.WriteLine("Wystartowano metodę");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return $"Mój czas wywołania to: {callDuration}";
        }

        public delegate string AsyncMethodCaller(int callDuration, out int threadId);
    }

    internal class Program
    {
        private static int threadId;

        static void Main(string[] args)
        {
            for (int i = 0; i < 4; i++)
            {
                var asyncDelegate = new AsyncDelegate();

                var caller = new AsyncMethodCaller(asyncDelegate.Run);

                var result = caller.BeginInvoke(3000, out threadId, null, null);

                Console.WriteLine($"Główny wątek wykonuje jakąś pracę na wątku {Thread.CurrentThread.ManagedThreadId}");

                var returnValue = caller.EndInvoke(out threadId, result);

                Console.WriteLine($"Metoda została wywołana na wątku: {threadId}, i zwróciła wartość {returnValue}");
            }

            Console.ReadLine();
        }
    }
}
