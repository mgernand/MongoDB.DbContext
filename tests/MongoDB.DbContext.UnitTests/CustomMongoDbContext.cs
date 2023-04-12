namespace MongoDB.DbContext.UnitTests
{
	using MadEyeMatt.MongoDB.DbContext;

	public sealed class CustomMongoDbContext : MongoDbContext
	{
		/// <inheritdoc />
		protected internal override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			if (!builder.IsConfigured)
			{
				builder.UseDatabase(GlobalFixture.ConnectionString, "changed");
			}
		}
	}
}
