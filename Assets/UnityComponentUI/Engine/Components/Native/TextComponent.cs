using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    public class TextComponent : BaseLayoutComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var text = builder.AddComponent<Text>();

            var textValue = props.GetString("text", string.Empty);

            text.SetPropertyDelayed(e => e.font, () => Font.CreateDynamicFontFromOSFont("Arial", 16));
            text.SetProperty(e => e.color, Color.black);
            text.SetProperty(e => e.text, textValue);
        }
    }
}
