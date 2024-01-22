<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
</Query>

void Main()
{
	var root = new ServiceCollection()
				.AddTransient<IFoo, Foo>()
				.AddScoped<IBar>(_ => new Bar())
				.AddSingleton<IBaz, Baz>()
				.BuildServiceProvider();
	var provider1 = root.CreateScope().ServiceProvider;
	var provider2 = root.CreateScope().ServiceProvider;

	GetServices<IFoo>(provider1);
	GetServices<IBar>(provider1);
	GetServices<IBaz>(provider1);
	Console.WriteLine();
	GetServices<IFoo>(provider2);
	GetServices<IBar>(provider2);
	GetServices<IBaz>(provider2);

	static void GetServices<T>(IServiceProvider provider)
	{
		provider.GetService<T>();
		provider.GetService<T>();
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
