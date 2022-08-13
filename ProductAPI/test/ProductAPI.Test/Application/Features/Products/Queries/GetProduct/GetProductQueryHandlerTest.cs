using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Application.Features.Products.Queries.GetProduct;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Queries.GetProduct
{
	public class GetProductQueryHandlerTest
	{
		private GetProductQueryHandler sut;
		private readonly Mock<IApplicationDbContext> _context = new Mock<IApplicationDbContext>();
		private readonly Mock<ILogger<GetProductQueryHandler>> _logger = new Mock<ILogger<GetProductQueryHandler>>();

		[Fact]
		public async Task Handler_ProductExists_ReturnProduct()
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
				}
			};

			var mockDbSet = products.AsQueryable().BuildMockDbSet();

			_context.Setup(db => db.Products).Returns(mockDbSet.Object);

			sut = new GetProductQueryHandler(_context.Object, _logger.Object);

			var request = new GetProductQuery { Id = 1 };

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Equal(products[0], result);
		}

		[Fact]
		public async Task Handler_ProductNotExists_ReturnNotFound()
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
				}
			};

			var mockDbSet = products.AsQueryable().BuildMockDbSet();

			_context.Setup(db => db.Products).Returns(mockDbSet.Object);

			sut = new GetProductQueryHandler(_context.Object, _logger.Object);

			var request = new GetProductQuery { Id = 2 };

			//Act & Assert
			NotFoundException result = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));
			Assert.Equal("Product not found", result.Message);
		}

	}
}
