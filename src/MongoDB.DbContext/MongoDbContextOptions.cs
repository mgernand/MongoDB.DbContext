namespace MadEyeMatt.MongoDB.DbContext
{
	using JetBrains.Annotations;
	using System;

	/// <summary>
	///		The options used by a <see cref="MongoDbContext" />.
	/// </summary>
	[PublicAPI]
	public abstract class MongoDbContextOptions : IEquatable<MongoDbContextOptions>
	{
		/// <summary>
		///		The connection string for the MongoDB server/replica-set
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		///		The name of the database.
		/// </summary>
		public string DatabaseName { get; set; }

		/// <summary>
		///     The type of context that these options are for.
		/// </summary>
		public abstract Type ContextType { get; }

		/// <inheritdoc />
		public bool Equals(MongoDbContextOptions other)
		{
			if(ReferenceEquals(null, other))
			{
				return false;
			}

			if(ReferenceEquals(this, other))
			{
				return true;
			}

			return 
				string.Equals(this.ConnectionString, other.ConnectionString, StringComparison.InvariantCulture) && 
				string.Equals(this.DatabaseName, other.DatabaseName, StringComparison.InvariantCulture) &&
				Type.Equals(this.ContextType, other.ContextType);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj))
			{
				return false;
			}

			if(ReferenceEquals(this, obj))
			{
				return true;
			}

			if(obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((MongoDbContextOptions)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			HashCode hashCode = new HashCode();
			hashCode.Add(this.ConnectionString, StringComparer.InvariantCulture);
			hashCode.Add(this.DatabaseName, StringComparer.InvariantCulture);
			hashCode.Add(this.ContextType);
			return hashCode.ToHashCode();
		}

		public static bool operator ==(MongoDbContextOptions left, MongoDbContextOptions right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(MongoDbContextOptions left, MongoDbContextOptions right)
		{
			return !Equals(left, right);
		}
	}

	/// <summary>
	///		The options to be used by a <see cref="MongoDbContext" />.
	/// </summary>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	[PublicAPI]
	public class MongoDbContextOptions<TContext> : MongoDbContextOptions
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContextOptions{TContext}" /> class. You normally override
		///     <see cref="MongoDbContext.OnConfiguring(MongoDbContextOptionsBuilder)" /> or use a <see cref="MongoDbContextOptionsBuilder{TContext}" />
		///     to create instances of this class and it is not designed to be directly constructed in your application code.
		/// </summary>
		public MongoDbContextOptions()
		{
		}

		/// <inheritdoc />
		public override Type ContextType => typeof(TContext);
	}
}
