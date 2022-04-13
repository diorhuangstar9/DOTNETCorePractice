using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizePractice.Authorize
{
    public class TestHandler : AuthorizationHandler<TestRequirement, Test2AuthorizeAttribute>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestRequirement requirement, 
            Test2AuthorizeAttribute resource)
        {
            //Console.WriteLine(context.Resource.GetType());
            if (context.Resource is Microsoft.AspNetCore.Routing.RouteEndpoint routeEndpoint)
            {
                var actionDescriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                var attributes = actionDescriptor?.MethodInfo.GetCustomAttributes(typeof(Test2AuthorizeAttribute), false)
                    .Cast<Test2AuthorizeAttribute>();
                var testVal = attributes.FirstOrDefault()?.TestVal;
                Console.WriteLine($"Attribute value:{testVal}");

            }
            Console.WriteLine($"TestRequirement value:{requirement.TestNum}");

            //context.Fail();
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
