using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class NoopElementBuilder : IRootElementBuilder
    {
        public GameObject RootGameObject => null;
        public string Path => null;

        public GameObject Build(IRootElementBuilder previousBuilder, IObjectPool pool, Transform parent = null)
        {
            return null;
        }

        public void Destroy(IObjectPool pool)
        {
        }

        public void AddChildBuilder(IRootElementBuilder builder)
        {

        }
    }
}
