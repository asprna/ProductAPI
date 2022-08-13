using FluentValidation.TestHelper;
using ProductAPI.Application.Features.Products.Commands.AddProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Commands.AddProduct
{
	public class AddProductCommandValidatorTest
	{
		private readonly AddProductCommandValidator _getAddProductCommandValidator;

		public AddProductCommandValidatorTest()
		{
			_getAddProductCommandValidator = new AddProductCommandValidator();
		}

		[Fact]
		public void AddProductCommandValidator_NameIsEmpty_ValidationError()
		{
			//Arrange
			var getAddProductCommand = new AddProductCommand 
			{
				Name = null,
				Description = "Description",
				DeliveryPrice = 1.00m,
				Price = 10.00m
			};

			//Act
			var result = _getAddProductCommandValidator.TestValidate(getAddProductCommand);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Name)
				.WithErrorMessage("Name is required");
		}

		[Fact]
		public void AddProductCommandValidator_NameLongerThan50_ValidationError()
		{
			//Arrange
			var getAddProductCommand = new AddProductCommand
			{
				Name = "Appleeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
				Description = "Description",
				DeliveryPrice = 1.00m,
				Price = 10.00m
			};

			//Act
			var result = _getAddProductCommandValidator.TestValidate(getAddProductCommand);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Name)
				.WithErrorMessage("Name must not exceed 50 characters");
		}

		[Fact]
		public void AddProductCommandValidator_DescriptionLongerThan200_ValidationError()
		{
			//Arrange
			var getAddProductCommand = new AddProductCommand
			{
				Name = "Apple",
				Description = "Descriptionnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn",
				DeliveryPrice = 1.00m,
				Price = 10.00m
			};

			//Act
			var result = _getAddProductCommandValidator.TestValidate(getAddProductCommand);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Description)
				.WithErrorMessage("Description must not exceed 200 characters");
		}

		[Fact]
		public void AddProductCommandValidator_PriceNull_ValidationError()
		{
			//Arrange
			var getAddProductCommand = new AddProductCommand
			{
				Name = "Apple",
				Description = "Description",
				DeliveryPrice = 1.00m
			};

			//Act
			var result = _getAddProductCommandValidator.TestValidate(getAddProductCommand);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Price)
				.WithErrorMessage("Price is required");
		}

		[Fact]
		public void AddProductCommandValidator_PriceLessThan0_ValidationError()
		{
			//Arrange
			var getAddProductCommand = new AddProductCommand
			{
				Name = "Apple",
				Description = "Description",
				DeliveryPrice = 1.00m,
				Price = -0.99m
			};

			//Act
			var result = _getAddProductCommandValidator.TestValidate(getAddProductCommand);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Price)
				.WithErrorMessage("Price should be greater than zero");
		}
	}
}
