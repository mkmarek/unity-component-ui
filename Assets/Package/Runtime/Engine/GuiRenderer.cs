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

        [SerializeField]
        private UIComponentIndex index;

        private Element rootElement;
        private IRootElementBuilder builder;
        private IRootElementBuilder previousBuilder;
        private bool changed;
        private bool isDestroyed;

        private static object _locker = new object();
        private Thread renderThread;

        private void Start()
        {
            ComponentPool.Initialize(index);
            Hooks.PrepopulateHookData();

            var i = ComponentPool.Instance;
            rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));

            renderThread = new Thread(() =>
            {
                while (!isDestroyed)
                {
                    try
                    {
                        if (changed) continue;
                        PropCollection.FlushCallbacks();
                        var newBuilder = rootElement.Render();
                        if (newBuilder == null) continue;

                        lock (_locker)
                        {
                            if (!(builder is NoopElementBuilder))
                            {
                                previousBuilder = builder;
                            }

                            builder = newBuilder;
                            changed = true;
                        }

                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                }
            });

            renderThread.Start();
        }

        private void Update()
        {
            Hooks.PrepopulateHookData();

            if (!changed) return;

            builder.Build(previousBuilder, this);

            changed = false;
        }

        private void OnDestroy()
        {
            isDestroyed = true;
        }

        public void MarkForDestruction(GameObject go)
        {
            Destroy(go);
        }

        public Transform Root => this.transform;
    }
}
