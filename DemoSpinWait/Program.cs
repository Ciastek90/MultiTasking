//var allowCounting = false;
//var counter = 0;

//var increment = Task.Factory.StartNew(() =>
//{
//    var spinWait = new SpinWait();
//    while (!allowCounting)
//    {
//        if (spinWait.NextSpinWillYield)
//        {
//            counter++;
//        }

//        spinWait.SpinOnce();
//    }

//    Console.WriteLine($"SpinWait został wywołany {spinWait.Count} razy, ale funkcja w ramch konteksu została uruchomiona {counter}");
//});

//var stop = Task.Factory.StartNew(() =>
//{
//    Thread.Sleep(1000);
//    allowCounting = true;
//});

//Task.WaitAll(increment, stop);

var allowCounting = false;
var counter = 0;
var N = 1000;

var tasks = new List<Task>();

var incrementTask = Task.Factory.StartNew(() =>
{
    var spinWait = new SpinWait();
    while (!allowCounting && counter < N)
    {
        if (spinWait.NextSpinWillYield)
        {
            counter++;
            Console.WriteLine(counter.ToString());
        }

        spinWait.SpinOnce();
    }

    Console.WriteLine("SpinWait called {0} times, yielded {1} times", spinWait.Count, counter);
});

var stopTask = Task.Factory.StartNew(() =>
{
    Thread.Sleep(100);
    allowCounting = true;
    Console.WriteLine("Stop");
});

var returnTask = Task.Factory.StartNew(() =>
{
    Thread.Sleep(100);
    allowCounting = false;
    Console.WriteLine("Return");
});

Task.WaitAll(incrementTask, stopTask, returnTask, stopTask);

Console.WriteLine($"Wartość licznika: {counter}");



// Dodaj zadanie które zatrzyma zliczanie a następnie drugie zadanie które je wznowi
// allowCounting (stop - [true]) (return - [false]) oba zadania muszą mieć opóźnienie Thread.Sleep(100)