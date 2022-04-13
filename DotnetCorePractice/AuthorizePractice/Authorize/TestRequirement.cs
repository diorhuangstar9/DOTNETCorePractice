using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizePractice.Authorize
{
    public class TestRequirement : IAuthorizationRequirement
    {
        public int TestNum { get; }
        public TestRequirement(int testNum)
        {
            TestNum = testNum;
        }

    }
}
