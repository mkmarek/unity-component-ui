using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Assets.Engine.Utils
{
    public static class ReflectionUtils
    {
        public static string GetPropertyChain<TSource, TProperty>(Expression<Func<TSource, TProperty>> expression)
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            var chain = new List<string>();

            while (member != null)
            {
                var propInfo = member.Member as PropertyInfo;

                if (propInfo == null)
                    throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

                chain.Add(propInfo.Name);

                member = member.Expression as MemberExpression;
            }

            chain.Reverse();

            return string.Join(".", chain);
        }
    }
}
