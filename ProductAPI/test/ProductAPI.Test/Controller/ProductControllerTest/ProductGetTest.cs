using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ProductAPI.Application.Exceptions;
using ProductAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Controller.ProductControllerTest
{
	public class ProductGetTest
	{
		private readonly Mock<ProductController> sut;
		private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();

		public ProductGetTest()
		{
			sut = new Mock<ProductController>() { CallBase = true };
			sut.Protected().Setup<ISender>("Mediator").Returns(_mockMediator.Object);
		}

		[Fact]
		public async Task Get_ProductFound_ReturnStatus200()
		{
			//Arrange
			var productId = 1;

			//Act
			var result = await sut.Object.Get(productId) as ObjectResult;

			//Assert
			Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		}
	}
}
