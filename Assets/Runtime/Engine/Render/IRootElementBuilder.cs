using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public interface IRootElementBuilder
    {
        GameObject Build(IRootElementBuilder previousBuilder, IObjectPool pool, Transform parent = null);
        void Destroy(IObjectPool pool);
    }
}
