//var tasks = new List<Task>();
//var counter = 0;
//var obj = new object();

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        Monitor.Enter(obj);
//        for (int i = 0; i < 1000000; i++)
//        {
//            counter++;
//        }

//        Monitor.Exit(obj);
//        Console.WriteLine(counter);
//    }));
//}

//Task.WaitAll(tasks.ToArray());

//var tasks = new List<Task>();
//var r = new Random();
//long total = 0;
//int n = 0;

//for(int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        var values = new int[10000];
//        var taskTotal = 0;
//        var taskNubersCount = 0;
//        var j = 0;
//        Monitor.Enter(r);
//        for(j = 0; j < 10000; j++)
//        {
//            values[j] = r.Next(1,1001);
//        }
//        Monitor.Exit(r);
//        taskNubersCount = j;
//        foreach(var value in values)
//        {
//            taskTotal += value;
//        }

//        Console.WriteLine($"Średnia dla tego wątku {Task.CurrentId,2} to: {taskTotal / taskNubersCount:N2} (N={taskNubersCount:N0})");

//        Interlocked.Add(ref n, taskNubersCount);
//        Interlocked.Add(ref total, taskTotal);
//    }));
//}

//Task.WaitAll(tasks.ToArray());
//Console.WriteLine($"\n Średnia dla wszystkich zadań to: {total/n:N2} (N={n:N0})");

// napisz program który sworzy dwa wątki każdy z wątków będzie wykorzystywał obiekt random do wylosowania wartości
// wątek 1: dodawał dwie losowe liczby
// wątek 2: mnoży dwie losowe liczby
// wynik końcowy ma być to 1/(suma wyników z obu wątków) 1/(wynik t1 + wynik t2)

//var r = new Random();
//var sum = 0;

//var task1 = Task.Run(() =>
//{
//    var value = 0;
//    Monitor.Enter(r);
//    value = r.Next(0, 100) + r.Next(0, 100);
//    Monitor.Exit(r);
//    Console.WriteLine($"Wynik dodawania to: {value}");
//    Interlocked.Add(ref sum, value);
//});

//var task2 = Task.Run(() =>
//{
//    var value = 0;
//    Monitor.Enter(r);
//    value = r.Next(0, 100) * r.Next(0, 100);
//    Monitor.Exit(r);

//    Console.WriteLine($"Wynik mnożenia to: {value}");

//    Interlocked.Add(ref sum, value);
//});

//Task.WaitAll(new[] { task1, task2 });
//Console.WriteLine($"Wynik działania to: {sum}");

// Napisz program na bazie powyższego kodu przy użyciu słowa lock
// Dodaj wątek któy dzieli dwie losowe liczby a ich wynik dodaje do ogólnego wyniku

//var obj = new object();
//lock (obj)
//{
//    // to do something
//}


//try
//{
//    Monitor.Enter(obj);
//    Monitor.
//    // to do something
//}
//finally
//{
//    Monitor.Exit(obj);
//}

var r = new Random();
var sum = 0;

var task1 = Task.Run(() =>
{
    var value = 0;
    lock (r)
    {
        value = r.Next(1, 100) + r.Next(1, 100);
    }
    Console.WriteLine($"Wynik dodawania to: {value}");
    Interlocked.Add(ref sum, value);
});

var task2 = Task.Run(() =>
{
    var value = 0;
    lock (r)
    {
        value = r.Next(1, 100) * r.Next(1, 100);
    }

    Console.WriteLine($"Wynik mnożenia to: {value}");

    Interlocked.Add(ref sum, value);
});

var task3 = Task.Run(() =>
{
    var value = 0;
    lock (r)
    {
        value = (int)Math.Pow(r.Next(1, 100), 2);
    }

    Console.WriteLine($"Wynik potęgowania to: {value}");

    Interlocked.Add(ref sum, value);
});

Task.WaitAll(new[] { task1, task2, task3 });
Console.WriteLine($"Wynik działania to: {sum}");