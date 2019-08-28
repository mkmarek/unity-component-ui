using System.Collections.Generic;
using System.IO;
using Assets.Editor.Language;
using Assets.Engine.Hierarchy;
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

        public UIComponent Interpret(string source, string pathToFile)
        {
            var markup = converter.Convert(source);
            var component = ScriptableObject.CreateInstance<UIComponent>();
            var elements = new List<HierarchyElement> ();

            component.ComponentName = Path.GetFileName(pathToFile).Replace(".luax", "");
            component.Markup = markup;

            return component;
        }
    }
}
