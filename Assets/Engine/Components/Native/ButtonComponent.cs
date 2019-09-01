using Assets.Engine.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Engine.Components.Native
{
    public class ButtonComponent : BaseLayoutComponent
    {
        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            base.Render(builder, props);

            var image = builder.AddComponent<Image>();
            var button = builder.AddComponent<Button>();
            var onClick = props.GetCallbackAction("onClick");

            var buttonClickedEvent = new Button.ButtonClickedEvent();
            buttonClickedEvent.AddListener(new UnityAction(onClick));

            button.SetProperty(e => e.onClick, buttonClickedEvent);
            image.SetProperty(e => e.color, Color.blue);
        }
    }
}
