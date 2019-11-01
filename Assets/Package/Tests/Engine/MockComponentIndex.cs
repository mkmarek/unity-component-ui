using System.Collections.Generic;
using UnityComponentUI.Engine;
using UnityComponentUI.Engine.Components;

namespace Assets.Package.Tests.Engine
{
    public class MockComponentIndex : IComponentPool
    {
        public Dictionary<string, IBaseUIComponent> Components { get; } = new Dictionary<string, IBaseUIComponent>();

        public MockComponentIndex()
        {
            var nativeComponents = ComponentPool.GetNativeComponents();

            foreach (var component in nativeComponents)
            {
                Components.Add(component.Item1, component.Item2());
            }
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            return Components[name];
        }
    }
}
