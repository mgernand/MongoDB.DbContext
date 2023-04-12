namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class DateTimeSerializationProvider : IBsonSerializationProvider
	{
		private static readonly DateTimeSerializer Serializer = DateTimeSerializer.UtcInstance.WithRepresentation(BsonType.Document);
		private static readonly NullableSerializer<DateTime> NullableSerializer = new NullableSerializer<DateTime>(DateTimeSerializer.UtcInstance.WithRepresentation(BsonType.Document));

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(DateTime))
			{
				return Serializer;
			}

			if (type == typeof(DateTime?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
