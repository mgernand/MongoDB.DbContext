namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;

	/// <inheritdoc />
	internal sealed class GuidSerializationProvider : IBsonSerializationProvider
	{
		private static readonly GuidSerializer Serializer = new GuidSerializer(GuidRepresentation.Standard);
		private static readonly NullableSerializer<Guid> NullableSerializer = new NullableSerializer<Guid>(Serializer);

		/// <inheritdoc />
		public IBsonSerializer GetSerializer(Type type)
		{
			if(type == typeof(Guid))
			{
				return Serializer;
			}

			if(type == typeof(Guid?))
			{
				return NullableSerializer;
			}

			// Fall back to MongoDB defaults.
			return null;
		}
	}
}
