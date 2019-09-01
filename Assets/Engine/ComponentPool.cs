using System;
using System.Collections.Generic;
using Assets.Engine.Components;
using Assets.Engine.Components.Native;

namespace Assets.Engine
{
    public class ComponentPool : IComponentPool
    {
        private static ComponentPool instance;

        public static ComponentPool Instance => instance ?? (instance = new ComponentPool());

        private readonly Dictionary<string, Func<IBaseUIComponent>> components;

        public ComponentPool()
        {
            components = new Dictionary<string, Func<IBaseUIComponent>>
            {
                { "Panel", () => new PanelComponent() },
                { "Button", () => new ButtonComponent() },
                { "Text", () => new TextComponent() }
            };
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            return components.ContainsKey(name) ? components[name]() : null;
        }
    }
}
