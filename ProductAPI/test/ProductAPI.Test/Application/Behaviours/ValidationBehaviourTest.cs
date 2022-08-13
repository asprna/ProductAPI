using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Moq.AutoMock;
using ProductAPI.Application.Behaviours;
using ProductAPI.Application.Features.Products.Commands.AddProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Application.Behaviours
{
	public class ValidationBehaviourTest
	{
		[Fact]
		public async Task ValidationBehaviour_RequestHasValidationErrors_ReturnValidationException()
		{
			//Arrange
			var mocker = new AutoMocker();
			var sut = mocker.CreateInstance<ValidationBehaviour<AddProductCommand, int>>();
			var validator = mocker.GetMock<IValidator<AddProductCommand>>();

			var request = new AddProductCommand 
			{
				Name = string.Empty,
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};
			var validationError = new ValidationFailure("Name", "Name is required");
			var validationResult = new ValidationResult { Errors = { validationError } };
			
			validator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<AddProductCommand>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

			var mockPipelineBehaviourDelegate = new Mock<RequestHandlerDelegate<int>>();
			mockPipelineBehaviourDelegate.Setup(m => m()).ReturnsAsync(It.IsAny<int>());

			//Act & Assert
			ValidationException result = await Assert.ThrowsAsync<ValidationException>(() => sut.Handle(request, CancellationToken.None, mockPipelineBehaviourDelegate.Object));
			Assert.Single(result.Errors);
		}

		[Fact]
		public void ValidationBehaviour_RequestHasNoValidationErrors_ReturnValidationResult()
		{
			//Arrange
			var mocker = new AutoMocker();
			var sut = mocker.CreateInstance<ValidationBehaviour<AddProductCommand, int>>();
			var validator = mocker.GetMock<IValidator<AddProductCommand>>();

			var request = new AddProductCommand 
			{
				Name = "Roland FP10",
				Description = "Roland FP10 Portable Digital Piano",
				DeliveryPrice = 819.00m,
				Price = 50.00m
			};
			
			var validationResult = new ValidationResult();
			var response = 1;

			validator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<AddProductCommand>>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(validationResult));

			var mockPipelineBehaviourDelegate = new Mock<RequestHandlerDelegate<int>>();
			mockPipelineBehaviourDelegate.Setup(m => m()).ReturnsAsync(response);

			//Act
			var result = sut.Handle(request, CancellationToken.None, mockPipelineBehaviourDelegate.Object);

			//Assert
			Assert.Equal(response, result.Result);
		}
	}
}
