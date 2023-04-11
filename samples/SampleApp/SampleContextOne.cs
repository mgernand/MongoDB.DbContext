namespace SampleApp
{
	using MadEyeMatt.MongoDB.DbContext;

	public class SampleContextOne : MongoDbContext
	{
		public SampleContextOne(MongoDbContextOptions<SampleContextOne> options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			builder.UseDatabase("mongodb://localhost:27017", "test-1");

			base.OnConfiguring(builder);
		}
	}
}
