<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
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
builder.Services.AddSingleton<IGreeter, Greeter>().Configure<GreetingOptions>(builder.Configuration.GetSection("greeting"));
var app = builder.Build();
app.UseMiddleware<GreetingMiddleware>();
app.Run();


public interface IGreeter
{
    string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
    private readonly GreetingOptions _options;
    public Greeter(IOptions<GreetingOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;
    }
    public string Greet(DateTimeOffset time) => time.Hour switch
    {
        var h when h >= 5 && h < 12 =>_options.Morning,
        var h when h >= 12 && h < 17 => _options.Afternoon,
        _ => _options.Evening
    };
}


public class GreetingMiddleware
{
	public GreetingMiddleware(RequestDelegate next)
	{
	}

	public Task InvokeAsync(HttpContext context, IGreeter greeter) => context.Response.WriteAsync(greeter.Greet(DateTimeOffset.Now));
}

public class GreetingOptions
{
	public string Morning { get; set; } = default!;
	public string Afternoon { get; set; } = default!;
	public string Evening { get; set; } = default!;
}

