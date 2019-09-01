using Assets.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Engine.Components.Native
{
    public class TextComponent : BaseLayoutComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var text = builder.AddComponent<Text>();

            var textValue = props.GetString("text", string.Empty);

            text.SetProperty(e => e.font, Font.CreateDynamicFontFromOSFont("Arial", 16));
            text.SetProperty(e => e.color, Color.black);
            text.SetProperty(e => e.text, textValue);
        }
    }
}
