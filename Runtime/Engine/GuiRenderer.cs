using System;
using System.Collections.Generic;
using System.Threading;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Hooks;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class GuiRenderer : MonoBehaviour, IObjectPool
    {
        [SerializeField]
        private UIComponentDefinition rootComponent;

        [SerializeField]
        private UIComponentIndex index;

        private bool built = false;

        private void Start()
        {
            ComponentPool.Initialize(index);
            ScreenSizeHook.Prepopulate();
            MousePositionHook.Prepopulate();
        }

        private void Update()
        {
            if (!built)
            {
                var rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));
                Reconciler.CreateElementTree(rootElement);
                Reconciler.InvokeBuilders(rootElement, this, this.transform);

                built = true;
            }

            Element.ClearElementCache();
            SystemHook.Prepopulate();
            ScreenSizeHook.Prepopulate();
            MousePositionHook.Prepopulate();
            PropCollection.FlushCallbacks();

            RenderQueue.Instance.DoUnitOfWork(this, this.transform);
        }

        public void MarkForDestruction(GameObject go)
        {
            Destroy(go);
        }

        public Transform Root => this.transform;
    }
}
