using Assets.Engine.Render;
using UnityEngine;

namespace Assets.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public void Render(Element container)
        {
            var builder = new ElementBuilder<GameObject>();

            this.Render(builder);

            container.Builder = builder;
        }

        public abstract void Render(ElementBuilder<GameObject> builder);
    }
}
