using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityComponentUI.Engine.Hooks;
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

        public UIComponent Create()
        {
            return Create(componentName, markup);
        }

        public static UIComponent Create(string componentName, string markup)
        {
            Reconciler.State.DoString(markup);

            Reconciler.State.Globals[$"{componentName}_render"] = Reconciler.State.Globals["render"];
            Reconciler.State.Globals["render"] = null;

            return new UIComponent(componentName, Reconciler.State);
        }
    }
}
