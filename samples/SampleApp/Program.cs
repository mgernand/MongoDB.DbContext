namespace SampleApp
{
	using System;
	using System.Threading.Tasks;
	using MadEyeMatt.MongoDB.DbContext;
	using Microsoft.Extensions.DependencyInjection;
	using MongoDB.Driver;

	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			IServiceCollection services = new ServiceCollection();

			services.AddLogging();
			services.AddOptions();

			services.AddMongoDbContext<SampleContextOne>(optionsBuilder =>
			{
				optionsBuilder.UseDatabase("mongodb://localhost:27017", "test1");
			});

			services.AddMongoDbContext<SampleContextTwo>(optionsBuilder =>
			{
				optionsBuilder.UseDatabase("mongodb://localhost:27017", "test2");
			});

			IServiceProvider serviceProvider = services.BuildServiceProvider();

			using(IServiceScope serviceScope = serviceProvider.CreateAsyncScope())
			{
				SampleContextOne sampleContextOne = serviceScope.ServiceProvider.GetRequiredService<SampleContextOne>();
				IMongoCollection<User> collection = sampleContextOne.GetCollection<User>();

				await collection.InsertOneAsync(new User(){ Name = "Tester" });
			}
			
			//SampleContextTwo sampleContextTwo = serviceProvider.GetRequiredService<SampleContextTwo>();
			//IMongoCollection<User> collectionTwo = sampleContextTwo.GetCollection<User>();
		}
	}
}
