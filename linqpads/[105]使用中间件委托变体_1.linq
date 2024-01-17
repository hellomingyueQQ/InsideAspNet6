<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
    var app = WebApplication.Create(args);
	app.Use(HelloMiddleware).Use(WorldMiddleware);
	app.Run();
}

static async Task HelloMiddleware(HttpContext httpContext, RequestDelegate next){
	await httpContext.Response.WriteAsync("Hello, ");
	await next(httpContext);
}

static Task WorldMiddleware(HttpContext httpContext, RequestDelegate next)=> httpContext.Response.WriteAsync("World!");

// Func<HttpContext, RequestDelegate, Task>





