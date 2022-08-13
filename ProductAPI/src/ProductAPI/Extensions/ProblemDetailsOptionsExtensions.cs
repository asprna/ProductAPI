using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ProductAPI.Extensions
{
    /// <summary>
    /// Extension for the ProblemDetails.
    /// </summary>
    public static class ProblemDetailsOptionsExtensions
    {
        /// <summary>
        /// Configuration for the fluent validation.
        /// </summary>
        /// <param name="options"></param>
        public static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                return factory.CreateValidationProblemDetails(ctx, errors);
            });
    }
}
