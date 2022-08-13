using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ProductAPI.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class ApiControllerBase : ControllerBase
	{
		private ISender _mediator;
		protected virtual ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
	}
}
