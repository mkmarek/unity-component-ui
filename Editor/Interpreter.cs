using System.IO;
using UnityComponentUI.Editor.Language;
using UnityComponentUI.Engine.Components;
using UnityEngine;

namespace UnityComponentUI.Editor
{
    public class Interpreter
    {
        private readonly XLuaElementConverter converter;

        public Interpreter()
        {
            converter = new XLuaElementConverter();
        }

        public UIComponentDefinition Interpret(string source, string pathToFile)
        {
            var markup = converter.Convert(source);
            var component = ScriptableObject.CreateInstance<UIComponentDefinition>();

            component.ComponentName = Path.GetFileName(pathToFile)?.Replace(".luax", "");
            component.Markup = markup;

            return component;
        }
    }
}
