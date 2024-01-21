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
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


var builder = WebApplication.CreateBuilder(new string[] { });
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IIntegrationService, CRUDSamples>()
	.AddControllers()
	.AddApplicationPart(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.MapControllers();
app.Run();


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
		forecast.Dump();
		return forecast;
	}
}

public interface IIntegrationService
{
	Task RunAsync();
}

public class CRUDSamples : IIntegrationService
{
	private readonly IHttpClientFactory _httpClientFactory;
	public CRUDSamples(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory ??
		   throw new ArgumentNullException(nameof(httpClientFactory));
	}

	public async Task RunAsync()
	{
		 await GetResourceAsync();
	}

	public async Task GetResourceAsync() {
		var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");
		httpClient.BaseAddress = new Uri("http://localhost:5000");
		httpClient.Timeout = new TimeSpan(0, 0, 30);
		var response = await httpClient.GetAsync("api/weather");
		response.EnsureSuccessStatusCode();
	}
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


// AddControllers扩展方法注册了与Controller相关服务的注册。在WebApplication对象被构建出来后，我们调用了它的MapControllers扩展方法将定义在所有Controller类型中的Action方法映射为对应的终结点。


