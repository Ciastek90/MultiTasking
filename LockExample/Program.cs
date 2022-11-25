var tasks = new List<Task>();
var text = string.Empty;
var lockObj = new object();

// napisz program na bazie istniejącego który konkatenuje ciągi znaków
// a konkatenacja ma wyjątać następująco:
// "1", "1,2", "1,2,3", "1,2,3,4"
// 

async Task<string> Concat(int i)
{
    lock (lockObj)
    {
        for (int j = 0; j < 10; j++)
        {
           text += $"{i * 10 + j},";
        }

        Console.WriteLine(text);

        return text;
    }
}

for (int i = 0; i < 10; i++)
{
    await Concat(i);
}

Task.WaitAll(tasks.ToArray());