using Microsoft.EntityFrameworkCore;
using ProductAPI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Test.Helper
{
	public class ProductTestBase : IDisposable 
	{
		protected readonly DataContext _context;

		public ProductTestBase()
		{
			var options = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new DataContext(options);

			_context.Database.EnsureCreated();

			ProductInitializer.Initializer(_context);
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();

			_context.Dispose();
		}
	}
}
