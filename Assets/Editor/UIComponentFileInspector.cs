using UnityEditor;

namespace Assets.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(UIComponent))]
    public class UIComponentFileWrapperInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Button("fdsfsdfs");
        }
    }
}
