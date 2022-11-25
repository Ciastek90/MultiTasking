using ReaderAndWriterLockSlim;

var syncCache = new SyncCache();
var tasks = new List<Task>();
var itemsCount = 0;

tasks.Add(Task.Run(() =>
{
    var vegetables = new string[] {
        "broccoli",
        "cauliflower",
        "carrot",
        "cabbage",
        "corn",
        "spinach"
    };

    for (int i = 0; i < vegetables.Length; i++)
    {
        syncCache.Add(i, vegetables[i]);
    }

    itemsCount = vegetables.Length;

    Console.WriteLine($"Zadanie {Task.CurrentId} zapisalo {vegetables.Length} warzyw\n");
}));

tasks.Add(Task.Run(() =>
{
    for(int i = 0; i < syncCache.Count; i++)
    {
        Console.WriteLine(syncCache.Read(i));
    }
}));

tasks.Add(Task.Run(() => 
{
    for(int i = 0; i < syncCache.Count; i++)
    {
        var value = syncCache.Read(i);
        if(value == "cabbage")
        {
            if(syncCache.AddOrUpdate(i, "green bean") != AddOrUpdateStatus.Unchanged)
            {
                Console.WriteLine("Zmieniliśmy kapuste na zieloną fasolę");
            }
        }
    }
}));

Task.WaitAll(tasks.ToArray());
for(int i = 0; i < syncCache.Count; i++)
{
    Console.WriteLine($"{i}: {syncCache.Read(i)}");
}