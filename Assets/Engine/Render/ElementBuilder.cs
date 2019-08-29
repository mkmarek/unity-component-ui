using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Engine.Utils;
using UnityEngine;

namespace Assets.Engine.Render
{
    public class ElementBuilder
    {
        private readonly Type currentType;
        protected readonly List<ElementBuilder> components;
        protected readonly List<ElementBuilder> children;
        protected readonly Dictionary<string, LambdaExpression> setters;

        public ElementBuilder(Type currentType)
        {
            this.currentType = currentType;
            components = new List<ElementBuilder>();
            children = new List<ElementBuilder>();
            setters = new Dictionary<string, LambdaExpression>();
        }
    }

    public class ElementBuilder<TElement> : ElementBuilder
    {
        public ElementBuilder<TComponent> AddComponent<TComponent>()
        {
            var component = new ElementBuilder<TComponent>();
            this.components.Add(component);

            return component;
        }

        public ElementBuilder<TElement> AddChild<TChild>(ElementBuilder<TChild> child)
        {
            this.children.Add(child);
            return this;
        }

        public ElementBuilder<TElement> SetProperty<TValue>(
            Expression<Func<TElement, TValue>> property, TValue value)
        {
            setters.Add(ReflectionUtils.GetPropertyChain(property), property);

            return this;
        }

        public ElementBuilder() : base(typeof(TElement))
        {
        }
    }
}
