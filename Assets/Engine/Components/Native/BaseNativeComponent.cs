using System.Collections.Generic;
using Assets.Engine.Render;

namespace Assets.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public IRootElementBuilder Render(Element container)
        {
            var builder = new GameObjectElementBuilder();

            this.Render(builder, container.Props);

            return builder;
        }

        public abstract void Render(GameObjectElementBuilder builder, PropCollection props);
    }
}
