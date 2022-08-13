using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Queries.GetProduct
{
	public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
	{
		public GetProductQueryValidator()
		{
			RuleFor(p => p.Id).GreaterThan(0).WithMessage("Invalid Product Id");
		}
	}
}
