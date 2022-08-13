using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProductAPI.Application.Behaviours;
using ProductAPI.Application.Contracts.Persistence;
using ProductAPI.Application.Exceptions;
using ProductAPI.Extensions;
using ProductAPI.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductAPI
{
	public class Startup
	{
		private IConfiguration _config;

		public Startup(IConfiguration configuration)
		{
			_config = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//Register DataContext
			services.AddDbContext<DataContext>(opt =>
			{
				opt.UseSqlite(_config.GetConnectionString("DefaultConnection"));
			});

			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DataContext>());

			services.AddMediatR(Assembly.GetExecutingAssembly());
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddControllers();

			services.AddProblemDetails(ConfigureProblemDetails);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductAPI", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//Global Exception Handling Middleware
			app.UseProblemDetails();

			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void ConfigureProblemDetails(ProblemDetailsOptions options)
		{
			// Custom mapping function for FluentValidation's ValidationException.
			options.MapFluentValidationException();

			options.IncludeExceptionDetails = (ctx, ex) =>
			{
				// Fetch services from HttpContext.RequestServices
				var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
				return env.IsDevelopment(); //&& !(ex is ValidationException);
			};

			options.ShouldLogUnhandledException = (context, ex, problem) => false;
			options.GetTraceId = ctx => null;

			options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
			options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

			options.Map<AppException>(ex => new ProblemDetails
			{
				Title = ex.Title,
				Status = ex.StatusCode,
				Detail = ex.Details
			});
		}
	}
}
