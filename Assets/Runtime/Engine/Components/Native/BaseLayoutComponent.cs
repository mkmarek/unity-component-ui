using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseLayoutComponent : BaseNativeComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            var rectTransform = builder.AddComponent<RectTransform>();
            var canvasRenderer = builder.AddComponent<CanvasRenderer>();

            var width = props.GetInt("width", 100);
            var height = props.GetInt("height", 100);
            var x = props.GetInt("x", 0);
            var y = props.GetInt("y", 0);

            rectTransform.SetProperty(e => e.anchorMin, Vector2.zero);
            rectTransform.SetProperty(e => e.anchorMax, Vector2.zero);
            rectTransform.SetProperty(e => e.pivot, new Vector2(0, 1));

            rectTransform.SetProperty(e => e.offsetMax, new Vector2(x + width, y + height));
            rectTransform.SetProperty(e => e.offsetMin, new Vector2(x, y));
        }
    }
}
