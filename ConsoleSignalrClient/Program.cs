using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleSignalrClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                //.WithUrl("http://localhost:5239/hubs/time")
                //WithUrl("http://localhost:5239/hubs/string")
                .WithUrl("http://localhost:5239/hubs/notification")
                //.AddMessagePackProtocol()
                .ConfigureLogging(x =>
                {
                    x.AddConsole();
                    x.SetMinimumLevel(LogLevel.Error);
                })
                .Build();

            connection.On<string>("showString", ShowNotification);

            await connection.StartAsync();
            //DateTime currentTime = await connection.InvokeAsync<DateTime>("GetCurrentDateTime");

            //Console.WriteLine($"Current time is {currentTime}");

            //await connection.InvokeAsync("SendStringToAllClients");
            var message = Console.ReadLine();
            await connection.InvokeAsync("SendNotification", message);

            Console.ReadKey();
        }

        private static void ShowNotification(string message)
        {
            Console.WriteLine(message);
        }
    }
}