using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIPractice.Services
{
    public interface IScopedService
    {
        public string GetScopeId();

    }

    public class ScopedTest1Service : IScopedService
    {
        public string ScopeId = Guid.NewGuid().ToString()[^4..];

        public string GetScopeId()
        {
            return $"test1-{ScopeId}";
        }
    }

    public class ScopedTest2Service : IScopedService
    {
        public string ScopeId = Guid.NewGuid().ToString()[^4..];

        public string GetScopeId()
        {
            return $"test2-{ScopeId}";
        }
    }

    public class ScopedTest3Service : IScopedService
    {
        public string ScopeId = Guid.NewGuid().ToString()[^4..];
        public string GetScopeId()
        {
            return $"test3-{ScopeId}";
        }
    }

    public delegate IScopedService ScopedServiceFactory(string type);
}
