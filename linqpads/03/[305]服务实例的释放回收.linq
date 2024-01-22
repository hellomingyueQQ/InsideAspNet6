<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
</Query>

void Main()
{
	using (var root = new ServiceCollection().AddTransient<IFoo, Foo>().AddScoped<IBar, Bar>().AddSingleton<IBaz,Baz>().BuildServiceProvider())
	{
		using (var scope = root.CreateScope())
		{
			var provider = scope.ServiceProvider;
			provider.GetService<IFoo>();
			provider.GetService<IBar>();
			provider.GetService<IBaz>();
			"Child container is disposed.".Dump();
		}
		"Root container is disposed.".Dump();
	}
}

public interface IFoo { }
public interface IBar { }
public interface IBaz { }
public interface IFoobar<T1, T2> { }
public class Base : IDisposable
{
	public Base() => Console.WriteLine($"An instance of {GetType().Name} is created.");
	public void Dispose() => Console.WriteLine($"The instance of {GetType().Name} is disposed.");
}

public class Foo : Base, IFoo, IDisposable { }
public class Bar : Base, IBar, IDisposable { }
public class Baz : Base, IBaz, IDisposable { }
