namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	/// <summary>
	///     Extensions methods for the <see cref="IServiceCollection" /> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Registers the given context with optional initial <see cref="MongoDbContextOptions"/>m configured
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

			services.TryAddScoped(serviceProvider => CreateMongoDbContextOptions<TContext>(serviceProvider, (_, builder) => optionsAction.Invoke(builder)));
			services.TryAddScoped<MongoDbContextOptions>(serviceProvider => serviceProvider.GetRequiredService<MongoDbContextOptions<TContext>>());

			services.TryAddScoped<TContext>();

			return services;
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
