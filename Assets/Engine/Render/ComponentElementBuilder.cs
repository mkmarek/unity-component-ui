using System;
using System.Linq.Expressions;
using Assets.Engine.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Engine.Render
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
                ReflectionUtils.SetProperty(component, setters[key], valuesForSetters[key]);
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
