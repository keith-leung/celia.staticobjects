using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BR.MsgHub.OpenApi.Models
{
    public class OpenApiRequestHandler : AuthorizationHandler<OpenApiRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, OpenApiRequirement requirement)
        {  
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
