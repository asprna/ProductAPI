using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Queries.GetAllProduct
{
	public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<Product>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ILogger<GetAllProductQueryHandler> _logger;

		public GetAllProductQueryHandler(IApplicationDbContext context,
			ILogger<GetAllProductQueryHandler> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<List<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting all products");
			var products = await _context.Products.ToListAsync();

			_logger.LogInformation($"{products.Count} products found");
			return products;
		}
	}
}
