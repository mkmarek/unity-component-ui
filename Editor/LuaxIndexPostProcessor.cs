using System.Linq;
using UnityComponentUI.Engine.Components;
using UnityEditor;
using UnityEngine;

namespace Assets.UnityComponentUI.Editor
{
    public class LuaxIndexPostProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var index = AssetDatabase.LoadAssetAtPath<UIComponentIndex>("Assets/ComponentIndex.asset");
            var isDirty = false;

            if (index == null)
            {
                index = ScriptableObject.CreateInstance<UIComponentIndex>();
                AssetDatabase.CreateAsset(
                    index, AssetDatabase.GenerateUniqueAssetPath("Assets/ComponentIndex.asset"));
            }

            foreach (string path in importedAssets)
            {
                var component = AssetDatabase.LoadAssetAtPath<UIComponentDefinition>(path);

                if (component == null) continue;

                index.Components.RemoveAll(e => e.Path == path);
                index.Components.Add(new UIComponentIndexItem { Path = path, Component = component});
                isDirty = true;
            }
            foreach (string str in deletedAssets)
            {
                if (index.Components.RemoveAll(e => e.Path == str) > 0)
                {
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                EditorUtility.SetDirty(index);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
