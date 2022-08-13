using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductAPI.Application.Behaviours
{
	public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
	{
		private readonly ILogger<TRequest> _logger;

		public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var requestName = typeof(TRequest).Name;

			try
			{
				return await next();
			}
			catch (AppException ex)
			{
				_logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName} {request}");
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName} {request}");
				throw new UnhandleException(ex.Message);
			}
		}
	}
}
