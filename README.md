# Dependapult
Simple dependency injection library for .NET 6

## What is it?

It is a minimal dependency injection library to register all of your dependencies.

## Features

- Two object lifetimes: transient and singleton;
- Convenient methods to register singleton or transient dependencies;
- Transient dependency implementation can be replaced;
- Singletons and the service itself uses lazy loading to save memory;

You can view the changelog [here](./Changelog.md).

## Usage

[Doxygen documentation](https://lzaromskis.github.io/Dependapult/)

Here are some typical scenarios when using Dependapult to show the useage of the service.

### Getting Dependapult service object

First step when using the component is to get the `DependapultService` object which contains all of the relevant methods.

```cs
DependapultService service = DependapultService.Instance;
```

### Registering a dependency without providing arguments

Most typical usage of the service is to provide the lifetime of the object, interface and the implementor types.

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency, Dependency>(DependencyLifetime.Singleton);

// Singleton convenience method
service.RegisterSingleton<IDependency, Dependency>();

// Transient convenience method
service.RegisterTransient<IDependency, Dependency>();
```

### Registering a dependency and providing arguments

A niche use case where you can additionaly provide an array of arguments to pass to the constructor.

```cs
object[] arguments = new object[] { "config", 150, false };

// Manually provide lifetime
service.RegisterDependency<IDependency, Dependency>(DependencyLifetime.Singleton, arguments);

// Singleton convenience method
service.RegisterSingleton<IDependency, Dependency>(arguments);

// Transient convenience method
service.RegisterTransient<IDependency, Dependency>(arguments);
```

### Registering a dependency and providing a creator function

It is possible to provide a creator function which creates a dependecy object. The creator function receives a `DependencyService` object and returns a dependency.

This is meant for cases where a constructor expects primitive types or if the dependecy has multiple public constructor.

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency>(DependencyLifetime.Singleton, s => new Dependency(s.GetDependency<IOtherDependency>(), 200));

// Singleton convenience method
service.RegisterSingleton<IDependency>(s => new Dependency(s.GetDependency<IOtherDependency>(), 200));

// Transient convenience method
service.RegisterTransient<IDependency>(s => new Dependency(s.GetDependency<IOtherDependency>(), 200));
```

### Replacing a dependency

It is possible to replace dependencies. **Only** transient dependencies can be replaced. Already created objects are unaffected.

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency, Dependency>(DependencyLifetime.Transient, true);

// Transient convenience method
service.RegisterTransient<IDependency, Dependency>(true);
```


### Getting a dependency

The final step of the service is to get the dependency that you need. It is recommended to use the method with generics.

```cs
// With generics
IDependency dependency = service.GetDependency<IDependency>();

// Without generics
IDependency dependency = service.GetDependency(typeof(IDependency)) as IDependency;

```

### Tagging a constructor

If your dependency contains multiple public constructors you **must** add a `DependapultConstructor` attribute to the constructor you want to be used by the service.

The attribute is not needed if the depencency has only one public constructor.

```cs
class Dependency
{
    public Dependency() 
    {
        ...
    }

    [DependapultConstructor]
    public Dependency(IOtherDependency otherDependency)
    {
        ...
    }
}
```