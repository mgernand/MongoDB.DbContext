namespace MongoDB.DbContext.UnitTests
{
	using MadEyeMatt.MongoDB.DbContext;

	public class SampleContextTwo : MongoDbContext
	{
		public SampleContextTwo(MongoDbContextOptions options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected internal override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			builder.UseDatabase("mongodb://localhost:27017", "test-2");

			base.OnConfiguring(builder);
		}
	}
}
