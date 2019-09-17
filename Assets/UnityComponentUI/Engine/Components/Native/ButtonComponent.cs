using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    public class ButtonComponent : BaseLayoutComponent
    {
        public override string Name => nameof(ButtonComponent);

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var image = builder.AddComponent<Image>();
            var button = builder.AddComponent<Button>();
            var onClick = props.GetCallbackAction("onClick");

            if (onClick != null)
            {
                var buttonClickedEvent = new Button.ButtonClickedEvent();
                buttonClickedEvent.AddListener(new UnityAction(onClick));

                button.SetProperty(e => e.onClick, buttonClickedEvent);
            }

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
