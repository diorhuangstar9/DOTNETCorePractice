using Microsoft.AspNetCore.Authorization;
using System;

namespace AuthorizePractice.Authorize
{

    public class Test2AuthorizeAttribute : Attribute
    {

        public Test2AuthorizeAttribute(int testVal)
        {
            TestVal = testVal;
        }

        public int TestVal { get; set; }
    }

    public class TestAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "Test";

        public TestAuthorizeAttribute(int testVal) => TestVal = testVal;

        // Get or set the Age property by manipulating the underlying Policy property
        public int TestVal
        {
            get
            {
                if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var testVal))
                {
                    return testVal;
                }
                return default(int);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value}";
            }
        }

    }
}
