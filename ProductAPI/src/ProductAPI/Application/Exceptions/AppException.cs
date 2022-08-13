using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProductAPI.Application.Exceptions
{
    /// <summary>
    /// Application specific exceptions.
    /// </summary>
    public abstract class AppException : Exception
    {
        public AppException(int statusCode, string title, string details) : base(details)
        {
            StatusCode = statusCode;
            Title = title;
            Details = details;
        }

        public AppException(int statusCode, string title, string details, Exception innerException) : base(details, innerException)
        {
            StatusCode = statusCode;
            Title = title;
            Details = details;
        }

        public int StatusCode { get; }
        public string Title { get; }
        public string Details { get; }
    }

    /// <summary>
    /// Return Http Not found when the entity is not found.
    /// </summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string details) :
            base((int)HttpStatusCode.NotFound, "Not Found", details)
        {
        }
    }

    /// <summary>
    /// Return internal server error when there is an issue with query execution on the database.
    /// </summary>
    public class DataContextException : AppException
    {
        public DataContextException(string details) :
            base((int)HttpStatusCode.InternalServerError, "Internal Server Error", details)
        {
        }
    }

    /// <summary>
    /// Return bad gateway request, when a http request failed. 
    /// </summary>
    public class HttpException : AppException
    {
        public HttpException(string details) :
            base((int)HttpStatusCode.BadGateway, "Bad Gateway", details)
        {
        }
    }

    /// <summary>
    /// Return internal server error when unexpected behaviour happens.
    /// </summary>
    public class UnhandleException : AppException
    {
        public UnhandleException(string details) :
            base((int)HttpStatusCode.InternalServerError, "Internal Server Error", details)
        {
        }
    }
}
