using System;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    [NativeComponentRegistration("text")]
    public class TextComponent : BaseLayoutComponent
    {
        private static readonly Expression<Func<Text, Font>> Font = e => e.font;
        private static readonly Expression<Func<Text, Color>> ColorProp = e => e.color;
        private static readonly Expression<Func<Text, string>> Text = e => e.text;
        private static readonly Expression<Func<Text, TextAnchor>> Alignment = e => e.alignment;
        private static readonly Expression<Func<Text, bool>> AlignByGeometry = e => e.alignByGeometry;

        public override string Name => nameof(TextComponent);

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var text = builder.AddComponent<Text>();

            var textValue = props.GetString("text", string.Empty);
            var alignment = props.GetEnum("textAnchor", TextAnchor.UpperLeft);
            var alignByGeometry = props.GetString("alignByGeometry", "false");

            text.SetProperty(Font, ComponentResources.Get("ArialFont") as Font);
            text.SetProperty(ColorProp, Color.black);
            text.SetProperty(Text, textValue);
            text.SetProperty(Alignment, alignment);
            text.SetProperty(AlignByGeometry, alignByGeometry == "true");
        }
    }
}
