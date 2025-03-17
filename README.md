# MongoDB.DbContext

A DB context implementation for MongoDB.

The DB context implementation is very heavily inspired by the EntityFramework Core ```DbContext```
infrastructure. The options configuration works the same. The database options can be set when
configuring the context, or agail later (and maybe altered) when new instances of the context
are created.

The context services are registered as scoped automatically. Every context will get the MongoDB
driver services (```IMongoDatabase```, ```IMongoClient```) from an interner service provider which
will have those services registered as singleton instances. This cache of internal service providers
uses the conenction string, str database name and the context options type as key, so changing the
database name for a context at runtime (by overriding ```OnConfiguring```) will result in a new internal
service provider with the corresponding MongoDB singleton services.

## This repository was moved to https://codeberg.org/mgernand/MongoDB.DbContext

## Usage

To add a ```MongoDbContext``` one uses the ```AddMongoDbContext``` extension method. There
are several ways to configure one or multiple contexts.

### Configuration without custom context class

To get started quickly, one can simply use the ```MongoDbContext``` class and configure the
database by passing an options builder action to ```AddMongoDbContext```. This way you have
no possibility to further configure the context at runtime.

```C#
services.AddMongoDbContext<MongoDbContext>(builder =>
{
	builder.UseDatabase("mongodb://localhost:27017", "test");
});
```

### Configuration with custom context class

If you need more control ove the context configuration or need to add multiple different
contexts, you need to dervice you context class from ```MongoDbContext```

```C#
services.AddMongoDbContext<SampleContext>(builder =>
{
	builder.UseDatabase("mongodb://localhost:27017", "test");
});
```

```C#
public sealed class SampleContext : MongoDbContext
{
	public SampleContext(MongoDbContextOptions options)
		: base(options)
	{
	}

	/// <inheritdoc />
	protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
	{
		builder.UseDatabase("mongodb://localhost:27017", "other");
	}
}
```

The ```OnConfiguring``` method is called everytime a new instance of the context is created,
so it is possible to change the connection settings for the context based on runtime information.

### Configuring multiple different context classes

If you need to add several contexts you need to inject the generic version of the ```MongoDbContextOptions```
to allow the correct options be created. There will be an exception thrown if you don't do it.

```C#
services.AddMongoDbContext<SampleContextOne>(builder =>
{
	builder.UseDatabase("mongodb://localhost:27017", "one");
});

services.AddMongoDbContext<SampleContextTwo>(builder =>
{
	builder.UseDatabase("mongodb://localhost:27017", "two");
});
```

```C#
public sealed class SampleContextOne : MongoDbContext
{
	public SampleContext(MongoDbContextOptions<SampleContextOne> options)
		: base(options)
	{
	}
}

public sealed class SampleContextTwo : MongoDbContext
{
	public SampleContext(MongoDbContextOptions<SampleContextTwo> options)
		: base(options)
	{
	}
}
```

### Configuring telemetry information

To expose telemetry information via ```System.Diagnostics``` just enable it in the ```MongoDbContextOptions```.
If you like to integrate this telemetry into OpenTelemetry just add a source with the name
```MongoDB.Driver.Core.Extensions.DiagnosticSources```.

```C#
services.AddMongoDbContext<SampleContext>(builder =>
{
	builder
		.UseDatabase("mongodb://localhost:27017", "test")
		.EnableTelemetry();
});
```


## References

[EntityFramework Core](https://github.com/dotnet/efcore)

[MongoDB.Driver.Core.Extensions.DiagnosticSources](https://github.com/jbogard/MongoDB.Driver.Core.Extensions.DiagnosticSources)
