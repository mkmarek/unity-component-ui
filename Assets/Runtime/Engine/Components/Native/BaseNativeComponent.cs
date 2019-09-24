using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public IRootElementBuilder Render(Element container)
        {
            var builder = new GameObjectElementBuilder(container.Component.Name);

            this.Render(builder, container.Props);

            return builder;
        }

        public abstract string Name { get; }

        public abstract void Render(GameObjectElementBuilder builder, PropCollection props);
    }
}
