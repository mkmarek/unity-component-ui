using System;
using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public void Render(IRootElementBuilder parent, Element container, int? key = null, bool initial = false)
        {
            var builder = new GameObjectElementBuilder(
                container.Component.Name,
                container.Path,
                initial);

            this.Render(builder, container.Props);

            parent?.AddChildBuilder(builder);

            RenderQueue.Instance.Enqueue(new RenderQueueItem
            {
                RenderAction = () => builder,
                Parent = parent,
            });
        }

        public abstract string Name { get; }

        public abstract void Render(GameObjectElementBuilder builder, PropCollection props);
    }
}
