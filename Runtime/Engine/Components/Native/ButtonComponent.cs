using System;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    [NativeComponentRegistration("Button")]
    public class ButtonComponent : BaseLayoutComponent
    {
        private static readonly Expression<Func<Image, Sprite>> Sprite = e => e.sprite;
        private static readonly Expression<Func<Image, Image.Type>> Type = e => e.type;
        private static readonly Expression<Func<Button, Button.ButtonClickedEvent>> OnClick = e => e.onClick;

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

                button.SetProperty(OnClick, buttonClickedEvent);
            }

            var imageResource = props.GetString("image", null);

            if (imageResource != null)
            {
                image.SetProperty(Sprite, ComponentResources.Get(imageResource) as Sprite);
            }

            image.SetProperty(Type, Image.Type.Sliced);

            builder.RenderElements(props.GetElements("children"));
        }
    }
}
