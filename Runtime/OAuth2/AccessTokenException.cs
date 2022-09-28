﻿using System;
using System.Net;

namespace Cdm.Authentication.OAuth2
{
    /// <summary>
    /// Access token response exception which is thrown in case of receiving a token error when an authorization code
    /// or an access token is expected.
    /// </summary>
    public class AccessTokenException : Exception
    {
        /// <summary>
        /// HTTP status code of error, or null if unknown.
        /// </summary>
        public HttpStatusCode? statusCode { get; }
        
        /// <summary>
        /// The error information.
        /// </summary>
        public AccessTokenError error { get; }

        public AccessTokenException(AccessTokenError error, HttpStatusCode? statusCode) : base(error.description)
        {
            this.error = error;
            this.statusCode = statusCode;
        }

        public AccessTokenException(AccessTokenError error, HttpStatusCode? statusCode, string message) : base(message)
        {
            this.error = error;
            this.statusCode = statusCode;
        }

        public AccessTokenException(AccessTokenError error, HttpStatusCode? statusCode,
            string message, Exception innerException) : base(message, innerException)
        {
            this.error = error;
            this.statusCode = statusCode;
        }
    }
}