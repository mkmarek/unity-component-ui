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
        private string name;

        public GameObject RootGameObject { get; private set; }

        public GameObjectElementBuilder(string name) : base(typeof(GameObject))
        {
            components = new List<ComponentElementBuilder>();
            childBuilders = new List<IRootElementBuilder>();
            this.name = name;
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

        public GameObject Build(IRootElementBuilder previousBuilder, IObjectPool pool, Transform parent = null)
        {
            var previousGameObjectElementBuilder = previousBuilder as GameObjectElementBuilder;

            RootGameObject = previousGameObjectElementBuilder?.RootGameObject != null
                ? previousGameObjectElementBuilder?.RootGameObject
                : new GameObject(name);

            RootGameObject.transform.SetParent(parent ? parent : pool.Root);

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
                var previousChildBuilder = previousGameObjectElementBuilder?.childBuilders.Count <= i
                    ? null
                    : previousGameObjectElementBuilder?.childBuilders[i];

                if (childBuilders[i] == null && previousChildBuilder != null)
                {
                    previousChildBuilder.Destroy(pool);
                }

                if (childBuilders[i] == null) continue;

                if (childBuilders[i] is NoopElementBuilder)
                {
                    childBuilders[i] = previousChildBuilder;
                }

                var childObjects = childBuilders[i].Build(
                    previousChildBuilder,
                    pool,
                    RootGameObject.transform);
            }

            if (childBuilders.Count < previousGameObjectElementBuilder?.childBuilders.Count)
            {
                for (var i = childBuilders.Count; i < previousGameObjectElementBuilder.childBuilders.Count; i++)
                {
                    previousGameObjectElementBuilder.childBuilders[i].Destroy(pool);
                }
            }

            return RootGameObject;
        }

        public void Destroy(IObjectPool pool)
        {
            pool.MarkForDestruction(RootGameObject);
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
                if (element == null) continue;

                AddChildBuilder(element.Render());
            }
        }
    }
}
