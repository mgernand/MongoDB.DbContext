namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using MadEyeMatt.MongoDB.DbContext.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class DateOnlySerializationProvider : IBsonSerializationProvider
	{
		private static readonly DateOnlySerializer Serializer = new DateOnlySerializer();
		private static readonly NullableSerializer<DateOnly> NullableSerializer = new NullableSerializer<DateOnly>(new DateOnlySerializer());

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(DateOnly))
			{
				return Serializer;
			}

			if(type == typeof(DateOnly?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}