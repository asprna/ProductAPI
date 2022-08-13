using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Queries.GetProduct
{
	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
	{
		private readonly IApplicationDbContext _context;
		private readonly ILogger<GetProductQueryHandler> _logger;
		private readonly IMemoryCache _cache;
		private MemoryCacheEntryOptions cacheOptions;

		public GetProductQueryHandler(IApplicationDbContext context,
			ILogger<GetProductQueryHandler> logger,
			IMemoryCache cache)
		{
			_context = context;
			_logger = logger;
			_cache = cache;

			cacheOptions = new MemoryCacheEntryOptions()
					.SetSlidingExpiration(TimeSpan.FromSeconds(10));
		}

		public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var product = new Product();

			_logger.LogInformation("Check if product available in memory cache");
			if (!_cache.TryGetValue<Product>($"productapi_{request.Id}", out product))
			{
				_logger.LogInformation("Get products from db");

				product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

				if (product == null)
				{
					_logger.LogInformation("Unable find the product");
					throw new NotFoundException("Product not found");
				}

				_cache.Set<Product>($"productapi_{product.Id}", product, cacheOptions);
			}
				
			_logger.LogInformation("Getting product details - Success");
			return product;
		}
	}
}
