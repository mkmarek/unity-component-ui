using System.IO;
using Assets.Editor.Language;
using UnityEngine;

namespace Assets.Editor
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
