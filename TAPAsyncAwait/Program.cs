/*
 * Napisz metodę asynchroniczną która pobierze dane z bazy danych
 */

static Task<int> MethodTask(int delay)
{
    Thread.Sleep(delay);

    return Task.FromResult(delay);
}

static async Task<int> MethodAsync(int delay)
{
    return await MethodTask(delay);
}

await MethodAsync(3000);