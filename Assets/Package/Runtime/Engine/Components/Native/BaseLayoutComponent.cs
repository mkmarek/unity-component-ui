using System;
using System.Linq.Expressions;
using UnityComponentUI.Engine.Render;
using UnityEngine;
using UnityEngine.Events;

namespace UnityComponentUI.Engine.Components.Native
{
    public abstract class BaseLayoutComponent : BaseNativeComponent
    {
        private static readonly Expression<Func<RectTransform, Vector2>> AnchorMin = e => e.anchorMin;
        private static readonly Expression<Func<RectTransform, Vector2>> AnchorMax = e => e.anchorMax;
        private static readonly Expression<Func<RectTransform, Vector2>> Pivot = e => e.pivot;
        private static readonly Expression<Func<MouseOverDetector, UnityAction>> OnEnter = e => e.OnEnter;
        private static readonly Expression<Func<MouseOverDetector, UnityAction>> OnLeave = e => e.OnLeave;
        private static readonly Expression<Func<RectTransform, Vector2>> OffsetMax = e => e.offsetMax;
        private static readonly Expression<Func<RectTransform, Vector2>> OffsetMin = e => e.offsetMin;

        public override void Render(GameObjectElementBuilder builder, PropCollection props)
        {
            var rectTransform = builder.AddComponent<RectTransform>();
            var canvasRenderer = builder.AddComponent<CanvasRenderer>();
            var mouseOverDetector = builder.AddComponent<MouseOverDetector>();

            var width = props.GetInt("width", 100);
            var height = props.GetInt("height", 100);
            var x = props.GetInt("x", 0);
            var y = props.GetInt("y", 0);
            var onEnter = props.GetCallbackAction("onEnter");
            var onLeave = props.GetCallbackAction("onLeave");

            if (onEnter != null)
            {
                mouseOverDetector.SetProperty(OnEnter, new UnityAction(onEnter));
            }

            if (onLeave != null)
            {
                mouseOverDetector.SetProperty(OnLeave, new UnityAction(onLeave));
            }

            rectTransform.SetProperty(AnchorMin, Vector2.zero);
            rectTransform.SetProperty(AnchorMax, Vector2.zero);
            rectTransform.SetProperty(Pivot, new Vector2(0, 1));

            rectTransform.SetProperty(OffsetMax, new Vector2(x + width, y + height));
            rectTransform.SetProperty(OffsetMin, new Vector2(x, y));
        }
    }
}
