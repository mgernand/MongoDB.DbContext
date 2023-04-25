// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable CS1591

namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.CompilerServices;

	internal static class Argument
	{
		/// <summary>
		///     Throws an <see cref="ArgumentNullException" /> if <paramref name="argument" /> is null.
		/// </summary>
		/// <param name="argument">The reference type argument to validate as non-null.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
		public static void ThrowIfNull(object argument, [CallerArgumentExpression("argument")] string paramName = null)
		{
			if(argument is null)
			{
				Throw(paramName);
			}
		}

		/// <summary>
		///     Throws an exception if <paramref name="argument" /> is null or empty.
		/// </summary>
		/// <param name="argument">The string argument to validate as non-null and non-empty.</param>
		/// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argument" /> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="argument" /> is empty.</exception>
		public static void ThrowIfNullOrEmpty(string argument, [CallerArgumentExpression("argument")] string paramName = null)
		{
			if(string.IsNullOrEmpty(argument))
			{
				ThrowNullOrEmptyException(argument, paramName);
			}
		}

		private static void ThrowNullOrEmptyException(string argument, string paramName)
		{
			ThrowIfNull(argument, paramName);
			throw new ArgumentException("The value cannot be empty.", paramName);
		}

		private static void Throw(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}

#if NETSTANDARD
namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class CallerArgumentExpressionAttribute : Attribute
	{
		public CallerArgumentExpressionAttribute(string parameterName)
		{
			this.ParameterName = parameterName;
		}

		public string ParameterName { get; }
	}
}
#endif
