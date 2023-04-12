namespace MadEyeMatt.MongoDB.DbContext.Initialization
{
	using System.Threading.Tasks;
	using global::MongoDB.Bson;
	using global::MongoDB.Driver;
	using JetBrains.Annotations;

	/// <inheritdoc />
	[PublicAPI]
	public abstract class EnsureSchemaBase<TContext> : IEnsureSchema
		where TContext : MongoDbContext
	{
		private readonly TContext context;

		/// <summary>
		///		Initializes a new instance of the <see cref="EnsureSchemaBase{TContext}"/> type.
		/// </summary>
		/// <param name="context"></param>
		protected EnsureSchemaBase(TContext context)
		{
			this.context = context;
		}

		/// <inheritdoc />
		public abstract Task ExecuteAsync();

		/// <summary>
		///		Checks of the collection for the document already exist int eh database.
		/// </summary>
		/// <typeparam name="TDocument"></typeparam>
		/// <returns></returns>
		public async Task<bool> CollectionExistsAsync<TDocument>()
		{
			string collectionName = this.context.GetCollectionName<TDocument>();
			BsonDocument filter = new BsonDocument("name", collectionName);
			IAsyncCursor<BsonDocument> collections = await this.context.Database.ListCollectionsAsync(new ListCollectionsOptions
			{
				Filter = filter
			});

			return await collections.AnyAsync();
		}
	}
}
