namespace MadEyeMatt.MongoDB.DbContext.Serialization.Serializers
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using JetBrains.Annotations;

	/// <summary>
	///     A serializer for the <see cref="DateOnly" /> type.
	/// </summary>
	[PublicAPI]
	public class CustomDateOnlySerializer : StructSerializerBase<DateOnly>
	{
		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
		{
			context.Writer.WriteString(value.ToString("yyyy-MM-dd"));
		}

		/// <inheritdoc />
		public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			string dateString = context.Reader.ReadString();
			return DateOnly.ParseExact(dateString, "yyyy-MM-dd", null);
		}
	}
}
