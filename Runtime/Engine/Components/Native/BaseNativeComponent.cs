using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public (List<Element> children, GameObjectElementBuilder builder) Render(Element container, PropCollection props, List<Element> children)
        {
            var builder = new GameObjectElementBuilder(
                container.Component.Name);

            this.Render(builder, props);

            return (children, builder);
        }

        public abstract string Name { get; }

        public abstract void Render(GameObjectElementBuilder builder, PropCollection props);
    }
}
