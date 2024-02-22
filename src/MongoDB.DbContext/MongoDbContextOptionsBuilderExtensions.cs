namespace MadEyeMatt.MongoDB.DbContext
{
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
		public static MongoDbContextOptionsBuilder UseDatabase(this MongoDbContextOptionsBuilder builder, 
			string connectionString, string databaseName)
		{
			Argument.ThrowIfNull(builder);
			Argument.ThrowIfNullOrEmpty(connectionString);
			Argument.ThrowIfNullOrEmpty(databaseName);

			builder.Options.ConnectionString = connectionString;
			builder.Options.DatabaseName = databaseName;

			return builder;
		}

		/// <summary>
		///		Sets a flag, indicating that the telemetry/diagnostics are enabled for the MongoDB driver.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="enableTelemetry"></param>
		/// <returns></returns>
		public static MongoDbContextOptionsBuilder EnableTelemetry(this MongoDbContextOptionsBuilder builder,
			bool enableTelemetry = true)
		{
			Argument.ThrowIfNull(builder);

			builder.Options.EnableTelemetry = enableTelemetry;

			return builder;
		}

		/// <summary>
		///		Sets a flag, indicating that the command text should be part of the telemetry.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="captureCommandText"></param>
		/// <returns></returns>
		public static MongoDbContextOptionsBuilder CaptureCommandText(this MongoDbContextOptionsBuilder builder, 
			bool captureCommandText = true)
		{
			Argument.ThrowIfNull(builder);

			builder.Options.CaptureCommandText = captureCommandText;

			return builder;
		}
	}
}
