using System;
using System.Collections.Generic;

namespace DIPractice.Services
{
    public class TestExecService
    {
        private readonly IScopedService test1Service;
        private readonly IScopedService test2Service;
        private readonly IScopedService test3Service;

        public TestExecService(Func<string, IScopedService> scopedServiceResolver)
        {
            test1Service = scopedServiceResolver("test1");
            test2Service = scopedServiceResolver("test2");
            test3Service = scopedServiceResolver("test3");
        }

        public IEnumerable<string> GetScopeIds()
        {
            var result = new List<string>();
            result.Add(test1Service.GetScopeId());
            result.Add(test2Service.GetScopeId());
            result.Add(test3Service.GetScopeId());
            return result;
        }
    }
}
