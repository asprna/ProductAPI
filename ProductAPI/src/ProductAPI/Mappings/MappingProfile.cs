using AutoMapper;
using ProductAPI.Application.Features.Products.Commands.AddProduct;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Product, AddProductCommand>().ReverseMap();
		}
	}
}
