namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using MadEyeMatt.MongoDB.DbContext.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class TimeOnlySerializationProvider : IBsonSerializationProvider
	{
		private static readonly TimeOnlySerializer Serializer = new TimeOnlySerializer();
		private static readonly NullableSerializer<TimeOnly> NullableSerializer = new NullableSerializer<TimeOnly>(new TimeOnlySerializer());

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
