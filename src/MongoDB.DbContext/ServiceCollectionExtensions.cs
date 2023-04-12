namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using JetBrains.Annotations;
	using MadEyeMatt.MongoDB.DbContext.Initialization;
	using MadEyeMatt.MongoDB.DbContext.Serialization;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	/// <summary>
	///     Extensions methods for the <see cref="IServiceCollection" /> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Adds the given context with optional initial <see cref="MongoDbContextOptions"/> configured
		///		using the <see cref="MongoDbContextOptionsBuilder"/>.
		/// </summary>
		/// <typeparam name="TContext">The context type to register.</typeparam>
		/// <param name="services">The service collection.</param>
		/// <param name="optionsAction">The (optional) options builder action.</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection services, Action<MongoDbContextOptionsBuilder> optionsAction = null)
			where TContext : MongoDbContext
		{
			optionsAction ??= _ =>
			{
			};

			services.TryAddScoped<TContext>();
			services.TryAddScoped(serviceProvider => CreateMongoDbContextOptions<TContext>(serviceProvider, (_, builder) => optionsAction.Invoke(builder)));
			services.TryAddScoped<MongoDbContextOptions>(serviceProvider => serviceProvider.GetRequiredService<MongoDbContextOptions<TContext>>());

			SerializationManager.Initialize();

			return services;
		}

		/// <summary>
		///		Adds a singleton schema service. This service will be used when the database is initialized
		/// </summary>
		/// <typeparam name="TEnsureSchema">The type of the schema service.</typeparam>
		/// <param name="services">The service collection.</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddEnsureSchema<TEnsureSchema>(this IServiceCollection services) 
			where TEnsureSchema : class, IEnsureSchema
		{
			return services.AddEnsureSchema(typeof(TEnsureSchema));
		}

		///  <summary>
		/// 		Adds a singleton schema service. This service will be used when the database is initialized
		///  </summary>
		///  <param name="services">The service collection.</param>
		///  <param name="ensureSchemaImplementationType">The type of the schema service.</param>
		///  <returns>The service collection.</returns>
		public static IServiceCollection AddEnsureSchema(this IServiceCollection services, Type ensureSchemaImplementationType)
		{
			if(!ensureSchemaImplementationType.IsAssignableTo(typeof(IEnsureSchema)))
			{
				throw new InvalidOperationException("The service doesn't implement the IEnsureSchema interface.");
			}

			return services.AddSingleton(typeof(IEnsureSchema), ensureSchemaImplementationType);
		}

		private static MongoDbContextOptions<TContext> CreateMongoDbContextOptions<TContext>(
			IServiceProvider serviceProvider,
			Action<IServiceProvider, MongoDbContextOptionsBuilder> optionsAction)
			where TContext : MongoDbContext
		{
			MongoDbContextOptionsBuilder<TContext> optionsBuilder = new MongoDbContextOptionsBuilder<TContext>(new MongoDbContextOptions<TContext>());

			optionsAction?.Invoke(serviceProvider, optionsBuilder);

			return optionsBuilder.Options;
		}
	}
}
