# SmartStart SignalR Simple Example

## Exercise 1 - Basic Hub and execution of requests by Client on Server


### Server
1. Create new WebAPI project (SignalR.Server) in dotnet 6.0 without https support
1. Create TimeHub class 
1. Define a method which returns current DateTime on server in TimeHub class
1. In Program.cs register SignalR services and register TimeHub on specific address
```
  builder.Services.AddSignalR();

  var app = builder.Build();

  app.MapHub<TimeHub>("/hubs/time");
  
```
> NOTE: TimeHub will be made available on addresss {serverAddress}/hubs/time

> "var" declares that the type will be defined during compilation based on the right side of the equation. <br/>
> https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations#implicitly-typed-local-variables


<br/>



### Client
1. Add new Console project in dotnet 6.0
1. Change signature of Main method in Program.cs
```
static async Task Main(string[] args)
```
3. Define new connection to previously defined TimeHub
```
  var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hubs/time").Build();
```
4. Add code which starts the connection to the Hub
```
await connection.StartAsync();
```
5. Execute method on the Hub and save the result of it, then write it on console
```
  DateTime currentTime = await connection.InvokeAsync<DateTime>("GetCurrentDateTime");
  Console.WriteLine($"Current time is {currentTime}");
```

<br/>



### Summary

This exercise showed how to configure basic method on a server in a Hub and then how to connect the client to the server and execute it.














<br/><br/>

## Exercise 2 - MessagePack vs JSON + Logging


<br/>



### Server
1. Install Microsoft.AspNetCore.SignalR.Protocols.MessagePack 6.*.*
1. In Program.cs configure SignalR to support MessagePack protocol
```
  builder.Services.AddSignalR().AddMessagePackProtocol();
```
3. Create new Hub, StringHub.cs
4. Register new Hub in Program.cs
```
  app.MapHub<StringHub>(“hubs/string”);
```
4. Create new method which will send a predefined text to all connected clients. It means that server has to execute a method X with parameter with predefined text. For example:
```
public async Task SendStringToAllClients()
{
    await Clients.All.SendAsync("showString", "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.");
}
```
> NOTE: We defined that we want to execute "showString" method on clients with the "Lorem.." string as a parameter


<br/>



### Client
1. Install Microsoft.AspNetcore.SignalR.Protocols.MessagePack 6.*.*
2. Modify the connection to connect to new StringHub
```
  var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hubs/string").Build();
```
3. Define how our client will handle the execution of "showString" method from incoming signalR requests
```
connection.On<string>("showString", x =>
{
    Console.WriteLine($"Received string: {x}");
});
```
> NOTE: The syntax `x => {...}` defines so called lambda, that will receive parameter X and execute some action defined between {}. <br/> 
> https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
4. Previous code about getting DateTime from server should be commented out/removed. 
Add execution of `SendStringToAllClients` on the Server from our Client
```
await hubConnection.StartAsync();
await hubConnection.InvokeAsync("SendStringToAllClients");
Console.ReadKey();
```
> NOTE: Console.ReadKey will stop the program from exiting after executing all the code. It execute all defined code and then wait until we click any key inside a Console.
5. Install Microsoft.Extensions.Logging.Console 6.*.* NuGet package 
6. Add logging to the console for our HubConnection. It will add logs/messages to our Console about what's happening in the background for our Connection.
```
var connection = new HubConnectionBuilder()
.WithUrl("http://localhost:5000/hubs/string")
.ConfigureLogging(x =>
{
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Error);
})
.Build();
```


<br/>



### Comparison
1. Rebuild Server and Client projects
2. Run Server with `dotnet run`
3. Run Client project in Visual Studio
4. One of the latest messages should be similar to:
```
Message received. Type: Binary, size: 596, EndOfMessage: True.
```
> NOTE: It shows the actual size and type of the protocol used for the message received. 
5. Close the client application
6. Modify HubConnection definition in order to use MessagePack
```
var connection = new HubConnectionBuilder()
.WithUrl("http://localhost:5000/hubs/string")
.AddMessagePackProtocol()
.ConfigureLogging(x =>
{
    x.AddConsole();
    x.SetMinimumLevel(LogLevel.Error);
})
.Build();
```
6. Rebuild Client application and run it again
7. Look for similar message as previously and compare message sizes


<br/>



### Summary

This exercise showed how to add support for binary protocol MessagePack and the difference in sizes between messages in JSON and MessagePack.

It also shows how to configure logging for HubConnection on Client side. 

Also server was able to execute a specfic method on all clients, which shows the continuous connection between Server and Clients.


<br/>

## Exercise 3 - Invoke method on a server with parameters

### Server
1. Define new NotificationsHub
2. Define new method which sends notifications to all clients, method should get as a parameter a message
3. Register Hub in Program.cs



<br/>

### Client
1. Change Client to connect to newly created Hub
2. Add possibility to read text from Console in a Client and use the text to execute previously defined Notifications method on a Server
> NOTE: You can use methods from Console class like Console.ReadLine() for reading things from Console. 
> You can also use loops in order to continously ask user for input and execute the method on a server.
<br/>

### Summary

This exercise shows that Client can execute methods on a Server with parameters. 


<br/>

# Resources
- Learning C#
  - https://dotnet.microsoft.com/en-us/learn/csharp
- SignalR
  - https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction
  - https://github.com/SignalR/SignalR
  - https://github.com/dotnet/aspnetcore/tree/main/src/SignalR
