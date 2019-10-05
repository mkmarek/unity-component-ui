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
            }
            foreach (string str in deletedAssets)
            {
                index.Components.RemoveAll(e => e.Path == str);
            }

            AssetDatabase.SaveAssets();
        }
    }
}
