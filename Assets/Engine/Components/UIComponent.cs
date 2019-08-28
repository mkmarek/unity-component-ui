using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Engine;
using Assets.Engine.Hierarchy;
using Assets.Engine.Render;
using Assets.Engine.Utils;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class UIComponent : ScriptableObject, IBaseUIComponent
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

        public void Render()
        {
            var script = new Script();

            script.Globals["Create"] = (Func<string, IDictionary<string, object>, string>)Element.Create;

            script.DoString(markup);
            script.Call(script.Globals["render"]);
        }
    }
}
