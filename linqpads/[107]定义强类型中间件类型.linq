<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
	var builder = WebApplication.CreateBuilder(args);
	builder.Services.AddSingleton<IGreeter, Greeter>()
	.AddSingleton<GreetingMiddleware>();
	
	var app = builder.Build();
	app.UseMiddleware<GreetingMiddleware>();
	app.Run();
}

public interface IGreeter{
	string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
	public string Greet(DateTimeOffset time)=>time.Hour switch{
		var h when h>=5 && h<12 => "Good morning",
		var h when h>=12 && h<17=> "Good afternoon",
		_=>"Good evening"
	};
}


public class GreetingMiddleware : IMiddleware
{
	private readonly IGreeter _greeter;
	public GreetingMiddleware(IGreeter greeter)=>_greeter = greeter;
	
	public Task InvokeAsync(HttpContext context, RequestDelegate next) => context.Response.WriteAsync(_greeter.Greet(DateTimeOffset.Now));
} 