using System;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.UI;

namespace UnityComponentUI.Engine.Components.Native
{
    [NativeComponentRegistration("Panel")]
    public class PanelComponent : BaseLayoutComponent
    {
        private static readonly Expression<Func<Image, Sprite>> Sprite = e => e.sprite;
        private static readonly Expression<Func<Image, Image.Type>> Type = e => e.type;

        public override string Name => nameof(PanelComponent);

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var image = builder.AddComponent<Image>();
            var imageResource = props.GetString("image", null);

            if (imageResource != null)
            {
                image.SetProperty(Sprite, ComponentResources.Get(imageResource) as Sprite);
            }

            image.SetProperty(Type, Image.Type.Sliced);
        }
    }
}
