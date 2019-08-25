using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace Assets.Editor
{
    [ScriptedImporter(1, "uicomponent")]
    public class UIComponentImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var obj = ScriptableObject.CreateInstance<UIComponent>();

            var content = File.ReadAllText(ctx.assetPath);
            Debug.Log(content);

            ctx.AddObjectToAsset("main", obj);
            ctx.SetMainObject(obj);
        }
    }
}
