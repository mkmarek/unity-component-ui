using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Utils;
using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class GameObjectElementBuilder : ElementBuilder, IRootElementBuilder
    {
        protected readonly List<ComponentElementBuilder> components;
        private List<IRootElementBuilder> childBuilders;

        public GameObject RootGameObject { get; private set; }

        public GameObjectElementBuilder() : base(typeof(GameObject))
        {
            components = new List<ComponentElementBuilder>();
            childBuilders = new List<IRootElementBuilder>();
        }

        public ComponentElementBuilder<TComponent> AddComponent<TComponent>()
        {
            var component = new ComponentElementBuilder<TComponent>();
            this.components.Add(component);

            return component;
        }

        public GameObjectElementBuilder SetProperty<TValue>(
            Expression<Func<GameObject, TValue>> property, TValue value)
        {
            var key = ReflectionUtils.GetPropertyChain(property);
            setters.Add(key, property);
            valuesForSetters.Add(key, value);

            return this;
        }

        public GameObject Build(IRootElementBuilder previousBuilder, Transform parent)
        {
            var previousGameObjectElementBuilder = previousBuilder as GameObjectElementBuilder;

            RootGameObject = previousGameObjectElementBuilder?.RootGameObject
                ? previousGameObjectElementBuilder?.RootGameObject
                : new GameObject();

            RootGameObject.transform.SetParent(parent);

            for (var i = 0; i < components.Count; i++)
            {
                components[i].Build(RootGameObject, previousGameObjectElementBuilder?.components[i]);
            }

            foreach (var key in setters.Keys)
            {
                ReflectionUtils.SetProperty(RootGameObject, setters[key], valuesForSetters[key]);
            }

            for (var i = 0; i < childBuilders.Count; i++)
            {
                var childObjects = childBuilders[i].Build(
                    previousGameObjectElementBuilder?.childBuilders[i],
                    RootGameObject.transform);
            }

            return RootGameObject;
        }

        public void AddChildBuilder(IRootElementBuilder builder)
        {
            childBuilders.Add(builder);
        }

        public void RenderElements(IEnumerable<Element> elements)
        {
            if (elements == null) return;

            foreach (var element in elements)
            {
                AddChildBuilder(element.Render());
            }
        }
    }
}
