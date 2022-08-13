using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
	public class ProductController : ApiControllerBase
	{
		[HttpGet("{id}", Name = "Get Product Details")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(int id)
		{
			return Ok();
		}
	}
}
