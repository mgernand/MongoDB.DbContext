namespace MongoDB.DbContext.UnitTests.Serialization
{
	using System;
	using FluentAssertions;
	using MadEyeMatt.MongoDB.DbContext.Serialization.Serializers;
	using MongoDB.Bson;
	using MongoDB.Bson.IO;
	using MongoDB.Bson.Serialization;
	using NUnit.Framework;

	[TestFixture]
	public class TimeOnlyTests
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			BsonSerializer.RegisterSerializer(new CustomTimeOnlySerializer());
		}

		private class TestClass
		{
			public TimeOnly Property { get; set; }
		}

		[Test]
		public void ShouldDeserialize()
		{
			string json = @"{""property"" : ""10:45:35.8471835""}";
			TestClass result = BsonSerializer.Deserialize<TestClass>(json);

			result.Should().NotBeNull();
			result.Property.Should().Be(TimeOnly.Parse("10:45:35.8471835"));
		}

		[Test]
		public void ShouldSerialize()
		{
			TestClass obj = new TestClass
			{
				Property = TimeOnly.Parse("10:45:35.8471835")
			};

			string json = obj.ToJson(new JsonWriterSettings
			{
				Indent = true
			});

			Console.WriteLine(json);
			json.Should().Contain(@"""10:45:35.8471835""");
		}
	}
}
