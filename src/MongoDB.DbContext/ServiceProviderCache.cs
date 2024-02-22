namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using System.Collections.Concurrent;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Core.Extensions.DiagnosticSources;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///		Creates a caches <see cref="IServiceProvider"/> instances that are used internally in <see cref="MongoDbContext"/> instances.
	/// </summary>
	internal sealed class ServiceProviderCache
	{
		private readonly ConcurrentDictionary<MongoDbContextOptions, IServiceProvider> serviceProviders = 
			new ConcurrentDictionary<MongoDbContextOptions, IServiceProvider>();

		private ServiceProviderCache()
		{
		}

		public static ServiceProviderCache Instance { get; } = new ServiceProviderCache();

		public IServiceProvider GetOrAdd(MongoDbContextOptions options)
		{
			return this.serviceProviders.GetOrAdd(options, static contextOptions =>
			{
				IServiceCollection services = new ServiceCollection();

				ValidateContextOptions(contextOptions);

				// Add the MongoDB services for this context internal service provider.
				services.AddSingleton<IMongoClient>(_ =>
				{
					MongoClientSettings clientSettings = MongoClientSettings.FromConnectionString(contextOptions.ConnectionString);

					// https://github.com/jbogard/MongoDB.Driver.Core.Extensions.DiagnosticSources
					if (contextOptions.EnableTelemetry)
					{
						InstrumentationOptions instrumentationOptions = new InstrumentationOptions
						{
							CaptureCommandText = contextOptions.CaptureCommandText,
							//ShouldStartActivity = @event => !"collectionToIgnore".Equals(@event.GetCollectionName())
						};

						clientSettings.ClusterConfigurator = builder =>
						{
							builder.Subscribe(new DiagnosticsActivityEventSubscriber(instrumentationOptions));
						};
					}

					return new MongoClient(clientSettings);
				});
				services.AddSingleton(serviceProvider =>
				{
					IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();
					return client.GetDatabase(contextOptions.DatabaseName);
				});

				return services.BuildServiceProvider();
			});
		}

		private static void ValidateContextOptions(MongoDbContextOptions contextOptions)
		{
			if(string.IsNullOrWhiteSpace(contextOptions.ConnectionString))
			{
				throw new InvalidOperationException("The connection string must be set.");
			}

			if (string.IsNullOrWhiteSpace(contextOptions.DatabaseName))
			{
				throw new InvalidOperationException("The database name must be set.");
			}
		}
	}
}
