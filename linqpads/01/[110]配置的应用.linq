<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


string demoFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
string json = @"
{
  ""greeting"": {

	""morning"": ""Good morning!"",
    ""afternoon"": ""Good afternoon!"",
    ""evening"": ""Good evening!""
  }
}";

File.WriteAllText(demoFilePath, json);

var builder = WebApplication.CreateBuilder();
builder.Services.AddSingleton<IGreeter, Greeter>();
var app = builder.Build();
app.UseMiddleware<GreetingMiddleware>();
app.Run();


public interface IGreeter
{
    string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
    private readonly IConfiguration _configuratioin;
    public Greeter(IConfiguration configuration)
    {
        _configuratioin = configuration.GetSection("greeting");
    }
    public string Greet(DateTimeOffset time) => time.Hour switch
    {
        var h when h >= 5 && h < 12 =>_configuratioin["morning"],
        var h when h >= 12 && h < 17 => _configuratioin["afternoon"],
        _ => _configuratioin["evening"]
    };
}


public class GreetingMiddleware
{
	public GreetingMiddleware(RequestDelegate next)
	{
	}

	public Task InvokeAsync(HttpContext context, IGreeter greeter) => context.Response.WriteAsync(greeter.Greet(DateTimeOffset.Now));
}

