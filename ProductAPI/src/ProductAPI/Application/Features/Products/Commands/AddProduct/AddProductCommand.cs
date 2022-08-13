using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Commands.AddProduct
{
	public class AddProductCommand : IRequest<int>
	{
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[StringLength(200)]
		public string Description { get; set; }
		[Required]
		public decimal Price { get; set; }
		public decimal DeliveryPrice { get; set; }
	}
}
