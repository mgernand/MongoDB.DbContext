namespace MadEyeMatt.MongoDB.DbContext.Serialization
{
	using System;

	internal static class TypeExtensions
	{
		/// <summary>
		///     Gets the type without nullable if the type is a <see cref="T:System.Nullable`1" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static Type UnwrapNullableType(this Type type)
		{
			Type underlyingType = Nullable.GetUnderlyingType(type);
			return (object)underlyingType != null ? underlyingType : type;
		}

		/// <summary>
		///     Determines if the specified type is a nullable.
		/// </summary>
		/// <returns><c>true</c> if the specified type is a nullable; otherwise, <c>false</c>.</returns>
		/// <param name="type">Type.</param>
		public static bool IsNullable(this Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}
	}
}
