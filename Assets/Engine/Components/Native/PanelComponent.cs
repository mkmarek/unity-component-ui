using Assets.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Engine.Components.Native
{
    public class PanelComponent : BaseLayoutComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var image = builder.AddComponent<Image>();

            image.SetProperty(e => e.color, Color.red);

            builder.RenderElements(props.GetElements("children"));
        }
    }
}
