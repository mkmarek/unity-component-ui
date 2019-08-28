using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;

        public UIComponent(Script state)
        {
            this.state = state;
        }

        public string Render()
        {
            return state.Call(state.Globals["render"]).CastToString();
        }
    }
}
