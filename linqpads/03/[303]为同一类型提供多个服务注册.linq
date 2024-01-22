<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
</Query>

void Main()
{
	var services = new ServiceCollection()
		.AddTransient<Base, Foo>()
		.AddTransient<Base, Bar>()
		.AddTransient<Base, Baz>()
		.BuildServiceProvider()
		.GetServices<Base>();
 
    services.Dump();
	Debug.Assert(services.OfType<Foo>().Any());
	Debug.Assert(services.OfType<Bar>().Any());
	Debug.Assert(services.OfType<Baz>().Any());
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
