namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using MadEyeMatt.MongoDB.DbContext.Serialization.Conventions;

	internal static class SerializationManager
	{
		static SerializationManager()
		{
			BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DecimalSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DateOnlySerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new TimeOnlySerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DateTimeSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new DateTimeOffsetSerializationProvider());
			BsonSerializer.RegisterSerializationProvider(new TimeSpanSerializationProvider());

			ConventionPack conventionPack =
			[
				new NamedIdMemberConvention("ID", "Id", "id", "_id"),
				new IdGeneratorConvention(),
				new CamelCaseElementNameConvention(),
				new EnumRepresentationConvention(BsonType.String)
			];

			// Register the default convention for every document type.
			ConventionRegistry.Register("DefaultConventions", conventionPack, _ => true);
		}

		internal static void Initialize()
		{
		}
	}
}
