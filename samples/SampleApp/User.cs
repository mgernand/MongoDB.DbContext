namespace SampleApp
{
	using MongoDB.Bson;

	public class User
	{
		public ObjectId Id { get; set; }

		public string Name { get; set; }
	}
}
