using System.Collections.Generic;
using System.Threading;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class GuiRenderer : MonoBehaviour
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
            var i = ComponentPool.Instance;
            rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));

            var t = new Thread(() =>
            {
                while (true)
                {
                    if (changed) continue;

                    var newBuilder = rootElement.Render();

                    if (newBuilder == null) continue;

                    lock (_locker)
                    {
                        previousBuilder = builder;
                        builder = newBuilder;
                        changed = true;
                    }
                }
            });

            t.Start();
        }

        private void Update()
        {
            if (!changed) return;

            builder.Build(previousBuilder, this.transform);

            changed = false;
        }
    }
}
