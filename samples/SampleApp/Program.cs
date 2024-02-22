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

			services.AddMongoDbContext<SampleContext>(builder =>
			{
				builder
					.UseDatabase("mongodb://localhost:27017", "test")
					.EnableTelemetry();
			});

			IServiceProvider serviceProvider = services.BuildServiceProvider();

			using(IServiceScope serviceScope = serviceProvider.CreateAsyncScope())
			{
				SampleContext sampleContextOne = serviceScope.ServiceProvider.GetRequiredService<SampleContext>();
				IMongoCollection<User> collection = sampleContextOne.GetCollection<User>();

				await collection.InsertOneAsync(new User
				{
					Name = "Tester"
				});

				User user = await collection.Find(x => x.Name == "Tester").FirstOrDefaultAsync();
				Console.WriteLine($"ID = {user.Id}, Name = {user.Name}, TenantID = {user.TenantId}");
			}

			Console.WriteLine("Press any key to quit...");
			Console.ReadKey();
		}
	}
}
