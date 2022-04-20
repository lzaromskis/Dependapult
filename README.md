# Dependapult
Simple dependency injection library for .NET 6

[[TOC]]

## What is it?

It is a minimal dependency injection library to register all of your dependencies.

## Features

- Two object lifetimes: transient and singleton;
- Convenient methods to register singleton or transient dependencies;
- Transient dependency implementation can be replaced;
- Singletons and the service itself uses lazy loading to save memory;

## Usage

Here are some typical scenarios when using Dependapult to show the useage of the service.

### Getting Dependapult service object

```cs
DependapultService service = DependapultService.Instance;
```

### Registering a dependency without providing arguments

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency, Dependency>(DependencyLifetime.Singleton);

// Singleton convenience method
service.RegisterSingleton<IDependency, Dependency>();

// Transient convenience method
service.RegisterTransient<IDependency, Dependency>();
```

### Registering a dependency and providing arguments

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

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency>(DependencyLifetime.Singleton, s => new Dependency(s.GetDependency<IOtherDependency>(), 200));

// Singleton convenience method
service.RegisterSingleton<IDependency>(s => new Dependency(s.GetDependency<IOtherDependency>(), 200));

// Transient convenience method
service.RegisterTransient<IDependency>(s => new Dependency(s.GetDependency<IOtherDependency>(), 200));
```

### Replacing a dependency

```cs
// Manually provide lifetime
service.RegisterDependency<IDependency, Dependency>(DependencyLifetime.Transient, true);

// Transient convenience method
service.RegisterTransient<IDependency, Dependency>(true);
```


### Getting a dependency

```cs
// With generics
IDependency dependency = service.GetDependency<IDependency>();

// Without generics
IDependency dependency = service.GetDependency(typeof(IDependency)) as IDependency;

```