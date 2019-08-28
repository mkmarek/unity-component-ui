using Assets.Engine.Render;
using UnityEngine;

namespace Assets
{
    public class GuiRenderer : MonoBehaviour
    {
        [SerializeField]
        private UIComponentDefinition rootComponent;

        private Reconciler reconciler;

        private void Start()
        {
            reconciler = new Reconciler(rootComponent);

            reconciler.BuildTree();
        }
    }
}
