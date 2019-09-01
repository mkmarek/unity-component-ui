using Assets.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Engine.Components.Native
{
    public class PanelComponent : BaseNativeComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            var rectTransform = builder.AddComponent<RectTransform>();
            var canvasRenderer = builder.AddComponent<CanvasRenderer>();
            var image = builder.AddComponent<Image>();

            image.SetProperty(e => e.color, Color.red);

            var width = props.GetInt("width", 100);
            var height = props.GetInt("height", 100);
            var x = props.GetInt("x", 0);
            var y = props.GetInt("y", 0);

            rectTransform.SetProperty(e => e.anchorMin, Vector2.zero);
            rectTransform.SetProperty(e => e.anchorMax, Vector2.zero);

            rectTransform.SetProperty(e => e.offsetMax, new Vector2(x + width, x + height));
            rectTransform.SetProperty(e => e.offsetMin, new Vector2(x, y));

            builder.RenderElements(props.GetElements("children"));
        }
    }
}
