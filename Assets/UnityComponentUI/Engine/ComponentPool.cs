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
        public static ComponentPool Instance { get; private set; }

        private readonly Dictionary<string, Func<IBaseUIComponent>> components;

        public static void Initialize(UIComponentIndex index)
        {
            Instance = new ComponentPool(index);
        }

        public ComponentPool(UIComponentIndex index)
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
            var typesWithMyAttribute =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attribute = t.GetCustomAttribute<NativeComponentRegistrationAttribute>(true)
                where attribute != null
                select new { Type = t, Attribute = attribute };

            foreach (var record in typesWithMyAttribute)
            {
                components.Add(record.Attribute.MarkupName, () => (IBaseUIComponent)Activator.CreateInstance(record.Type));
            }
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            return components.ContainsKey(name) ? components[name]() : null;
        }
    }
}
