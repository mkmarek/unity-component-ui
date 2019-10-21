using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityComponentUI.Engine.Components;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityComponentUI.Editor
{
    [ScriptedImporter(1, "luax")]
    public class LuaxImporter : ScriptedImporter
    {
        private readonly Interpreter interpreter;

        public LuaxImporter()
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
