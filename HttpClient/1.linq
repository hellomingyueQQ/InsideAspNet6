<Query Kind="Statements">
  <NuGetReference Version="6.0.0">Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference Version="6.0.0">Microsoft.Extensions.Http</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>Microsoft.Extensions.Http</Namespace>
  <Namespace>Microsoft.Extensions.Http.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Abstractions</Namespace>
  <Namespace>Microsoft.Extensions.Options</Namespace>
  <Namespace>Microsoft.Extensions.Primitives</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

#region appsettings.json

string demoFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
string json = @"
{
""Logging"": {

	""LogLevel"": {
	""Default"": ""Warning"",
      ""Microsoft.AspNetCore"": ""Warning""
	}
  },
  ""AllowedHosts"": ""*""
}";

File.WriteAllText(demoFilePath, json);

#endregion

var builder = WebApplication.CreateBuilder(new string[] { });
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IIntegrationService, CRUDSamples>()
		.AddSingleton<JsonSerializerOptionsWrapper>()
	.AddControllers()
	.AddApplicationPart(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.MapControllers();
app.Run();


#region Controllers

[Route("api/[controller]")]
[ApiController]
public class GreetingController
{
	private readonly IIntegrationService _service;
	public GreetingController(IIntegrationService service)
	{
		_service = service;
	}

	[HttpGet("greet")]
	public async Task Greet()
	{
		await _service.RunAsync();
	}
}

[Route("api/[controller]")]
[ApiController]
public class WeatherController
{

	[HttpGet("")]
	public IList<WeatherForecast> GetWeather()
	{
		var summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};
		var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray();
		forecast.Dump("发送");
		return forecast;
	}
}
#endregion

#region Services

public interface IIntegrationService
{
	Task RunAsync();
}

public class CRUDSamples : IIntegrationService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
	public CRUDSamples(IHttpClientFactory httpClientFactory, JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
	{
		_httpClientFactory = httpClientFactory ??
		   throw new ArgumentNullException(nameof(httpClientFactory));
		_jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
	}

	public async Task RunAsync()
	{
		await GetResourceAsync();
	}

	public async Task GetResourceAsync()
	{
		var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");
		httpClient.BaseAddress = new Uri("http://localhost:5000");
		httpClient.Timeout = new TimeSpan(0, 0, 30);
		var response = await httpClient.GetAsync("api/weather");
		response.EnsureSuccessStatusCode();
		var content = await response.Content.ReadAsStringAsync();
		
		 if (response.Content.Headers.ContentType?.MediaType == "application/json")
		{
			var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(
				content,
				_jsonSerializerOptionsWrapper.Options);
				weatherForecast.Dump("收到");
		}
	}
}

#endregion

#region Models

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

#endregion


public class JsonSerializerOptionsWrapper
{
	public JsonSerializerOptions Options { get; }

	public JsonSerializerOptionsWrapper()
	{
		Options = new JsonSerializerOptions(
			JsonSerializerDefaults.Web);
	}
}


//http://localhost:5000/api/greeting/greet



// AddControllers扩展方法注册了与Controller相关服务的注册。在WebApplication对象被构建出来后，我们调用了它的MapControllers扩展方法将定义在所有Controller类型中的Action方法映射为对应的终结点。


