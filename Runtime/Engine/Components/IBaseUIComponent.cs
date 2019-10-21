using System;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine.Components
{
    public interface IBaseUIComponent
    {
        void Render(IRootElementBuilder parent, Element container, int? key = null, bool initial = false);
        string Name { get; }
    }
}
