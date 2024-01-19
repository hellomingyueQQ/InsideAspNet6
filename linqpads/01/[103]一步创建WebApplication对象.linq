<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
    var app = WebApplication.Create(args);
	app.Run(HandleAsync);
	app.Run();
	
}

static Task HandleAsync(HttpContext httpContext)=>httpContext.Response.WriteAsync("Hello, World!");

