using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ProductAPI.Application.Features.Products.Commands.AddProduct;
using ProductAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Controller.ProductControllerTest
{
	public class ProductPostTest
	{
		private readonly Mock<ProductController> sut;
		private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();

		public ProductPostTest()
		{
			sut = new Mock<ProductController>() { CallBase = true };
			sut.Protected().Setup<ISender>("Mediator").Returns(_mockMediator.Object);
		}

		[Fact]
		public async Task Post_ProductAdded_ReturnStatus200()
		{
			//Arrange
			var product = new AddProductCommand 
			{ 
				Name = "Apple",
				Description = "Fruit",
				DeliveryPrice = 1.00m,
				Price = 10.00m
			};

			//Act
			var result = await sut.Object.Post(product) as ObjectResult;

			//Assert
			Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
	}
}
