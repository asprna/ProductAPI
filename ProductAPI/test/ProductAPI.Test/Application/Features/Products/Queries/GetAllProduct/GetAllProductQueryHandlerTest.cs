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
			var products = new List<Product>
			{
				new Product
				{
					Id = 1,
					Name = "Apple",
					Description = "Fruit",
					DeliveryPrice = 1.00m,
					Price = 5.00m
				},
				new Product
				{
					Id = 2,
					Name = "Potato",
					Description = "Vegetable",
					DeliveryPrice = 1.00m,
					Price = 5.00m
				},
			};

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
			var products = new List<Product>();

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
