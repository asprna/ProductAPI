using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductAPI.Domain.Entities
{
	public record Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[StringLength(50)]
		public string Name { get; set; }
		[StringLength(200)]
		public string Description { get; set; }
		public decimal Price { get; set; }
		public decimal DeliveryPrice { get; set; }
	}
}
