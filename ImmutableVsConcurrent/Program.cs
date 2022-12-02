using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;

const int items = 10000;
//var linear = new ConcurrentStack<int>();

//for (int i = 0; i < items; i++)
//{
//    linear.Push(i);
//}

//if(linear.TryPeek(out var result))
//{
//    Console.WriteLine($"Wynikiem podejrzenia danych jest {result}");
//}

// Interlocked
var stack = new ConcurrentStack<int>();
var tasks = new List<Task>();
var obj = new object();
var counter = 0;
for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Run(() =>
    {
        for (int j = 0; j < items; j++)
        {
            stack.Push(j + counter * items);
        }
        Interlocked.Increment(ref counter);
    }));
}

Task.WaitAll(tasks.ToArray());

var readerTask = Task.Run(() =>
{
    var result = 0;
    while (stack.Any())
    {
        if (stack.TryPop(out result))
        {
            Console.WriteLine(result);
        }
    }
});

Task.WaitAll(readerTask);

//var stack = ImmutableStack<int>.Empty;
//var tasks = new List<Task>();
//var counter = 0;

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        for (int j = 0; j < items; j++)
//        {
//            stack = stack.Push(j + counter * items);
//        }

//        Interlocked.Increment(ref counter);
//    }));
//}

//Task.WaitAll(tasks.ToArray());

//var readTask = Task.Run(() =>
//{
//    var value = 0;
//    var taskStack = stack;
//    while (taskStack.Any())
//    {
//        taskStack = taskStack.Pop(out value);
//        Console.WriteLine($"zdjęta wartość ze stosu: {value}");
//    }
//});

//Task.WaitAll(readTask);
//Console.ReadLine();

//var concurrencyStack = new ConcurrentStack<int>();
//var tasks = new List<Task>();
//var counter = 0;
//var random = new Random();
//const int packSize = 1000;
//const int fileNumber = 100_000;
//const int taskCount = fileNumber / packSize;
//var obj = new object();

//void CreateFiles(Random random, int start, int end)
//{
//    for(int i = start; i < end; i++)
//    {
//        using var stream = new FileStream($"./test-purpose/{i}.txt", FileMode.OpenOrCreate, FileAccess.Write);
//        using (var writer = new StreamWriter(stream))
//        {
//            writer.Write(random.Next(0, int.MaxValue).ToString());
//        }
//    }
//}

//for (int i = 0; i < taskCount; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        lock (random)
//        {
//            CreateFiles(random, counter * packSize, (counter + 1) * packSize);
//            counter++;
//        }
//    }));
//}

//Task.WaitAll(tasks.ToArray());

//var concurrencyStack = new ConcurrentStack<int>();
//var tasks = new List<Task>();
//var counter = 0;
//const int packSize = 10000;
//const int fileNumber = 100_000;
//const int taskCount = fileNumber / packSize;
//var obj = new object();

//void PushConcurrency(int start, int end)
//{
//    for (int i = start; i < end; i++)
//    {
//        using var stream = new FileStream($"./test-purpose/{i}.txt", FileMode.Open, FileAccess.Read);
//        using (var reader = new StreamReader(stream))
//        {
//            concurrencyStack.Push(int.Parse(reader.ReadToEnd()));
//        }
//    }
//}
//tasks.Add(Task.Run(() =>
//    {
//        PushConcurrency(0, fileNumber);
//    }));
//tasks.Add(Task.Run(() =>
//{
//    PushConcurrency(0, fileNumber);
//}));
//Task.WaitAll(tasks.ToArray());

//var stopWatch = new Stopwatch();
//stopWatch.Start();
//var taskRead = Task.Run(() =>
//{
//    while (!concurrencyStack.IsEmpty)
//    {
//        var value = 0;
//        concurrencyStack.TryPop(out value);
//    }
//});
//Task.WaitAll(taskRead);
//stopWatch.Stop();


//var immutableStack = ImmutableStack<int>.Empty;
//tasks.Clear();
//counter = 0;

//void PushImmutable(int start, int end)
//{
//    for(int i = start; i < end; i++)
//    {
//        using var stream = new FileStream($"./test-purpose/{i}.txt", FileMode.Open, FileAccess.Read);
//        using(var reader = new StreamReader(stream))
//        {
//            if(immutableStack == null)
//            {
//                return;
//            }

//            immutableStack = immutableStack.Push(int.Parse(reader.ReadToEnd()));
//        }
//    }
//}


//tasks.Add(Task.Run(() =>
//{
//    PushImmutable(0, fileNumber);
//}));
//tasks.Add(Task.Run(() =>
//{
//    PushImmutable(0, fileNumber);
//}));
//Task.WaitAll(tasks.ToArray());


//var stopWatch1 = new Stopwatch();
//stopWatch1.Start();
//var readTask = Task.Run(() => {
//    var value = 0;
//    var stack = immutableStack;
//    while (!stack.IsEmpty)
//    {
//        stack = stack.Pop(out value);
//    }
//});
//Task.WaitAll(readTask);

//stopWatch1.Stop();

//Console.WriteLine($"Concurrency time {stopWatch.Elapsed}");
//Console.WriteLine($"Immutable time {stopWatch1.Elapsed}");