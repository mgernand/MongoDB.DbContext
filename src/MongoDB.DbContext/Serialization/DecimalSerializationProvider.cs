namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class DecimalSerializationProvider : IBsonSerializationProvider
	{
		private static readonly DecimalSerializer Serializer = new DecimalSerializer(BsonType.Decimal128);
		private static readonly NullableSerializer<decimal> NullableSerializer = new NullableSerializer<decimal>(Serializer);

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(decimal))
			{
				return Serializer;
			}

			if(type == typeof(decimal?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
