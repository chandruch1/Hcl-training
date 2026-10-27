//LearnAsyncProgramming obj = new LearnAsyncProgramming();
//await obj.BasicAsync();
//Console.WriteLine(await obj.TrackTaskAsync());

public class LearnAsyncProgramming 
{
    public static async Task BasicAsync()
    {
        Console.WriteLine("BasicAsync started 1");
        await Task.Delay(7000);
        Console.WriteLine("BasicAsync started 2");
        Console.WriteLine("BasicAsyn started 3");
    }

    public static async Task<int> TrackTaskAsync()
    {
        Console.WriteLine("Sub method started...");
        await Task.Delay(9000);
        Console.WriteLine("Sub method completed");
        return 18;

    }

    public static async Task TaskTrackingAsync()
    {
        Console.WriteLine("Main method is started");
        Task<int> task1 = TrackTaskAsync();
        Console.WriteLine("Back to main method");
        Console.WriteLine($"status : {task1.Status}");
        Console.WriteLine($"IsCompleted : {task1.IsCompleted}");
        Console.WriteLine("Doing main method work...");
        int res = await task1;
        Console.WriteLine($"Result: {res}");
        Console.WriteLine($"IsCompleted: {task1.IsCompleted}");
        Console.WriteLine($"Status: {task1.Status}");

        Console.WriteLine("Main method completed");



    }
    public  async static  Task Main(string[] args)
    {
        await TaskTrackingAsync();
    }
}

