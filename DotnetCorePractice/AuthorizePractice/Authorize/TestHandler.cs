using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizePractice.Authorize
{
    public class TestHandler : AuthorizationHandler<TestRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestRequirement requirement)
        {
            //Console.WriteLine(context.Resource.GetType());
            //if (context.Resource is Microsoft.AspNetCore.Routing.RouteEndpoint routeEndpoint)
            //{
            //    //var endpoint = httpContext.GetEndpoint();
            //    //var actionDescriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            //    context.Succeed(requirement);
            //}
            Console.WriteLine($"TestRequirement value:{requirement.TestNum}");
            //context.Fail();
            return Task.CompletedTask;
        }
    }
}
