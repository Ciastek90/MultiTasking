// Przykład 1 inkrementacja zmiennej
//var tasks = new List<Task>();
//var spinLock = new SpinLock();
//var counter = 0;

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        var lockTaken = false;

//        try
//        {
//            spinLock.Enter(ref lockTaken);
//            for (int i = 0; i < 10; i++)
//            {
//                counter++;
//            }
//        }
//        finally
//        {
//            if (lockTaken)
//            {
//                spinLock.Exit();
//            }
//        }
//    }));
//}

//Task.WaitAll(tasks.ToArray());
//Console.WriteLine($"Wartość licznika wynosi: {counter}");

//using System.Diagnostics;
//using System.Text;

//var builder = new StringBuilder();
//var tasks = new List<Task>();
//var spinLock = new SpinLock();

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        var lockTaken = false;
//        for (int j = 0; j < 10000; j++)
//        {
//            lockTaken = false;
//            try
//            {
//                spinLock.Enter(ref lockTaken);
//                builder.Append((j % 10).ToString());
//            }
//            finally
//            {
//                if (lockTaken)
//                {
//                    spinLock.Exit();
//                }
//            }
//        }
//    }));
//}

//Task.WaitAll(tasks.ToArray());
//Console.WriteLine($"ilość elementów to: {builder.Length}");
//Console.WriteLine($"ilość liczb podzielnych przez 7 to " +
//    $"{builder.ToString().Where(x => x == '7').Count()}");

// Napisz program który zwiększa zmienną przy użyciu spin locka oraz przy użyciu słówka kluczowego
// lock. Chcemy aby zostało stworzone 10000 zadań które będą inkrementować zmienną. Zmierz
// czasy obu operacji. Do zmierzenia czasu użyj klasy StopWatch

using System.Diagnostics;

var tasks = new List<Task>();
var stopwatch = new Stopwatch();
const int N = 1_000_000;
var counter = 0;
var obj = new object();
stopwatch.Start();
for (int i = 0; i < N; i++)
{
    tasks.Add(Task.Run(() =>
    {
        lock (obj)
        {
            counter++;
        }
    }));
}

Task.WaitAll(tasks.ToArray());
stopwatch.Stop();
tasks.Clear();
Console.WriteLine($"lock {stopwatch.Elapsed}");
// część nr 2
var spinLock = new SpinLock();
counter = 0;
var stopwatch1 = new Stopwatch();
stopwatch1.Start();
for (int i = 0; i < N; i++)
{
    tasks.Add(Task.Run(() =>
    {
        var lockTaken = false;
        try
        {
            spinLock.Enter(ref lockTaken);
            counter++;
        }
        finally
        {
            if (lockTaken)
            {
                spinLock.Exit();
            }
        }
    }));
}
Task.WaitAll(tasks.ToArray());
stopwatch1.Stop();
Console.WriteLine($"spinlock {stopwatch1.Elapsed}");

Console.ReadLine();