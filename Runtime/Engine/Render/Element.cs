using System;
using System.Collections.Generic;
using UnityComponentUI.Engine.Components;

namespace UnityComponentUI.Engine.Render
{
    public class Element
    {
        private static readonly Dictionary<string, Element> Elements = new Dictionary<string, Element>();

        public IBaseUIComponent Component { get; }

        public PropCollection Props { get; }

        public string Id { get; }

        private Element(string id, IBaseUIComponent component, PropCollection props = null)
        {
            this.Component = component;
            this.Props = props;
            this.Id = id;
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();

            var element = new Element(
                id,
                ComponentPool.Instance.GetComponentByName(name),
                new PropCollection(props));

            Elements.Add(id, element);

            return id;
        }

        public static Element Create(IBaseUIComponent component, PropCollection props = null)
        {
            var id = "root";

            return new Element(id, component, props);
        }

        public static Element GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            return Elements.ContainsKey(id) ? Elements[id] : null;
        }

        public void Render(IRootElementBuilder parent, int? key = null)
        {
            this.Component.Render(parent, this, key);
        }
    }
}
