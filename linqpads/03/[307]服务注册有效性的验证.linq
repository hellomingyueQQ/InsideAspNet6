<Query Kind="Program">
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection.Extensions</Namespace>
</Query>

void Main()
{
	BuildServiceProvider(false);
	BuildServiceProvider(true);
}

static void BuildServiceProvider(bool validateOnBuild){
	try
	{	        
		var options = new ServiceProviderOptions(){
			ValidateOnBuild = validateOnBuild
		};
		new ServiceCollection().AddSingleton<IFoobar, Foobar>().BuildServiceProvider(options);
		$"Status: Success; ValidateOnBuild: {validateOnBuild}".Dump();
		
	}
	catch (Exception ex)
	{
		$"Status: Fail; ValidateOnBuild: {validateOnBuild}".Dump();
		$"Error: {ex.Message}".Dump();
	}
}

public interface IFoobar{
	
}

public class Foobar : IFoobar
{
	private Foobar() {}
	public static readonly Foobar Instance = new Foobar();
}

