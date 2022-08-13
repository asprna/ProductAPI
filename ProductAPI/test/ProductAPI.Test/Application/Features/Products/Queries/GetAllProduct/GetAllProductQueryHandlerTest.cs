using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Features.Products.Queries.GetAllProduct;
using ProductAPI.Domain.Entities;
using ProductAPI.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Queries.GetAllProduct
{
	public class GetAllProductQueryHandlerTest : ProductTestBase
	{
		private GetAllProductQueryHandler sut;
		private readonly Mock<ILogger<GetAllProductQueryHandler>> _logger = new Mock<ILogger<GetAllProductQueryHandler>>();

		[Fact]
		public async Task Handler_ProductExists_ReturnAllProduct()
		{
			//Arrange
			sut = new GetAllProductQueryHandler(_context, _logger.Object);

			var request = new GetAllProductQuery();

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Equal(SeedProductData.Products.Count, result.Count);
		}

		[Fact]
		public async Task Handler_ProductNotExists_ReturnNotProduct()
		{
			//Arrange
			var product = _context.Set<Product>();
			_context.Products.RemoveRange(product);
			await _context.SaveChangesAsync(CancellationToken.None);

			sut = new GetAllProductQueryHandler(_context, _logger.Object);

			var request = new GetAllProductQuery();

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Empty(result);
		}
	}
}
