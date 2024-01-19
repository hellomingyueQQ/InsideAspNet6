<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main()
{
	var builder = WebApplication.CreateBuilder();
	builder.Services.AddSingleton<IGreeter, Greeter>();
	var app = builder.Build();
	app.UseMiddleware<GreetingMiddleware>();
	app.Run();
}

public interface IGreeter
{
	string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
	public string Greet(DateTimeOffset time) => time.Hour switch
	{
		var h when h >= 5 && h < 12 => "Good morning",
		var h when h >= 12 && h < 17 => "Good afternoon",
		_ => "Good evening"
	};
}


public class GreetingMiddleware
{
	public GreetingMiddleware(RequestDelegate next)
	{
	}

	public Task InvokeAsync(HttpContext context, IGreeter greeter) => context.Response.WriteAsync(greeter.Greet(DateTimeOffset.Now));
}

