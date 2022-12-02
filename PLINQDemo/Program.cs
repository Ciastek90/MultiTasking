var source = Enumerable.Range(1, 2_000);

var parallelQuery = 
    from number in source.AsParallel()
    where number % 10 == 0
    select number;

parallelQuery.ForAll((e) =>
{
    Console.WriteLine(e);
});

foreach(var e in parallelQuery)
{
    Console.WriteLine(e);
}

var parallelQuery2 = (from number in source.AsParallel()
                      where number % 10 == 0
                      select number).ToArray();

var parallelQuery3 =
    source.AsParallel()
    .Where(x => x % 10 == 0)
    .Select(x => x);
        