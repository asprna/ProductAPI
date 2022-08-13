using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Features.Products.Queries.GetAllProduct;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Queries.GetAllProduct
{
	public class GetAllProductQueryHandlerTest
	{
		private GetAllProductQueryHandler sut;
		private readonly Mock<IApplicationDbContext> _context = new Mock<IApplicationDbContext>();
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

			var mockDbSet = products.AsQueryable().BuildMockDbSet();

			_context.Setup(db => db.Products).Returns(mockDbSet.Object);

			sut = new GetAllProductQueryHandler(_context.Object, _logger.Object);

			var request = new GetAllProductQuery();

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Equal(products.Count, result.Count);
		}

		[Fact]
		public async Task Handler_ProductNotExists_ReturnNotProduct()
		{
			//Arrange
			var products = new List<Product>();

			var mockDbSet = products.AsQueryable().BuildMockDbSet();

			_context.Setup(db => db.Products).Returns(mockDbSet.Object);

			sut = new GetAllProductQueryHandler(_context.Object, _logger.Object);

			var request = new GetAllProductQuery();

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Empty(result);
		}
	}
}
