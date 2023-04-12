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
	public class DateOnlyTests
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			BsonSerializer.RegisterSerializer(new DateOnlySerializer());
		}

		private class TestClass
		{
			public DateOnly Property { get; set; }
		}

		[Test]
		public void ShouldDeserialize()
		{
			string json = @"{""property"" : ""2022-04-01""}";
			TestClass result = BsonSerializer.Deserialize<TestClass>(json);

			result.Should().NotBeNull();
			result.Property.Should().Be(new DateOnly(2022, 4, 1));
		}

		[Test]
		public void ShouldSerialize()
		{
			TestClass obj = new TestClass
			{
				Property = new DateOnly(2022, 4, 1),
			};

			string json = obj.ToJson(new JsonWriterSettings
			{
				Indent = true
			});

			Console.WriteLine(json);
			json.Should().Contain(@"""2022-04-01""");
		}
	}
}
