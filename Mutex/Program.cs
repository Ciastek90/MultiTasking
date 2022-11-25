//var mutex = new Mutex();
//const int iterationsCount = 1;
//const int threadCount = 10;

//void ThreadProc()
//{
//    for(int i = 0; i < iterationsCount; i++)
//    {
//        UseResource();
//    }
//}

//void UseResource()
//{
//    Console.WriteLine($"Wątek o nazwie {Thread.CurrentThread.Name} prosi o dostęp do mutex'a");

//    mutex.WaitOne();

//    Console.WriteLine($"Wątek o nazwie {Thread.CurrentThread.Name} wszedł do sekcji chronionej");

//    Thread.Sleep(1000);

//    Console.WriteLine($"Wątek o nazwie {Thread.CurrentThread.Name} opuszcza sekcję chronioną");
//    mutex.ReleaseMutex();

//    Console.WriteLine($"Wątek o nazwie {Thread.CurrentThread.Name} zwolnił mutex");
//}

//Thread.Sleep(1000);

//for(int i = 0; i < threadCount; i++)
//{
//    var thread = new Thread(new ThreadStart(ThreadProc));
//    thread.Name = $"Thread[{i+1}]";
//    thread.Start();
//}

//Console.ReadLine();

var mutext = new Mutex();
const int iterationCount = 1;
const int threadCount = 10;
var tasks = new List<Task>();

for (int i = 0; i < threadCount; i++)
{
    tasks.Add(Task.Run(() =>
    {
        for(int i = 0; i < iterationCount; i++)
        {
            Console.WriteLine($"Wątek o numerze {Task.CurrentId} prosi o dostęp do mutex'a");
            mutext.WaitOne();

            Console.WriteLine($"Wątek o numerze {Task.CurrentId} wszedł do sekcji chronionej");
            //praca
            Thread.Sleep(1000);

            Console.WriteLine($"Wątek o numerze {Task.CurrentId} opuszcza sekcję chronioną");
            mutext.ReleaseMutex();
            Console.WriteLine($"Wątek o numerze {Task.CurrentId} zwolnił mutex");
        }
    }));
}

Thread.Sleep(1000);

Task.WaitAll(tasks.ToArray());

Console.ReadLine();