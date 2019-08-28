using UnityEngine;

namespace Assets
{
    public class GuiRenderer : MonoBehaviour
    {
        [SerializeField]
        private UIComponent rootComponent;

        private void Start()
        {
            rootComponent.Render();
        }
    }
}
