using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MoonSharp.Interpreter;

namespace UnityComponentUI.Engine.Utils
{
    public static class ReflectionUtils
    {
        static ReflectionUtils()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.ClrFunction, typeof(Action),
                v => new Action(() => v.Callback.Invoke(null, null)));
        }

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

        public static void SetProperty(
            object parent, LambdaExpression expression, object value)
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            var chain = new List<PropertyInfo>();

            while (member != null)
            {
                var propInfo = member.Member as PropertyInfo;

                if (propInfo == null)
                    throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

                chain.Add(propInfo);

                member = member.Expression as MemberExpression;
            }

            chain.Reverse();

            for (var i = 0; i < chain.Count - 1; i++)
            {
                parent = chain[i].GetValue(parent);
            }

            chain.Last().SetValue(parent, value);
        }

        public static object GetProperty(
            object parent, LambdaExpression expression)
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            var chain = new List<PropertyInfo>();

            while (member != null)
            {
                var propInfo = member.Member as PropertyInfo;

                if (propInfo == null)
                    throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

                chain.Add(propInfo);

                member = member.Expression as MemberExpression;
            }

            chain.Reverse();

            for (var i = 0; i < chain.Count - 1; i++)
            {
                parent = chain[i].GetValue(parent);
            }

            return chain.Last().GetValue(parent);
        }
    }
}
