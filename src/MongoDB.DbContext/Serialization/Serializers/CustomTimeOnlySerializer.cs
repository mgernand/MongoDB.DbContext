namespace MadEyeMatt.MongoDB.DbContext.Serialization.Serializers
{
	using System;
	using global::MongoDB.Bson.Serialization;
	using global::MongoDB.Bson.Serialization.Serializers;
	using JetBrains.Annotations;

	/// <summary>
	///     A serializer for the <see cref="TimeOnly" /> type.
	/// </summary>
	[PublicAPI]
	public class CustomTimeOnlySerializer : StructSerializerBase<TimeOnly>
	{
		private readonly TimeSpanSerializer timeSpanSerializer = new TimeSpanSerializer();

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
		{
			TimeSpan timeSpanValue = value.ToTimeSpan();
			this.timeSpanSerializer.Serialize(context, args, timeSpanValue);
		}

		/// <inheritdoc />
		public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			TimeSpan timeSpan = this.timeSpanSerializer.Deserialize(context, args);
			return TimeOnly.FromTimeSpan(timeSpan);
		}
	}
}
