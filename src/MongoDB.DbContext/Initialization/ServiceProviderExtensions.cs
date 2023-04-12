namespace MadEyeMatt.MongoDB.DbContext.Initialization
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     Extension methods for the <see cref="IServiceProvider" /> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceProviderExtensions
	{
		/// <summary>
		///     Initializes the MongoDB driver and ensures schema and indexes.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public static async Task InitializeMongoDbStores(this IServiceProvider serviceProvider)
		{
			IEnumerable<IEnsureSchema> ensureSchemata = serviceProvider.GetServices<IEnsureSchema>();
			foreach(IEnsureSchema ensureSchema in ensureSchemata)
			{
				await ensureSchema.ExecuteAsync();
			}
		}
	}
}
