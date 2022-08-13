using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Test.Helper
{
	public static class SeedProductData
	{
		public static readonly List<Product> Products = new List<Product>
		{
			new Product
			{
				Id = 1,
				Name = "Yamaha PSR E463",
				Description = "Yamaha E series Keyboard",
				Price = 499.99M,
				DeliveryPrice = 16.99M
			},
			new Product
			{
				Id = 2,
				Name = "Yamaha PSR S700",
				Description = "Yamaha S series Keyboard",
				Price = 2999.99M
			}
		};
	}
}
