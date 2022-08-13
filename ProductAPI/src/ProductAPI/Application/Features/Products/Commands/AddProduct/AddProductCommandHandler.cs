using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductAPI.Application.Features.Products.Commands.AddProduct
{
	/// <summary>
	/// Add Product
	/// </summary>
	public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
	{
		private readonly IApplicationDbContext _context;
		private readonly ILogger<AddProductCommandHandler> _logger;
		private readonly IMapper _mapper;

		public AddProductCommandHandler(IApplicationDbContext context,
			ILogger<AddProductCommandHandler> logger,
			 IMapper mapper)
		{
			_context = context;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Creating new product");
			
			var product = _mapper.Map<Product>(request);
			var newProduct = await _context.Products.AddAsync(product);

			var result = await _context.SaveChangesAsync(cancellationToken) > 0;

			if (!result)
			{
				_logger.LogInformation("Unable to save changes to DB");
				throw new DataContextException("Unable to save changes to DB");
			}

			_logger.LogInformation($"Product Id {newProduct.Entity.Id} is successfully created");

			return newProduct.Entity.Id;
		}
	}
}
