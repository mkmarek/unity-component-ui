using System.Collections.Generic;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class RenderQueueItem
    {
        public Element elementToUpdate;
    }

    public class RenderQueue : Queue<RenderQueueItem>
    {
        private static RenderQueue _instance;
        public static RenderQueue Instance => _instance ?? (_instance = new RenderQueue());

        public void DoUnitOfWork(IObjectPool pool, Transform root)
        {
            while (Count > 0)
            {
                var item = Dequeue();
                Reconciler.Reconcile(item.elementToUpdate, pool, item.elementToUpdate.Clone(), root);
            }
        }
    }
}
