using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Wallet.Shared
{
    public static class JwtEventHandler
    {
        public static Task HandleAuthenticationFailed(AuthenticationFailedContext context)
        {
            context.NoResult();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(new
            {
                statusCode = context.Response.StatusCode,
                error = "Authentication failed.",
                details = context.Exception.Message
            });
        }

        public static Task OnChallenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    error = "You are not authorized."
                });
            }
            return Task.CompletedTask;
        }

        public static Task OnForbidden(ForbiddenContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(new
            {
                statusCode = context.Response.StatusCode,
                error = "You do not have permission to access this resource."
            });
        }
    }
}
