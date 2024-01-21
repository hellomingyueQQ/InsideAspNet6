<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
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
var builder = WebApplication.CreateBuilder(new string[] { });
builder.Services
	.AddSingleton<IGreeter, Greeter>()
	.Configure<GreetingOptions>(builder.Configuration.GetSection("greeting"))
	.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.MapControllers();
app.Run();


public class GreetingController
{
	[HttpGet("/greet")]
	public string Greet([FromServices] IGreeter greeter) => greeter.Greet(DateTimeOffset.Now);
}

public interface IGreeter
{
	string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
	private readonly GreetingOptions _options;
	private readonly ILogger _logger;
	public Greeter(IOptions<GreetingOptions> optionsAccessor, ILogger<Greeter> logger)
	{
		_options = optionsAccessor.Value;
		_logger = logger;
	}
	public string Greet(DateTimeOffset time)
	{
		var message = time.Hour switch
		{
			var h when h >= 5 && h < 12 => _options.Morning,
			var h when h >= 12 && h < 17 => _options.Afternoon,
			_ => _options.Evening
		};
		_logger.LogInformation(message: "{time}={message}", time, message);
		return message;
	}
}

public class GreetingOptions
{
	public string Morning { get; set; } = default!;
	public string Afternoon { get; set; } = default!;
	public string Evening { get; set; } = default!;
}

// https://forum.linqpad.net/discussion/2909/linqpad-unable-to-pick-up-apicontroller
// https://forum.linqpad.net/discussion/2599/same-mini-asp-net-core-code-in-linqpad-unable-to-get-responsed-but-good-in-visual-studio
