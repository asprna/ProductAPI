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

namespace ProductAPI.Application.Features.Products.Queries.GetProduct
{
	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
	{
		private readonly IApplicationDbContext _context;
		private readonly ILogger<GetProductQueryHandler> _logger;

		public GetProductQueryHandler(IApplicationDbContext context,
			ILogger<GetProductQueryHandler> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Find the correct product");
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

			if (product == null)
			{
				_logger.LogInformation("Unable find the product");
				throw new NotFoundException("Product not found");
			}

			_logger.LogInformation("Getting product details - Success");
			return product;
		}
	}
}
