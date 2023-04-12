namespace SampleApp
{
	using MongoDB.Bson;

	public class Tenant
	{
		public ObjectId Id { get; set; }

		public string Name { get; set; }
	}
}
