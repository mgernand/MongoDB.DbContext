#if !NETSTANDARD
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
	public class TimeOnlySerializer : StructSerializerBase<TimeOnly>
	{
		private readonly TimeSpanSerializer timeSpanSerializer = new TimeSpanSerializer();

		/// <inheritdoc />
		public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
		{
			this.timeSpanSerializer.Serialize(context, args, value.ToTimeSpan());
		}

		/// <inheritdoc />
		public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			TimeSpan timeSpan = this.timeSpanSerializer.Deserialize(context, args);
			return TimeOnly.FromTimeSpan(timeSpan);
		}
	}
}
#endif