using UnityComponentUI.Engine;
using UnityEngine;

namespace UnityComponentUI.Example
{
    public class Resources : MonoBehaviour
    {
        [SerializeField]
        private Object[] resources;

        private void Awake()
        {
            foreach (var resource in resources)
            {
                ComponentResources.Register(resource.name, resource);
            }

            ComponentResources.Register("ArialFont", Font.CreateDynamicFontFromOSFont("Arial", 16));
        }
    }
}
