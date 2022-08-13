using FluentValidation.TestHelper;
using ProductAPI.Application.Features.Products.Queries.GetProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPI.Test.Features.Products.Queries.GetProduct
{
	public class GetProductQueryValidatorTest
	{
		private readonly GetProductQueryValidator _getProductQueryValidator;

		public GetProductQueryValidatorTest()
		{
			_getProductQueryValidator = new GetProductQueryValidator();
		}

		[Fact]
		public void GetProductQueryValidator_LessThanOne_ValidationError()
		{
			//Arrange
			var getProductQuery = new GetProductQuery { Id = 0 };

			//Act
			var result = _getProductQueryValidator.TestValidate(getProductQuery);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Id)
				.WithErrorMessage("Invalid Product Id");
		}

		[Fact]
		public void GetProductQueryValidator_GreaterThanOne_NoValidationError()
		{
			//Arrange
			var getProductQuery = new GetProductQuery { Id = 1 };

			//Act
			var result = _getProductQueryValidator.TestValidate(getProductQuery);

			//Assert
			result.ShouldNotHaveAnyValidationErrors();
		}
	}
}
