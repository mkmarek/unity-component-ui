using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public interface IRootElementBuilder
    {
        GameObject Build(IRootElementBuilder previousBuilder, Transform parent);
    }
}
