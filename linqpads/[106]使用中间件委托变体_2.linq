<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
	var app = WebApplication.Create();
	app.Use(HelloMiddleware).Use(WorldMiddleware);
	app.Run();
}

static async Task HelloMiddleware(HttpContext httpContext, Func<Task> next){
	await httpContext.Response.WriteAsync("Hello, ");
	await next();
}

static Task WorldMiddleware(HttpContext httpContext, Func<Task> next)=>httpContext.Response.WriteAsync("World!");







// Func<HttpContext, Func<Task>, Task>