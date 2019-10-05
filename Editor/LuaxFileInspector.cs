using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace UnityComponentUI.Editor
{
    [CustomEditor(typeof(LuaxImporter))]
    [CanEditMultipleObjects]
    public class LuaxFileInspector : ScriptedImporterEditor
    {
        SerializedProperty boundSystem;

        public override void OnEnable()
        {
            base.OnEnable();
            // Once in OnEnable, retrieve the serializedObject property and store it.
            boundSystem = serializedObject.FindProperty("boundSystem");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedObject in case it has been changed outside the Inspector.
            serializedObject.Update();

            // Draw the boolean property.
            EditorGUILayout.PropertyField(boundSystem);

            // Apply the changes so Undo/Redo is working
            serializedObject.ApplyModifiedProperties();

            // Call ApplyRevertGUI to show Apply and Revert buttons.
            ApplyRevertGUI();
        }
    }
}
