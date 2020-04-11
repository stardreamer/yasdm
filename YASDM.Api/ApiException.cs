using System;

namespace YASDM.Api
{
    public class ApiUnauthorizedException: ApiException
    {
        public ApiUnauthorizedException() : base("You are not authorized to perform this action") {}

        public ApiUnauthorizedException(string message) : base(message)
        {
        }
    }

        public class ApiRefreshTokenExpiredException: ApiException
    {
        public ApiRefreshTokenExpiredException() : base("This refresh token has expired") {}
    }

    public class ApiNotFoundException: ApiException
    {
        public ApiNotFoundException(string message = null) : base(message ?? "The requested resource was not found") {}
    }

    public class ApiException : Exception
    {
        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }
    }

}