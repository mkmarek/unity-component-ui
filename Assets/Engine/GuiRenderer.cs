using System.Collections.Generic;
using Assets.Engine;
using Assets.Engine.Render;
using UnityEngine;

namespace Assets
{
    public class GuiRenderer : MonoBehaviour
    {
        [SerializeField]
        private UIComponentDefinition rootComponent;

        private Element rootElement;
        private IRootElementBuilder builder;

        private void Start()
        {
            rootElement = Element.Create(rootComponent.Create(), new PropCollection(new Dictionary<string, object>()));
        }

        public void Traverse()
        {
            var previousBuilder = builder;

            builder = rootElement.Render();

            if (builder != null)
            {
                builder.Build(previousBuilder, this.transform);
            }
            else
            {
                builder = previousBuilder;
            }
        }

        private void Update()
        {
            Traverse();
        }
    }
}
