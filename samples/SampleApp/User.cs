namespace SampleApp
{
	using System;
	using MongoDB.Bson;

	public class User
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public Guid TenantId { get; set; } = Guid.NewGuid();
	}
}
