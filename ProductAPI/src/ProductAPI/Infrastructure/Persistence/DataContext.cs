using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Infrastructure.Persistence
{
	public class DataContext : DbContext, IApplicationDbContext
	{
		public DataContext(DbContextOptions<DataContext> options)
			: base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
	}
}
