using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductAPI.Application.Contracts.Persistence
{
	public interface IApplicationDbContext
	{
		DbSet<Product> Products { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
