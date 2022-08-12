namespace Digital_Jungle_Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
            .UseStartup<DJ_Startup>()
            .Build()
            .Run();
    }
}
