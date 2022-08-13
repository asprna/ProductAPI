using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Application.Features.Products.Commands.AddProduct;
using ProductAPI.Domain.Entities;
using ProductAPI.Test.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Commands.AddProduct
{
	public class AddProductCommandHandlerTest : ProductTestBase
	{
		private AddProductCommandHandler sut;
		private readonly Mock<ILogger<AddProductCommandHandler>> _logger = new Mock<ILogger<AddProductCommandHandler>>();
		private readonly Mock<IApplicationDbContext> _contexMoq = new Mock<IApplicationDbContext>();
		private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

		[Fact]
		public async Task Handler_AddProduct_ReturnProductId()
		{
			//Arrange
			var request = new AddProductCommand
			{
				Name = "Roland FP10",
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};

			var product = new Product
			{
				Id = 0,
				Name = "Roland FP10",
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};

			var lastProductId = SeedProductData.Products.Select(p => p.Id).Max();

			_mapper.Setup(x => x.Map<Product>(request)).Returns(product);

			sut = new AddProductCommandHandler(_context, _logger.Object, _mapper.Object);

			//Act
			var result = await sut.Handle(request, CancellationToken.None);

			//Assert
			Assert.Equal(++lastProductId, result);
		}

		[Fact]
		public async Task Handler_UnableToProduct_ReturnDataContextException()
		{
			//Arrange
			var request = new AddProductCommand
			{
				Name = "Roland FP10",
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};

			var product = new Product
			{
				Id = 0,
				Name = "Roland FP10",
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};

			var lastProductId = SeedProductData.Products.Select(p => p.Id).Max();

			_mapper.Setup(x => x.Map<Product>(request)).Returns(product);
			_contexMoq.Setup(p => p.Products.AddAsync(product, default));
			_contexMoq.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(-1);

			sut = new AddProductCommandHandler(_contexMoq.Object, _logger.Object, _mapper.Object);

			//Act & Assert
			DataContextException result = await Assert.ThrowsAsync<DataContextException>(() => sut.Handle(request, CancellationToken.None));
			Assert.Equal("Unable to save changes to DB", result.Message);
		}
	}
}
