using System;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityComponentUI.Engine.Render
{
    public class ComponentElementBuilder : ElementBuilder
    {
        private Object component;

        public ComponentElementBuilder(Type currentType) : base(currentType)
        {
        }

        public Object Build(GameObject gameObject, ComponentElementBuilder previousBuilder)
        {
            component = previousBuilder?.component
                ? previousBuilder?.component
                : gameObject.AddComponent(currentType);

            foreach (var key in setters.Keys)
            {
                var value = valuesForSetters.ContainsKey(key)
                    ? valuesForSetters[key]
                    : null;

                var previousValue = previousBuilder?.valuesForSetters.ContainsKey(key) == true
                    ? previousBuilder?.valuesForSetters[key]
                    : null;

                if (previousValue != null && previousValue.Equals(value)) continue;

                ReflectionUtils.SetProperty(component, setters[key], value);
            }

            return component;
        }
    }

    public class ComponentElementBuilder<TElement> : ComponentElementBuilder
    {
        public ComponentElementBuilder<TElement> SetProperty<TValue>(
            Expression<Func<TElement, TValue>> property, TValue value)
        {
            var key = ReflectionUtils.GetPropertyChain(property);
            setters.Add(key, property);
            valuesForSetters.Add(key, value);

            return this;
        }

        public ComponentElementBuilder() : base(typeof(TElement))
        {
        }
    }
}
