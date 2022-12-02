//var random = new Random();
//var breakIndex = random.Next(1,11);

//var result = Parallel.For(0, 100, (i, state) =>
//{
//    Console.WriteLine($"Rozpoczęcie iteracji numer: {i}");
//    var delay = 0;
//    lock (random)
//    {
//        delay = random.Next(1, 1000);
//    }

//    Thread.Sleep(delay);

//    if(i == breakIndex)
//    {
//        Console.WriteLine($"Break in iteration {i}");
//        state.Break();
//    }

//    Console.WriteLine($"Kończymy iteracje numer: {i}");
//});

//using System.Collections.Concurrent;
//using System.Diagnostics;

//bool IsPrime(int number)
//{
//    if (number < 2)
//    {
//        return false;
//    }

//    for (var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
//    {
//        if (number % divisor == 0)
//        {
//            return false;
//        }
//    }

//    return true;
//}

//IList<int> GetPrimeList(IList<int> numbers) => numbers.Where(IsPrime).ToList();
//IList<int> GetPrileListWithParallel(IList<int> numbers)
//{
//    var primeNumbers = new ConcurrentBag<int>();

//    Parallel.ForEach(numbers, number =>
//    {
//        if (IsPrime(number))
//        {
//            primeNumbers.Add(number);
//        }
//    });

//    return primeNumbers.ToList();
//}

//const int limit = 2_000_000;
//var numbers = Enumerable.Range(0, limit).ToList();

//var watch = Stopwatch.StartNew();
//var linearPrime = GetPrimeList(numbers);
//watch.Stop();

//var watchForParallel = Stopwatch.StartNew();
//var paralllelPrime = GetPrileListWithParallel(numbers);
//watchForParallel.Stop();

//Console.WriteLine($"Linear time: {watch.Elapsed}");
//Console.WriteLine($"Parallel time: {watchForParallel.Elapsed}");


//using System.Collections.Concurrent;
//using System.Diagnostics;

//bool IsPrime(int number)
//{
//    if(number < 2)
//    {
//        return false;
//    }

//    for(var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
//    {
//        if (number % divisor == 0)
//        {
//            return false;
//        }
//    }

//    return true;
//}

//IList<int> GetPrimeList(IList<int> numbers) => numbers.Where(IsPrime).ToList();
//async Task<IList<int>> GetPrileListWithParallel(IList<int> numbers)
//{
//    var primeNumbers = new ConcurrentBag<int>();

//    await Parallel.ForEachAsync(numbers, async (number, token) =>
//    {
//        await Task.Delay(10);
//        if (IsPrime(number))
//        {
//            primeNumbers.Add(number);
//        }
//    });

//    return primeNumbers.ToList();
//}

//const int limit = 2_000_000;
//var numbers = Enumerable.Range(0, limit).ToList();

//var watch = Stopwatch.StartNew();
//var linearPrime = GetPrimeList(numbers);
//watch.Stop();

//var watchForParallel = Stopwatch.StartNew();
//var paralllelPrime = await GetPrileListWithParallel(numbers);
//watchForParallel.Stop();

//Console.WriteLine($"Linear time: {watch.Elapsed}");
//Console.WriteLine($"Parallel time: {watchForParallel.Elapsed}");

void BasicAction()
{
    Console.WriteLine($"Metoda alfa zostałauruchomiona w wątku numer: {Thread.CurrentThread.ManagedThreadId}"); ;
}

Parallel.Invoke(
    BasicAction,
    () =>
    {
        Console.WriteLine($"Metoda beta zostałauruchomiona w wątku numer: {Thread.CurrentThread.ManagedThreadId}"); ;
    },
    delegate ()
    {
        Console.WriteLine($"Metoda gamma zostałauruchomiona w wątku numer: {Thread.CurrentThread.ManagedThreadId}"); ;
    });