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
        public string GetScopeId()
        {
            return $"test1-{Guid.NewGuid().ToString()[^4..]}";
        }
    }

    public class ScopedTest2Service : IScopedService
    {
        public string GetScopeId()
        {
            return $"test2-{Guid.NewGuid().ToString()[^4..]}";
        }
    }

    public class ScopedTest3Service : IScopedService
    {
        public string GetScopeId()
        {
            return $"test3-{Guid.NewGuid().ToString()[^4..]}";
        }
    }


}
