namespace MongoDB.DbContext.UnitTests
{
	using MadEyeMatt.MongoDB.DbContext;

	public class CustomMongoDbContext : MongoDbContext
	{
		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			if(!builder.IsConfigured)
			{
				builder.UseDatabase(GlobalFixture.ConnectionString, "changed");
			}
		}
	}
}
