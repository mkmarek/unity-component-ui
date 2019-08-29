using Assets.Engine.Render;
using UnityEngine;

namespace Assets.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public string Render()
        {
            return null;
        }

        public abstract void Render(ElementBuilder<GameObject> builder);
    }
}
