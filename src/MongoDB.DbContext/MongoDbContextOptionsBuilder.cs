namespace MadEyeMatt.MongoDB.DbContext
{
	using JetBrains.Annotations;
	using System;

	/// <summary>
	///     Provides the API for configuring <see cref="MongoDbContextOptions{TContext}" /> instances.
	/// </summary>
	/// <typeparam name="TContext">The type of context to be configured.</typeparam>
	[PublicAPI]
	public class MongoDbContextOptionsBuilder<TContext> : MongoDbContextOptionsBuilder
		where TContext : MongoDbContext
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContextOptionsBuilder{TContext}" /> type with no options set.
		/// </summary>
		public MongoDbContextOptionsBuilder()
			: this(new MongoDbContextOptions<TContext>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContextOptionsBuilder{TContext}" /> type to further configure
		///     a given <see cref="MongoDbContextOptions" />.
		/// </summary>
		/// <param name="options">The options to be configured.</param>
		public MongoDbContextOptionsBuilder(MongoDbContextOptions<TContext> options)
			: base(options)
		{
		}

		/// <summary>
		///     Gets the options being configured.
		/// </summary>
		public new virtual MongoDbContextOptions<TContext> Options => (MongoDbContextOptions<TContext>)base.Options;
	}

	/// <summary>
	///     Provides the API for configuring <see cref="MongoDbContextOptions" /> instances.
	/// </summary>
	[PublicAPI]
	public class MongoDbContextOptionsBuilder
	{
		private readonly MongoDbContextOptions options;

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContextOptionsBuilder" /> type with no options set.
		/// </summary>
		public MongoDbContextOptionsBuilder()
			: this(new MongoDbContextOptions<MongoDbContext>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContextOptionsBuilder" /> type to further configure
		///     a given <see cref="MongoDbContextOptions" />.
		/// </summary>
		/// <param name="options">The options to be configured.</param>
		public MongoDbContextOptionsBuilder(MongoDbContextOptions options)
		{
			ArgumentNullException.ThrowIfNull(options);

			this.options = options;
		}

		/// <summary>
		///     Gets the options being configured.
		/// </summary>
		public virtual MongoDbContextOptions Options => this.options;

		/// <summary>
		///     Gets a value indicating whether any options have been configured.
		/// </summary>
		/// <remarks>
		///     This can be useful when you have overridden <see cref="MongoDbContext.OnConfiguring(MongoDbContextOptionsBuilder)" />
		///		to configure the context, but in some cases you also externally provide options. This property can be used to determine
		///		if the options have already been set, and skip some or all of the logic in <see cref="MongoDbContext.OnConfiguring(MongoDbContextOptionsBuilder)" />.
		/// </remarks>
		public virtual bool IsConfigured => 
			!string.IsNullOrWhiteSpace(this.options.ConnectionString) && 
			!string.IsNullOrWhiteSpace(this.options.DatabaseName);
	}
}
