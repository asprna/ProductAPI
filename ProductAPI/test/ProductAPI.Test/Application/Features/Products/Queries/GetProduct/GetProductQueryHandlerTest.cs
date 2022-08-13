using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Application.Features.Products.Queries.GetProduct;
using ProductAPI.Domain.Entities;
using ProductAPI.Test.Extensions;
using ProductAPI.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Queries.GetProduct
{
	public class GetProductQueryHandlerTest : ProductTestBase
	{
		private GetProductQueryHandler sut;
		private readonly Mock<ILogger<GetProductQueryHandler>> _logger = new Mock<ILogger<GetProductQueryHandler>>();
		private readonly IMemoryCache _cache;

		public GetProductQueryHandlerTest()
		{
			var services = new ServiceCollection();
			services.AddMemoryCache();
			var serviceProvider = services.BuildServiceProvider();
			var memoryCache = serviceProvider.GetService<IMemoryCache>();
			_cache = memoryCache;
		}

		[Fact]
		public async Task Handler_ProductExists_ReturnProduct()
		{
			//Arrange
			sut = new GetProductQueryHandler(_context, _logger.Object, _cache);

			var request = new GetProductQuery { Id = 1 };

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Equal(SeedProductData.Products.Where(x => x.Id == 1).FirstOrDefault(), result);
		}

		[Fact]
		public async Task Handler_ProductNotExists_ReturnNotFound()
		{
			//Arrange
			sut = new GetProductQueryHandler(_context, _logger.Object, _cache);

			var request = new GetProductQuery { Id = 3 };

			//Act & Assert
			NotFoundException result = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));
			Assert.Equal("Product not found", result.Message);
		}

		[Fact]
		public async Task Handler_ProductViaMemoryCache_ReturnProduct()
		{
			//Arrange
			var id = 1;
			sut = new GetProductQueryHandler(_context, _logger.Object, _cache);

			var request = new GetProductQuery { Id = id };

			//Act

			_cache.Set($"productapi_{id}", SeedProductData.Products.Where(p => p.Id == id).FirstOrDefault());

			var result1 = await sut.Handle(request, CancellationToken.None);

			//Assert
			_logger.VerifyLog(LogLevel.Information, "Get products from db", Times.Never());
		}

		[Fact]
		public async Task Handler_ProductNotViaMemoryCache_ReturnProduct()
		{
			//Arrange
			var id = 1;
			sut = new GetProductQueryHandler(_context, _logger.Object, _cache);

			var request = new GetProductQuery { Id = id };

			//Act
			await sut.Handle(request, CancellationToken.None);
			_cache.TryGetValue<Product>($"productapi_{id}", out var product);

			//Assert
			_logger.VerifyLog(LogLevel.Information, "Get products from db");
			Assert.Equal(SeedProductData.Products.Where(p => p.Id == id).FirstOrDefault(), product);
		}

	}
}
