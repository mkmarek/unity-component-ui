using UnityEngine;

namespace UnityComponentUI.Engine.Render
{
    public class PassThroughElementBuilder : IRootElementBuilder
    {
        private IRootElementBuilder childBuilder;

        public PassThroughElementBuilder(IRootElementBuilder builder)
        {
            childBuilder = builder;
        }

        public GameObject Build(IRootElementBuilder previousBuilder, IObjectPool pool, Transform parent = null)
        {
            var previousPassThroughBuilder = (previousBuilder as PassThroughElementBuilder);
            var previousChildBuilder = previousPassThroughBuilder?.childBuilder;

            return childBuilder.Build(previousChildBuilder, pool, parent);
        }

        public void Destroy(IObjectPool pool)
        {
            childBuilder.Destroy(pool);
        }
    }
}
