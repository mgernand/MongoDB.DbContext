﻿namespace SampleApp
{
	using MadEyeMatt.MongoDB.DbContext;

	public class SampleContextTwo : MongoDbContext
	{
		public SampleContextTwo(MongoDbContextOptions<SampleContextTwo> options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected override void OnConfiguring(MongoDbContextOptionsBuilder builder)
		{
			base.OnConfiguring(builder);
		}
	}
}
