using System;
using System.Runtime.Caching;
using Ninject.Extensions.Interception;

namespace AOPCaching
{
    public class CachedResultInterceptor : IInterceptor
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public void Intercept(IInvocation invocation)
        {
            // Pull the custom attribute class
            var attributes = invocation.Request.Method.GetAttribute<CachedResultAttribute>();
            if (attributes == null)
            {
                invocation.Proceed();
                return;
            }

            // Determine the expiration minutes
            int expirationMinutes = attributes.ExpirationMinutes != 0 ? attributes.ExpirationMinutes : 480; //480 = 8 hours

            // Generate the cache key
            string key = GenerateKey(invocation, attributes.CacheKey);

            // Pull data from cache or execute the method and store the result
            if(Cache.Contains(key))
            {
                invocation.ReturnValue = Cache[key];
            }
            else
            {
                invocation.Proceed();
                if(invocation.ReturnValue != null)
                {
                    Cache.Add(key, invocation.ReturnValue, DateTime.Now.AddMinutes(expirationMinutes));
                }
            }
        }

        private string GenerateKey(IInvocation invocation, string keyFormat)
        {
            if (string.IsNullOrEmpty(keyFormat))
                keyFormat = invocation.Request.Method.DeclaringType + "." + invocation.Request.Method.Name;

            return keyFormat + string.Join("-", invocation.Request.Arguments);
        }
    }
}
