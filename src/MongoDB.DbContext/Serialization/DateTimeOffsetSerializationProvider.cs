namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class DateTimeOffsetSerializationProvider : IBsonSerializationProvider
	{
		private static readonly DateTimeOffsetSerializer Serializer = new DateTimeOffsetSerializer(BsonType.Document);
		private static readonly NullableSerializer<DateTimeOffset> NullableSerializer = new NullableSerializer<DateTimeOffset>(new DateTimeOffsetSerializer(BsonType.Document));

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(DateTimeOffset))
			{
				return Serializer;
			}

			if (type == typeof(DateTimeOffset?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
