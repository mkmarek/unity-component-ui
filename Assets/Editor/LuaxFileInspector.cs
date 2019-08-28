using System.Linq;
using UnityEditor;

namespace Assets.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(UIComponent))]
    public class LuaxFileInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var component = (UIComponent)this.target;

            GUILayout.Label("Component name: " + component.ComponentName);
            GUILayout.Label("Markup:");
            GUILayout.Box(component.Markup);
        }
    }
}
