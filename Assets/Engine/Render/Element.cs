using System;
using System.Collections.Generic;

namespace Assets.Engine.Render
{
    public class Element
    {
        private static Dictionary<string, Element> elements = new Dictionary<string, Element>();

        public IBaseUIComponent Component { get; }

        public IDictionary<string, object> Props { get; }

        public IList<Element> Children { get; }

        private Element(IBaseUIComponent component, IDictionary<string, object> props = null)
        {
            this.Children = new List<Element>();
            this.Component = component;
            this.Props = props;
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();

            elements.Add(id, new Element(
                ComponentPool.Instance.GetComponentByName(name),
                props));

            return id;
        }

        public static Element GetById(string id)
        {
            return elements.ContainsKey(id) ? elements[id] : null;
        }
    }
}
