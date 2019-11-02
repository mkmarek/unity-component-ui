using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Components.Native;

namespace UnityComponentUI.Engine
{
    public class ComponentPool : IComponentPool
    {
        public static IComponentPool Instance { get; private set; }

        private readonly Dictionary<string, Func<IBaseUIComponent>> components;

        public static void Initialize(IComponentIndex index)
        {
            Instance = new ComponentPool(index);
        }

        public static void Initialize(IComponentPool pool)
        {
            Instance = pool;
        }

        public ComponentPool(IComponentIndex index)
        {
            components = new Dictionary<string, Func<IBaseUIComponent>>();

            AddNativeComponents();

            foreach (var obj in index.Components)
            {
                var component = obj.Component.Create();
                components.Add(obj.Component.ComponentName, () => component);
            }
        }

        private void AddNativeComponents()
        {
            var nativeComponents = GetNativeComponents();

            foreach (var component in nativeComponents)
            {
                components.Add(component.Item1, component.Item2);
            }
        }

        public static IEnumerable<(string, Func<IBaseUIComponent>)> GetNativeComponents()
        {
            var typesWithMyAttribute =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attribute = t.GetCustomAttribute<NativeComponentRegistrationAttribute>(true)
                where attribute != null
                select new {Type = t, Attribute = attribute};

            foreach (var record in typesWithMyAttribute)
            {
                yield return (record.Attribute.MarkupName, () => (IBaseUIComponent)Activator.CreateInstance(record.Type));
            }
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            return components.ContainsKey(name) ? components[name]() : null;
        }
    }
}
