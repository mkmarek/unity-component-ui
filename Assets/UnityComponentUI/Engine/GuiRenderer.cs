using System;
using System.Collections.Generic;
using System.Threading;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class GuiRenderer : MonoBehaviour, IObjectPool
    {
        [SerializeField]
        private UIComponentDefinition rootComponent;

        private Element rootElement;
        private IRootElementBuilder builder;
        private IRootElementBuilder previousBuilder;
        private bool changed;

        private static object _locker = new object();

        private void Start()
        {
            Hooks.PrepopulateHookData();

            var i = ComponentPool.Instance;
            rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));

            var t = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (changed) continue;
                        PropCollection.FlushCallbacks();
                        var newBuilder = rootElement.Render();
                        if (newBuilder == null) continue;

                        lock (_locker)
                        {
                            previousBuilder = builder;
                            builder = newBuilder;
                            changed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            });

            t.Start();
        }

        private void Update()
        {
            Hooks.PrepopulateHookData();

            if (!changed) return;

            builder.Build(previousBuilder, this);

            changed = false;
        }

        public void MarkForDestruction(GameObject go)
        {
            Destroy(go);
        }

        public Transform Root => this.transform;
    }
}
