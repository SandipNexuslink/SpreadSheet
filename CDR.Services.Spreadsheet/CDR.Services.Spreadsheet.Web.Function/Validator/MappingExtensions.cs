using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CDR.Services.Spreadsheet.Web.Function.Validator
{

    public class MappingtableRequest<T>
    {
        /// <summary>
        /// The deserialized value of the request.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Whether or not the deserialized value was found to be valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The collection of validation errors.
        /// </summary>
        public IList<ValidationFailure> Errors { get; set; }
    }

    public static class MappingExtensions
    {
        /// <summary>
        /// Creates a <see cref="BadRequestObjectResult"/> containing a collection
        /// of minimal validation error details.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static BadRequestObjectResult ToBadRequest<T>(this MappingtableRequest<T> request)
        {
            return new BadRequestObjectResult(request.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Error = e.ErrorMessage
            }));
        }
    }
}
