using System;
using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseNativeComponent : IBaseUIComponent
    {
        public void Render(IRootElementBuilder parent, Element container, int? key = null)
        {
            var builder = new GameObjectElementBuilder(
                container.Component.Name,
                $"{parent?.Path ?? "root"}{(key.HasValue ? $"|{key}" : "")}|{container.Component.Name}");

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
