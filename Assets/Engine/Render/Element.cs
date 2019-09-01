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

        public IDictionary<string, object> Props { get; private set; }

        private Element(IBaseUIComponent component, IDictionary<string, object> props = null)
        {
            this.Component = component;
            this.Props = props;
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();

            var element = new Element(
                ComponentPool.Instance.GetComponentByName(name),
                props);

            Elements.Add(id, element);

            if (props?.ContainsKey("children") == false)
            {
                return id;
            }

            if (!(props?["children"] is Table children))
            {
                return id;
            }

            props["children"] = children.Values.Select(child => GetById(child.CastToString())).ToArray();

            return id;
        }

        public static Element Create(IBaseUIComponent component, IDictionary<string, object> props = null)
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
