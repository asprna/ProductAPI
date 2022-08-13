using ProductAPI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Test.Helper
{
	public class ProductInitializer
	{
		public static void Initializer(DataContext context)
		{
			if (context.Products.Any())
			{
				return;
			}

			Seed(context);
		}

		private static void Seed(DataContext context)
		{
			context.Products.AddRange(SeedProductData.Products);
			context.SaveChanges();
		}
	}
}
