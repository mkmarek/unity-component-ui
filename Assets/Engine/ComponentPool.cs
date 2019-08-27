using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private Dictionary<string, IBaseUIComponent> components;

        public ComponentPool()
        {
            components = new Dictionary<string, IBaseUIComponent>();
            components.Add("Panel", new PanelComponent());
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            if (components.ContainsKey(name))
            {
                return components[name];
            }

            return null;
        }
    }
}
