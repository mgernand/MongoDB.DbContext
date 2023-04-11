namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		Extension methods for the <see cref="MongoDbContextOptionsBuilder"/> type.
	/// </summary>
	[PublicAPI]
	public static class MongoDbContextOptionsBuilderExtensions
	{
		/// <summary>
		///		Configures the MongoDB database to use.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="connectionString"></param>
		/// <param name="databaseName"></param>
		/// <returns></returns>
		public static MongoDbContextOptionsBuilder UseDatabase(this MongoDbContextOptionsBuilder builder, string connectionString, string databaseName)
		{
			ArgumentException.ThrowIfNullOrEmpty(connectionString);
			ArgumentException.ThrowIfNullOrEmpty(databaseName);

			builder.Options.ConnectionString = connectionString;
			builder.Options.DatabaseName = databaseName;

			return builder;
		}
	}
}
