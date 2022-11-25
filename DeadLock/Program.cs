//var object1 = new object();
//var object2 = new object();

//void FunkcjaPodstawowa()
//{
//    lock (object1)
//    {
//        Console.WriteLine("Funkcja podstawowa pierwszy lock");
//        Thread.Sleep(1000);
//        lock (object2)
//        {
//            Console.WriteLine("Funkcja podstawowa drugi lock");
//        }
//    }
//}

//void FunkcjaDrugorzedna()
//{
//    lock (object2)
//    {
//        Console.WriteLine("Funkcja drugorzedna pierwszy lock");
//        Thread.Sleep(1000);
//        lock (object1)
//        {
//            Console.WriteLine("Funkcja drugorzedna drugi lock");
//        }
//    }
//}

//var thread1 = new Thread(FunkcjaPodstawowa);
//var thread2 = new Thread(FunkcjaDrugorzedna);

//thread1.Start();
//thread2.Start();

//while (true)
//{

//}

var object1 = new object();
var object2 = new object();
var task1 = Task.Run(() =>
{
    lock (object1)
    {
        Console.WriteLine("Funcja pierwsza pierwszy lock");
        Thread.Sleep(1000);
        lock (object2)
        {
            Console.WriteLine("Funkcja druga drugi lock");
        }
    }
});

var task2 = Task.Run(() => { 
    lock (object2)
    {
        Console.WriteLine("Funkcja druga pierwszy lock");
        Thread.Sleep(1000);
        lock (object1)
        {
            Console.WriteLine("Funckja druga drugi lock");
        }
    }
});

task1.Start();
task2.Start();

Task.WaitAll(new[] { task1, task2 });