namespace MongoDB.DbContext.UnitTests
{
	using MadEyeMatt.MongoDB.DbContext;

	public class SampleContextOne : MongoDbContext
	{
		public SampleContextOne(MongoDbContextOptions options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected internal override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			builder.UseDatabase("mongodb://localhost:27017", "test-1");

			base.OnConfiguring(builder);
		}
	}
}
