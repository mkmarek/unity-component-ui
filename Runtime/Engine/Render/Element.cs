using System;
using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Components;

namespace UnityComponentUI.Engine.Render
{
    public class Element
    {
        private static readonly Dictionary<string, Element> Elements = new Dictionary<string, Element>();

        public IBaseUIComponent Component { get; }

        public PropCollection Props { get; set; }

        public string Id { get; }
        
        public GameObjectElementBuilder Builder { get; set; }

        public List<Element> Children { get; set; }

        public Element Parent { get; set; }

        public bool Invalidated { get; set; }

        private Element(string id, IBaseUIComponent component, PropCollection props = null)
        {
            this.Component = component;
            this.Props = props;
            this.Id = id;
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();
            var propCollection = new PropCollection(props);

            var children = propCollection.GetElements("children")
                .Where(e => e != null)
                .ToList();
            var element = new Element(
                id,
                ComponentPool.Instance.GetComponentByName(name),
                propCollection);

            foreach (var child in children)
            {
                child.Parent = element;
            }

            element.Children = children;
            Elements.Add(id, element);

            return id;
        }

        public static void ClearElementCache()
        {
            Elements.Clear();
        }

        public static Element Create(IBaseUIComponent component, PropCollection props = null)
        {
            var id = "root";

            return new Element(id, component, props);
        }

        public static Element GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var element = Elements.ContainsKey(id) ? Elements[id] : null;

            return element;
        }

        public void Render()
        {
            var (children, builder) = this.Component.Render(this, this.Props, this.Children);

            this.Children = children;
            this.Builder = builder;
        }

        public Element Clone()
        {
            return new Element(this.Id, this.Component, this.Props)
            {
                Builder = this.Builder,
                Children = this.Children,
                Parent = this.Parent
            };
        }
    }
}
