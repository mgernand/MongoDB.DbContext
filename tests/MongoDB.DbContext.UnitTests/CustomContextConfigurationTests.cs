namespace MongoDB.DbContext.UnitTests
{
	using System;
	using System.Reflection;
	using FluentAssertions;
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.Extensions.DependencyInjection;
	using MongoDB.Driver;
	using NUnit.Framework;

	[TestFixture]
	public class CustomContextConfigurationTests
	{
		[Test]
		public void ShouldConfigureOptions()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddMongoDbContext<CustomMongoDbContext>();
			IServiceProvider serviceProvider = services.BuildServiceProvider();

			CustomMongoDbContext context = serviceProvider.GetRequiredService<CustomMongoDbContext>();
			IMongoClient _ = context.Client;

			FieldInfo fieldInfo = typeof(MongoDbContext).GetField("options", BindingFlags.Instance | BindingFlags.NonPublic);
			MongoDbContextOptions value = (MongoDbContextOptions)fieldInfo.GetValue(context);

			value.ConnectionString.Should().Be(GlobalFixture.ConnectionString);
			value.DatabaseName.Should().Be("changed");
		}

		[Test]
		public void ShouldNotAllowSameContextTypeWithNonGenericOptions()
		{
			IServiceCollection services = new ServiceCollection();
			services.AddMongoDbContext<SampleContextOne>(optionsBuilder =>
			{
				optionsBuilder.UseDatabase("mongodb://localhost:27017", "test1");
			});

			services.AddMongoDbContext<SampleContextTwo>(optionsBuilder =>
			{
				optionsBuilder.UseDatabase("mongodb://localhost:27017", "test2");
			});
			IServiceProvider serviceProvider = services.BuildServiceProvider();

			Action action = () =>
			{
				SampleContextOne sampleContextOne = serviceProvider.GetRequiredService<SampleContextOne>();
				SampleContextTwo sampleContextTwo = serviceProvider.GetRequiredService<SampleContextTwo>();
			};

			action.Should().Throw<InvalidOperationException>();
		}
	}
}
