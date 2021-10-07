using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizePractice.Authorize
{
    public class TestRequirement2 : IAuthorizationRequirement
    {
        public int TestNum2 { get; }
        public TestRequirement2(int testNum)
        {
            TestNum2 = testNum;
        }
    }
}
