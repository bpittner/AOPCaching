using System;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace AOPCaching
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachedResultAttribute : InterceptAttribute
    {
        public string CacheKey { get; set; }
        public int ExpirationMinutes { get; set; }

        /// <summary>
        /// Uses default caching time of 8 hours.
        /// </summary>
        public CachedResultAttribute()
        {
            ExpirationMinutes = 0;
        }

        /// <summary>
        /// Specifies the caching duration of the method in minutes.
        /// </summary>
        /// <param name="expirationMinutes"></param>
        public  CachedResultAttribute(int expirationMinutes)
        {
            ExpirationMinutes = expirationMinutes;
        }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<CachedResultInterceptor>();
        }
    }
}
