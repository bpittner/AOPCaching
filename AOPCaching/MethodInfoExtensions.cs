using System;
using System.Reflection;

namespace AOPCaching
{
    public static class MethodInfoExtensions
    {
        public static T GetAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            var attributes = methodInfo.GetCustomAttributes(typeof(T), true);
            if (attributes.Length == 0) return null;
            var attr = attributes[0] as T;
            return attr;
        }
    }
}
