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

        private void Start()
        {
            ComponentPool.Initialize(index);
            ScreenSizeHook.Prepopulate();

            var rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));
            rootElement.Render(null);
        }

        private void Update()
        {
            SystemHook.Prepopulate();
            ScreenSizeHook.Prepopulate();
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
