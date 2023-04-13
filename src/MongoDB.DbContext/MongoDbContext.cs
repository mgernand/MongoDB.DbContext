// ReSharper disable UseMethodIsInstanceOfType

namespace MadEyeMatt.MongoDB.DbContext
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using global::MongoDB.Driver;
	using global::MongoDB.Driver.Linq;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	///     Represents a session with the database and can be used to query and save instances of your documents.
	/// </summary>
	[PublicAPI]
	public class MongoDbContext : IDisposable, IAsyncDisposable
	{
		private readonly object lockObject = new object();

		private readonly MongoDbContextOptions options;
		private bool initializing;
		private IServiceScope serviceScope;
		private Task<IClientSessionHandle> sessionTask;

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContext" /> type.
		///		The <see cref="OnConfiguring(MongoDbContextOptionsBuilder)" /> method will be called to configure
		///		the database to be used for this context.
		/// </summary>
		protected MongoDbContext()
			: this(new MongoDbContextOptions<MongoDbContext>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoDbContext" /> type using the given options.
		///     The <see cref="OnConfiguring(MongoDbContextOptionsBuilder)" /> method will still be called to allow further
		///     configuration of the options.
		/// </summary>
		/// <param name="options">The options for this context.</param>
		public MongoDbContext(MongoDbContextOptions options)
		{
			ArgumentNullException.ThrowIfNull(options);

			if(!options.ContextType.IsAssignableFrom(this.GetType()))
			{
				throw new InvalidOperationException($"The MongoDbContextOptions passed to the {this.GetType().Name} constructor must be a MongoDbContextOptions<{this.GetType().Name}>. When registering multiple MongoDbContext types, make sure that the constructor for each context type has a MongoDbContextOptions<TContext> parameter rather than a non-generic MongoDbContextOptions parameter.");
			}

			this.options = options;
		}

		private IServiceProvider InternalServiceProvider
		{
			get
			{
				if(this.initializing)
				{
					throw new InvalidOperationException("An attempt was made to use the context instance while it is being configured. A MongoDbContext instance cannot be used inside 'OnConfiguring' since it is still being configured at this point. This can happen if a second operation is started on this context instance before a previous operation completed. Any instance members are not guaranteed to be thread safe.");
				}

				if(this.serviceScope != null)
				{
					return this.serviceScope.ServiceProvider;
				}

				try
				{
					this.initializing = true;

					MongoDbContextOptionsBuilder optionsBuilder = new MongoDbContextOptionsBuilder(this.options);
					this.OnConfiguring(optionsBuilder);

					this.serviceScope = ServiceProviderCache.Instance.GetOrAdd(optionsBuilder.Options)
						.GetRequiredService<IServiceScopeFactory>()
						.CreateScope();

					return this.serviceScope.ServiceProvider;
				}
				finally
				{
					this.initializing = false;
				}
			}
		}

		/// <summary>
		///		Gets the client associated with this context.
		/// </summary>
		public IMongoClient Client => this.InternalServiceProvider.GetRequiredService<IMongoClient>();

		/// <summary>
		///		Gets the database associated with this context.
		/// </summary>
		public IMongoDatabase Database => this.InternalServiceProvider.GetRequiredService<IMongoDatabase>();

		/// <summary>
		///		Gets the session instance if one exists, else returns <c>null</c>.
		/// </summary>
		public IClientSessionHandle Session { get; private set; }

		/// <summary>
		///     Gets the collection for the given document type.
		/// </summary>
		/// <typeparam name="TDocument">The type representing a document.</typeparam>
		/// <returns>The collection.</returns>
		public IMongoCollection<TDocument> GetCollection<TDocument>()
		{
			return this.Database.GetCollection<TDocument>(this.GetCollectionName<TDocument>());
		}

		/// <summary>
		///     Gets the queryable of the collection for the given document type.
		/// </summary>
		/// <typeparam name="TDocument">The type representing a document.</typeparam>
		/// <returns>The queryable of the collection.</returns>
		public IMongoQueryable<TDocument> GetQueryable<TDocument>()
		{
			IMongoCollection<TDocument> collection = this.GetCollection<TDocument>();
			return collection.AsQueryable();
		}

		/// <summary>
		///     Starts a client session and transaction.
		/// </summary>
		/// <param name="clientSessionOptions">The session options.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A client session.</returns>
		public Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions clientSessionOptions = null, CancellationToken cancellationToken = default)
		{
			async Task<IClientSessionHandle> Start()
			{
				IClientSessionHandle handle = await this.Client.StartSessionAsync(clientSessionOptions, cancellationToken);

				this.Session = handle;

				return handle;
			}

			lock (this.lockObject)
			{
				if (this.sessionTask != null)
				{
					return this.sessionTask;
				}

				this.sessionTask = Start();

				return this.sessionTask;
			}
		}

		/// <summary>
		///     Returns the name of the collection for the given document type.
		/// </summary>
		/// <typeparam name="TDocument">The type representing a document.</typeparam>
		/// <returns>The name of the collection.</returns>
		public virtual string GetCollectionName<TDocument>()
		{
			return typeof(TDocument).Name;
		}

		/// <inheritdoc />
		public ValueTask DisposeAsync()
		{
			if (this.serviceScope != null)
			{
				if (this.serviceScope is IAsyncDisposable asyncDisposable)
				{
					return asyncDisposable.DisposeAsync();
				}

				this.serviceScope.Dispose();
			}

			return default;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.serviceScope?.Dispose();
		}

		/// <summary>
		///     Configures the database to be used for this context. This method is called for each instance
		///		of the context that is created.
		/// </summary>
		/// <remarks>
		///     <para>
		///         In situations where an instance of <see cref="MongoDbContextOptions" /> may or may not have been passed
		///         to the constructor, you can use <see cref="MongoDbContextOptionsBuilder.IsConfigured" /> to determine if
		///         the options have already been set, and skip some or all of the logic in
		///         <see cref="OnConfiguring(MongoDbContextOptionsBuilder)" />.
		///     </para>
		/// </remarks>
		/// <param name="builder">A builder used modify the options for this context.</param>
		protected internal virtual void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
		}
	}
}
