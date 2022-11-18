static Task<int> ReadTask(Stream stream, byte[] buffer, int offset, int length, object? state)
{
    var tcs = new TaskCompletionSource<int>();
    stream.BeginRead(buffer, offset, length, asyncOpreation =>
    {
        try
        {
            stream.Position = 0;
            tcs.SetResult(stream.EndRead(asyncOpreation));
        }
        catch (Exception e)
        {
            tcs.SetException(e);
        }
    }, state);

    return tcs.Task;
}

var stream = new FileStream(@"C:\Users\arekp\Downloads\keycloak-user-migration-master\keycloak-user-migration-master\docker\TAPManually\Program.cs", FileMode.Open);

var buffer = new byte[1000];

var task = ReadTask(stream,buffer,0, 1000, null);
task.Wait();

Console.WriteLine(task.Result);

Console.ReadLine();