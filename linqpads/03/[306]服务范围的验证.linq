<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
</Query>

void Main()
{
	var root = new ServiceCollection().AddSingleton<IFoo, Foo>().AddScoped<IBar, Bar>().BuildServiceProvider(true); 
	// 使用.BuildServiceProvider(true)方法创建的根ServiceProvider是不支持创建Scoped服务的。
	var child = root.CreateScope().ServiceProvider;
	ResolveService<IFoo>(root);
	ResolveService<IBar>(root);
	ResolveService<IFoo>(child);
	ResolveService<IBar>(child);

	void ResolveService<T>(IServiceProvider provider){
		var isRootContainer = root == provider?"Yes":"No";
		try
		{
			provider.GetService<T>();
			$"Status: Success; Service Type: {typeof(T).Name}; Root: {isRootContainer}".Dump();
		}
		catch (Exception ex)
		{
			$"Status: Fail; Service Type: { typeof(T).Name}; Root: { isRootContainer}".Dump();
			$"Error: {ex.Message}".Dump();
		}
	}
}

public interface IFoo { }
public interface IBar { }
public class Foo:IFoo{
	public Foo(IBar bar)
	{
		Bar = bar;
	}

	public IBar Bar {get;}
}

public class Bar : IBar { }