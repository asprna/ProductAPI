using MediatR;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Queries.GetProduct
{
	public class GetProductQuery : IRequest<Product>
	{
		public int Id { get; set; }
	}
}
