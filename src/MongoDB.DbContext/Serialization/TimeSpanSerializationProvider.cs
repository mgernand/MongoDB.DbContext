namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class TimeSpanSerializationProvider : IBsonSerializationProvider
	{
		private static readonly TimeSpanSerializer Serializer = new TimeSpanSerializer();
		private static readonly NullableSerializer<TimeSpan> NullableSerializer = new NullableSerializer<TimeSpan>(Serializer);

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if (type == typeof(TimeSpan))
			{
				return Serializer;
			}

			if (type == typeof(TimeSpan?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
