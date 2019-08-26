using System.Linq;
using UnityEditor;

namespace Assets.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(UIComponent))]
    public class UIComponentFileWrapperInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var component = (UIComponent)this.target;

            GUILayout.Label("Component name: " + component.ComponentName);
            GUILayout.Label("Root component id: " + component.RootId);
            GUILayout.Label("Components used:");
            GUILayout.Box(string.Join(
                "\n",
                component.Elements?.Select(e => $"{e.ComponentName} ({e.Id})") ?? new string[] {}));
        }
    }
}
