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
            rootElement = Element.Create(rootComponent.Create());
        }

        public void Traverse()
        {
            var previousBuilder = builder;
            builder = rootElement.Render();

            var result = builder.Build(previousBuilder);

            result.transform.SetParent(this.transform);
        }
    }
}
