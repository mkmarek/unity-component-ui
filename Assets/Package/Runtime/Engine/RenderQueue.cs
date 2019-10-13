using System;
using System.Collections.Generic;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class RenderQueueItem
    {
        public Func<IRootElementBuilder> RenderAction { get; set; }
        public IRootElementBuilder Parent { get; set; }
    }

    public class RenderQueue : Queue<RenderQueueItem>
    {
        private static RenderQueue _instance;
        private readonly Dictionary<string, IRootElementBuilder> componentToBuilder = new Dictionary<string, IRootElementBuilder>();

        public static RenderQueue Instance => _instance ?? (_instance = new RenderQueue());

        public void DoUnitOfWork(IObjectPool pool, Transform root)
        {
            while (Count > 0)
            {

                var item = Dequeue();

                var builder = item.RenderAction();

                if (builder == null) return;

                var previousBuilder = componentToBuilder.ContainsKey(builder.Path)
                    ? componentToBuilder[builder.Path]
                    : null;

                var parent = previousBuilder?.RootGameObject?.transform.parent ??
                             item.Parent?.RootGameObject?.transform;

                if (parent == null && item.Parent != null)
                {
                    // Waiting for parent to build
                    Enqueue(item);
                    return;
                }

                builder.Build(previousBuilder, pool, parent ?? root);

                if (!(builder is NoopElementBuilder))
                {
                    componentToBuilder[builder.Path] = builder;
                }
            }
        }
    }
}
