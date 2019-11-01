using System.Collections.Generic;
using UnityComponentUI.Engine.Render;

namespace UnityComponentUI.Engine.Components
{
    public interface IBaseUIComponent
    {
        (List<Element> children, GameObjectElementBuilder builder) Render(Element container, PropCollection props, List<Element> children);
        string Name { get; }
    }
}
