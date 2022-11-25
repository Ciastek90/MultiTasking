//void WriteAsync(string text, int i)
//{
//    using var stream = new FileStream("incredible.txt", FileMode.OpenOrCreate, FileAccess.Write);
//    using (var writer = new StreamWriter(stream))
//    {
//        Console.WriteLine($"iteration {i}");
//        Console.WriteLine($"task number {Task.CurrentId}");
//        var task = writer.WriteLineAsync(text);
//        task.Wait();
//    }
//}

//for (int i = 0; i < 10000; i++)
//{
//    WriteAsync(new string('+', 1000), i);
//}

// Na bazie tego programu napisz taki który wpisuje liczby do pliku używająć wielu wątków
// Oraz stwórz wiele wątków które będą z tego pliku czytać dane i wyświetlać je na ekranie

//async Task WriteAsync(string text, int i)
//{
//    using var stream = new FileStream("incredible5.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
//    using (var writer = new StreamWriter(stream)) 
//    {
//        await writer.WriteLineAsync(i.ToString());
//    }
//}

//var tasks = new List<Task>();
//for(int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run (() =>
//    {
//        using var stream = new FileStream("incredible5.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
//        using (var reader = new StreamReader(stream))
//        {
//            Console.WriteLine($"Task number: {Task.CurrentId}");
//            Console.WriteLine($"data from file: {reader.ReadLine()}");
//        }
//    }));
//}

//for (int i = 0; i < 10000; i++)
//{
//    await WriteAsync(new string('*', 1000), i);
//}

//Task.WaitAll(tasks.ToArray());

//3
//var fileLock = new ReaderWriterLockSlim();
//var stream = new FileStream("data.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
//var writer = new StreamWriter(stream);
//var tasks = new List<Task>();

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        fileLock.EnterWriteLock();
//        try
//        {
//            writer.WriteLine(new string('-', 1000) + Task.CurrentId);
//        }
//        finally
//        {
//            fileLock.ExitWriteLock();
//        }
//    }));
//}

//Task.WaitAll(tasks.ToArray());

//writer.Dispose();
//stream.Dispose();

//// 2
//var stream = new FileStream("data.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
//var writer = new StreamWriter(stream);
//var random = new Random();
//var tasks = new List<Task>();

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        lock (random)
//        {
//            writer.WriteLine(random.Next(1, 200).ToString());
//        }
//    }));
//}

//Task.WaitAll(tasks.ToArray());

//writer.Dispose();
//stream.Dispose();

//// 1
var stream = new FileStream("data.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
var wirter = new StreamWriter(stream);
var tasks = new List<Task>();
var @object = new object();

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Run(() =>
    {
        Monitor.Enter(@object);
        try
        {
            wirter.WriteLine(Task.CurrentId + new string('0', 10000));
        }
        finally
        {
            Monitor.Exit(@object);
        }
    }));
}

wirter.Dispose();
stream.Dispose();

//1. Napisz program który korzysta z mechanizmu monitora do zapisania danych do pliku przez różne zadania
//2. Napisz program który zapisze losowe liczby przez różne zadania do pliku i użyj słowa lock
//3. Napisz program który zapisze dane do pliku korzystając z ReaderWriterLockSlim
