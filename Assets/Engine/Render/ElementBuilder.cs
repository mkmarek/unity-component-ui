using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Assets.Engine.Render
{
    public class ElementBuilder
    {
        protected readonly Type currentType;
        protected readonly Dictionary<string, LambdaExpression> setters;
        protected readonly Dictionary<string, object> valuesForSetters;

        public ElementBuilder(Type currentType)
        {
            this.currentType = currentType;
            setters = new Dictionary<string, LambdaExpression>();
            valuesForSetters = new Dictionary<string, object>();
        }
    }
}
