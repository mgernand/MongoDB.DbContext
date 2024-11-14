namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using MadEyeMatt.MongoDB.DbContext.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class TimeOnlySerializationProvider : IBsonSerializationProvider
	{
		private static readonly CustomTimeOnlySerializer Serializer = new CustomTimeOnlySerializer();
		private static readonly NullableSerializer<TimeOnly> NullableSerializer = new NullableSerializer<TimeOnly>(Serializer);

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(TimeOnly))
			{
				return Serializer;
			}

			if (type == typeof(TimeOnly?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
