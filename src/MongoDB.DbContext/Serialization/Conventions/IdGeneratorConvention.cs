namespace MadEyeMatt.MongoDB.DbContext.Serialization.Conventions
{
	using System;
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Conventions;
	using global::MongoDB.Bson.Serialization.IdGenerators;
	using global::MongoDB.Bson.Serialization.Serializers;

	internal sealed class IdGeneratorConvention : ConventionBase, IPostProcessingConvention
	{
		/// <inheritdoc />
		public void PostProcess(BsonClassMap classMap)
		{
			BsonMemberMap idMemberMap = classMap.IdMemberMap;
			if(idMemberMap == null || idMemberMap.IdGenerator != null)
			{
				return;
			}

			Type idMemberType = Nullable.GetUnderlyingType(idMemberMap.MemberType) ?? idMemberMap.MemberType;
			if(idMemberType == typeof(string))
			{
				idMemberMap
					.SetIdGenerator(StringObjectIdGenerator.Instance)
					.SetSerializer(new StringSerializer(BsonType.ObjectId));
			}
			else if(idMemberType == typeof(Guid))
			{
				idMemberMap
					.SetIdGenerator(CombGuidGenerator.Instance)
					.SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
			}
			else if(idMemberType == typeof(ObjectId))
			{
				idMemberMap
					.SetIdGenerator(ObjectIdGenerator.Instance)
					.SetSerializer(ObjectIdSerializer.Instance);
			}
		}
	}
}
