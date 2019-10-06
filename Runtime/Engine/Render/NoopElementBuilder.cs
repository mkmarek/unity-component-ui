using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class NoopElementBuilder : IRootElementBuilder
    {
        public GameObject Build(IRootElementBuilder previousBuilder, IObjectPool pool, Transform parent = null)
        {
            return null;
        }

        public void Destroy(IObjectPool pool)
        {
        }
    }
}
