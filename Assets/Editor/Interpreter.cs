using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Editor.Language;
using Assets.Editor.Language.AST;
using Assets.Engine.Components;
using Assets.Engine.Hierarchy;
using Assets.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class Interpreter
    {
        private readonly Parser parser;

        public Interpreter()
        {
            parser = new Parser();
        }

        public UIComponent Interpret(string source, string pathToFile)
        {
            var ast = parser.Parse(source);
            var component = ScriptableObject.CreateInstance<UIComponent>();
            var elements = new List<HierarchyElement> ();

            component.ComponentName = Path.GetFileName(pathToFile).Replace(".uicomponent", "");
            component.RootId = GatherHierarchyElements(ast.Template.RootNode, elements).Id;
            component.Elements = elements;

            return component;
        }

        private HierarchyElement GatherHierarchyElements(ASTTemplateNode templateNode, IList<HierarchyElement> elements)
        {

            var element = new HierarchyElement()
            {
                ComponentName = templateNode.Name,
                Id = Guid.NewGuid().ToString(),
                Children = new List<string>()
            };

            elements.Add(element);

            foreach (var node in templateNode.Children)
            {
                var child = GatherHierarchyElements(node, elements);
                element.Children.Add(child.Id);
            }

            return element;
        }
    }
}
