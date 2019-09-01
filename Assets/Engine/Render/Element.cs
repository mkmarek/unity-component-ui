using System;
using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Engine.Render
{
    public class Element
    {
        private static readonly Dictionary<string, Element> Elements = new Dictionary<string, Element>();

        public IBaseUIComponent Component { get; }

        public PropCollection Props { get; }

        private Element(IBaseUIComponent component, PropCollection props = null)
        {
            this.Component = component;
            this.Props = props;
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();

            var element = new Element(
                ComponentPool.Instance.GetComponentByName(name),
                new PropCollection(props));

            Elements.Add(id, element);

            return id;
        }

        public static Element Create(IBaseUIComponent component, PropCollection props = null)
        {
            return new Element(component, props);
        }

        public static Element GetById(string id)
        {
            return Elements.ContainsKey(id) ? Elements[id] : null;
        }

        public IRootElementBuilder Render()
        {
            var builder = this.Component.Render(this);

            return builder;
        }
    }
}
