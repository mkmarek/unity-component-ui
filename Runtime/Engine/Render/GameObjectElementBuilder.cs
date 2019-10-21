using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Utils;
using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class GameObjectElementBuilder : ElementBuilder, IRootElementBuilder
    {
        protected readonly List<ComponentElementBuilder> components;
        private readonly List<IRootElementBuilder> childBuilders;
        private IEnumerable<Element> childElements;
        private string name;
        private readonly bool initialRenderer;

        public string Path { get; private set; }
        public GameObject RootGameObject { get; private set; }

        public GameObjectElementBuilder(string name, string path, bool initialRenderer) : base(typeof(GameObject))
        {
            components = new List<ComponentElementBuilder>();
            childBuilders = new List<IRootElementBuilder>();
            this.name = name;
            this.initialRenderer = initialRenderer;
            Path = path;
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

            if (childElements != null)
            {
                var elementCount = childElements.Count();

                for (var i = 0; i < elementCount; i++)
                {
                    var element = childElements.ElementAt(i);
                    if (previousGameObjectElementBuilder == null || initialRenderer)
                    {
                        element?.Render(this, i, initialRenderer);
                    }
                    else if (previousGameObjectElementBuilder.childElements.Count() <= i)
                    {
                        element?.Render(this, i, initialRenderer);
                    }
                    else if (!previousGameObjectElementBuilder.childElements.ElementAt(i).Props.Equals(element.Props))
                    {
                        element?.Render(this, i, initialRenderer);
                    }
                    else
                    {
                        var builder = previousGameObjectElementBuilder.childBuilders[i];
                        RenderQueue.Instance.Enqueue(new RenderQueueItem
                        {
                            RenderAction = () => builder,
                            Parent = this,
                        });
                    }
                }
            }

            RootGameObject = previousGameObjectElementBuilder?.RootGameObject != null
                ? previousGameObjectElementBuilder?.RootGameObject
                : new GameObject(Path);

            RootGameObject.transform.SetParent(parent ? parent : pool.Root);

            for (var i = 0; i < components.Count; i++)
            {
                components[i].Build(RootGameObject, previousGameObjectElementBuilder?.components[i]);
            }

            foreach (var key in setters.Keys)
            {
                if (previousGameObjectElementBuilder?.valuesForSetters.ContainsKey(key) != true ||
                    !previousGameObjectElementBuilder.valuesForSetters[key].Equals(valuesForSetters[key]))
                {
                    ReflectionUtils.SetProperty(RootGameObject, setters[key], valuesForSetters[key]);
                }
            }

            for (var i = 0; i < childBuilders.Count; i++)
            {
                var previousChildBuilder = previousGameObjectElementBuilder?.childBuilders.Count <= i
                    ? null
                    : previousGameObjectElementBuilder?.childBuilders[i];

                if (childBuilders[i] == null)
                {
                    previousChildBuilder?.Destroy(pool);
                }

                if (childBuilders[i] == null) continue;

                if (childBuilders[i] is NoopElementBuilder)
                {
                    childBuilders[i] = previousChildBuilder;
                }
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
            childElements = elements;
        }
    }
}
