using UnityEngine;

namespace Assets.Engine.Render
{
    public interface IRootElementBuilder
    {
        GameObject Build(IRootElementBuilder previousBuilder);
    }
}
