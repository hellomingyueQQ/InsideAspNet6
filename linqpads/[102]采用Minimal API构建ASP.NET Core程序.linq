<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
	RequestDelegate handler = context=>context.Response.WriteAsync("Hello, World!");
	WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
	WebApplication app = builder.Build();
	app.Run(handler);
	app.Run();
}
