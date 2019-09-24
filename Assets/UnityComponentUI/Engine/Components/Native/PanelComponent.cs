using UnityComponentUI.Engine.Render;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    [NativeComponentRegistration("Panel")]
    public class PanelComponent : BaseLayoutComponent
    {
        public override string Name => nameof(PanelComponent);

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var image = builder.AddComponent<Image>();
            var imageResource = props.GetString("image", null);

            if (imageResource != null)
            {
                image.SetProperty(e => e.sprite, ComponentResources.Get(imageResource));
            }

            image.SetProperty(e => e.type, Image.Type.Sliced);

            builder.RenderElements(props.GetElements("children"));
        }
    }
}
