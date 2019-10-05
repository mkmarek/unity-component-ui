using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    [NativeComponentRegistration("Text")]
    public class TextComponent : BaseLayoutComponent
    {
        public override string Name => nameof(TextComponent);

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var text = builder.AddComponent<Text>();

            var textValue = props.GetString("text", string.Empty);
            var alignment = props.GetEnum("textAnchor", TextAnchor.UpperLeft);
            var alignByGeometry = props.GetString("alignByGeometry", "false");

            text.SetPropertyDelayed(e => e.font, () => ComponentResources.Get("ArialFont"));
            text.SetProperty(e => e.color, Color.black);
            text.SetProperty(e => e.text, textValue);
            text.SetProperty(e => e.alignment, alignment);
            text.SetProperty(e => e.alignByGeometry, alignByGeometry == "true");
        }
    }
}
