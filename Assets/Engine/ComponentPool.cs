using System;
using System.Collections.Generic;
using Assets.Engine.Components;
using Assets.Engine.Components.Native;

namespace Assets.Engine
{
    public class ComponentPool : IComponentPool
    {
        private static ComponentPool instance;

        public static ComponentPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentPool();
                }

                return instance;
            }
        }

        private Dictionary<string, Func<IBaseUIComponent>> components;

        public ComponentPool()
        {
            components = new Dictionary<string, Func<IBaseUIComponent>>();
            components.Add("Panel", () => new PanelComponent());
            components.Add("Button", () => new ButtonComponent());
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            if (components.ContainsKey(name))
            {
                return components[name]();
            }

            return null;
        }
    }
}
