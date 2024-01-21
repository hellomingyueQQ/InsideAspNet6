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


var builder = WebApplication.CreateBuilder(new string[] { });
builder.Services
	.AddSingleton<IGreeter, Greeter>()
	.AddControllers()
	.AddApplicationPart(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.MapControllers();
app.Run();


[Route("api/[controller]")]
[ApiController]
public class GreetingController 
{
	 [HttpGet("greet")]
	public string Greet([FromServices] IGreeter greeter) => greeter.Greet(DateTimeOffset.Now);
}

public interface IGreeter
{
	string Greet(DateTimeOffset time);
}

public class Greeter : IGreeter
{
	public Greeter()
	{
	}
	public string Greet(DateTimeOffset time)
	{
		return "test";
	}
}

// AddControllers扩展方法注册了与Controller相关服务的注册。在WebApplication对象被构建出来后，我们调用了它的MapControllers扩展方法将定义在所有Controller类型中的Action方法映射为对应的终结点。


