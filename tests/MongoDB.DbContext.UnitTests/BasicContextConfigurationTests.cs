namespace MongoDB.DbContext.UnitTests
{
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;
	using System.Reflection;
	using FluentAssertions;

	[TestFixture]
	public class BasicContextConfigurationTests
	{
		private ServiceProvider serviceProvider;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddMongoDbContext<MongoDbContext>(options =>
			{
				options.UseDatabase(GlobalFixture.ConnectionString, GlobalFixture.Database);
			});

			this.serviceProvider = services.BuildServiceProvider();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			this.serviceProvider?.Dispose();
		}

		[Test]
		public void ShouldConfigureOptions()
		{
			MongoDbContext context = this.serviceProvider.GetRequiredService<MongoDbContext>();
			
			FieldInfo fieldInfo = typeof(MongoDbContext).GetField("options", BindingFlags.Instance | BindingFlags.NonPublic);
			MongoDbContextOptions value = (MongoDbContextOptions)fieldInfo.GetValue(context);

			value.ConnectionString.Should().Be(GlobalFixture.ConnectionString);
			value.DatabaseName.Should().Be(GlobalFixture.Database);
		}
	}
}
