//Semaphore semaphore;

//int suma = 0;

//void Worker(object number)
//{
//    Console.WriteLine($"Wątek {number} startuje i oczekuje na sygnał od semafora");
//    semaphore.WaitOne();

//    var partialSum = Interlocked.Add(ref suma, 100);

//    Thread.Sleep(1000 + partialSum);

//    Console.WriteLine($"Wątek {number} zwalnia semafor.");
//    Console.WriteLine($"Wątek {number} poprzednia ilość elementów w semaforze to: {semaphore.Release()}");
//}

//semaphore = new Semaphore(0, 5);

//for(int i=0; i<10; i++)
//{
//    var thread = new Thread(new ParameterizedThreadStart(Worker));

//    thread.Start();
//}

//Thread.Sleep(500);

//Console.WriteLine("Główny wątek wywołuje funkcje Release(3)");
//semaphore.Release(releaseCount: 5);
//Console.WriteLine("Główny wiątek nadal istnieje.");

//var semaphore = new SemaphoreSlim(0, 3);
//var sum = 0;
//Console.WriteLine($"Ilość zadań któe mają dostęp do semafora to: {semaphore.CurrentCount}");
//var tasks = new List<Task>();

//for (int i = 0; i < 10; i++)
//{
//    tasks.Add(Task.Run(() =>
//    {
//        Console.WriteLine($"Zadanie {Task.CurrentId} rozpoczyna pracę i oczekuje na sygnał od semafora");
//        var semaphoreCount = 0;
//        semaphore.Wait();
//        try
//        {
//            var partialSum = Interlocked.Add(ref sum, 100);

//            Console.WriteLine($"Suma częściowa {partialSum}");
//            Console.WriteLine($"Suma {sum}");
//            Console.WriteLine($"Zadanie {Task.CurrentId} rozpoczyna pracę wramach semafora");
//            Thread.Sleep(1000 + partialSum);
//        }
//        finally
//        {
//            semaphoreCount = semaphore.Release();
//        }

//        Console.WriteLine($"Zadanie {Task.CurrentId} zwalnia semafor, ilość elementów mogących wejść do semofora to: {semaphoreCount}");
//    }));
//}

//Thread.Sleep(500);
//Console.WriteLine("Główny wątek wywołuje zwolnienie 3 miejsc");
//semaphore.Release(3);
//Console.WriteLine($"{semaphore.CurrentCount} zadań może teraz wejść do semafora");

//Task.WaitAll(tasks.ToArray());

//Console.WriteLine("Główny wątek nadal istnieje");

// Stworz program który czyta dane z pliku w wielu wątkach maksymalna ilość czytelników jednocześnie to 4 ilość wszyztkich wątków 20
// użyj do tego celu klasę SlimSemaphore.

var stream = new FileStream("data.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
var streamReader = new StreamReader(stream);
var semaphore = new SemaphoreSlim(4);
var tasks = new List<Task>();

for (int i = 0; i < 20; i++)
{
    tasks.Add(Task.Run(() =>
    {
        semaphore.Wait();
        try
        {
            var text = streamReader.ReadToEnd();
            stream.Position = 0;
            Console.WriteLine(Task.CurrentId + text);
            Thread.Sleep(1000 + 10 * Task.CurrentId ?? 1);
        }
        finally
        {
            semaphore.Release();
        }
    }));
}

Thread.Sleep(500);
semaphore.Release(4);

Task.WaitAll(tasks.ToArray());