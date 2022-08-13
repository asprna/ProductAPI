using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Commands.AddProduct
{
	public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
	{
		public AddProductCommandValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty().WithMessage("Name is required")
				.NotNull()
				.MaximumLength(50).WithMessage("Name must not exceed 50 characters");

			RuleFor(p => p.Description)
				.MaximumLength(200).WithMessage("Description must not exceed 200 characters");

			RuleFor(p => p.Price)
				.NotEmpty().WithMessage("Price is required")
				.GreaterThan(0).WithMessage("Price should be greater than zero");
		}
	}
}
