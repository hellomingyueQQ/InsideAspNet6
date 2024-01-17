<Query Kind="Program">
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

void Main(string[] args)
{
    var app = WebApplication.Create(args);
	IApplicationBuilder appBuilder = app;
	appBuilder.Use(HelloMiddleware).Use(WorldMiddleware);
	app.Run();

}

static RequestDelegate HelloMiddleware(RequestDelegate next) => async httpContext => {
	await httpContext.Response.WriteAsync("Hello, ");
	await next(httpContext);
};

static RequestDelegate WorldMiddleware(RequestDelegate next)=> httpContext=>httpContext.Response.WriteAsync("World!");




