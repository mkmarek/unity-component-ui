using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Assets.Package.Runtime.Engine.Hooks;
using UnityComponentUI.Engine.Utils;
using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class GameObjectElementBuilder : ElementBuilder, IRootElementBuilder
    {
        protected readonly List<ComponentElementBuilder> components;
  
        public string Name { get; }
        public GameObject RootGameObject { get; private set; }

        public GameObjectElementBuilder(string name) : base(typeof(GameObject))
        {
            components = new List<ComponentElementBuilder>();
            Name = name;
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
                : new GameObject(Name);

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

            return RootGameObject;
        }

        public void Destroy(IObjectPool pool)
        {
            pool.MarkForDestruction(RootGameObject);
        }
    }
}
