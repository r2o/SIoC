# SIoC

SIoC is a small and fast IoC engine.
This engine avoid every unnecesarry overload you can have.

With SIoC you can bind your interfaces with the implementation in 4 different ways:
- Singleton: gaves you the same instance over and over the time.
- Transient: gaves you a new instance of the implmentation every time you need it.
- Constant: which is a Singleton but with a constructor that use parameters like string, DateTime, int, etc.
- Method: is like Transient but with a constructor that use parameters like string, DateTime, int, etc.

### Usage

To use intall it by Package Manager Console and type Install-Package SIoC.core

After you download the package you need to create your class where all the bindings are going to be defined.
Your class should inherit BaseIoCModule and you need to implement the method called Load

###Defining an IoCModule
```

public class IoCModule : BaseIoCModule
{
	public IoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr) 
		: base(br, rr)
	{
	}

	public override void Load()
	{
		BindInSingleton<IHelloWorld, HelloWorld>();
		/// or BindInTransient<IHelloWorld, HelloWorld>();
		/// or BindToMethod<IHelloWorld>(()=> new HelloWorld());
		/// or BindToConstant<IHelloWorld>(new HelloWorld());
	}
}
```

Once you already have done that part, you need to add the following configuration on your application config
```
  <configSections>
    <section name="IoCSection" type="SIoC.core.Config.IoCConfigurationSection, SIoC.core"/>
  </configSections>

  <IoCSection IoCModule="<Class that has the bindings>, <assembly name>" ContainerProvider="SIoC.core.Impl.SIoCContainer, SIoC.core">
```

###Example for application config

```
<IoCSection IoCModule="ConsoleTest.IoCModule, ConsoleTest" ContainerProvider="SIoC.core.Impl.SIoCContainer, SIoC.core">
```
