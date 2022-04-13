using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizePractice.Authorize
{
    public class TestHandler2 : IAuthorizationHandler
    {
        Task IAuthorizationHandler.HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirments = context.PendingRequirements.ToList();
            foreach (var requirement in pendingRequirments)
            {
                if (requirement is TestRequirement testRequirement1)
                {
                    Console.WriteLine($"testRequirement1 value:{testRequirement1.TestNum}");
                    context.Succeed(requirement);
                }
                if (requirement is TestRequirement2 testRequirement2)
                {
                    Console.WriteLine($"testRequirement2 value:{testRequirement2.TestNum2}");
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;

        }
    }
}
