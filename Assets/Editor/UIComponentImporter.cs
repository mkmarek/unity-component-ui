using System.IO;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace Assets.Editor
{
    [ScriptedImporter(1, "uicomponent")]
    public class UIComponentImporter : ScriptedImporter
    {
        private readonly Interpreter interpreter;

        public UIComponentImporter()
        {
            interpreter = new Interpreter();
        }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var content = File.ReadAllText(ctx.assetPath);

            var obj = interpreter.Interpret(content, ctx.assetPath);

            ctx.AddObjectToAsset("main", obj);
            ctx.SetMainObject(obj);

            EditorUtility.SetDirty(obj);
        }
    }
}
