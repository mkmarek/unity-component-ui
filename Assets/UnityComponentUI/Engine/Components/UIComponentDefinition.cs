using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine.Components
{
    [Serializable]
    public class UIComponentDefinition : ScriptableObject
    {
        [SerializeField]
        private string componentName;

        [SerializeField]
        private string markup;

        [SerializeField]
        private string boundSystem;

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        public string Markup
        {
            get => markup;
            set => markup = value;
        }

        public string BoundSystem
        {
            get => boundSystem;
            set => boundSystem = value;
        }

        public UIComponent Create()
        {
            var state = new Script();

            state.Globals["Create"] = (Func<string, IDictionary<string, object>, string>)Element.Create;
            state.DoString(markup);

            return new UIComponent(state, ConnectedSystemRegistry.Instance.Get(boundSystem));
        }
    }
}
