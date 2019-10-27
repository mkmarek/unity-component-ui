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

        public string Path { get; private set; }

        private Element(string id, string path, IBaseUIComponent component, PropCollection props = null)
        {
            this.Component = component;
            this.Props = props;
            this.Id = id;
            this.Path = path;
        }

        public void SetContainer(Element container)
        {
            this.Path = $"{container.Path}|{Path}";
            PrependPath(this.Path);
        }

        public void PrependPath(string path)
        {
            var childElements = Props.GetElements("children");
            var index = 0;
            foreach (var el in childElements)
            {
                if (el != null)
                {
                    el.Path = $"{path}|{el.Path}|{index++}";
                    el.PrependPath(el.Path);
                }
            }
        }

        public static string Create(string name, IDictionary<string, object> props = null)
        {
            var id = Guid.NewGuid().ToString();
            var propCollection = new PropCollection(props);

            var element = new Element(
                id,
                name,
                ComponentPool.Instance.GetComponentByName(name),
                propCollection);

            Elements.Add(id, element);

            return id;
        }

        public static Element Create(IBaseUIComponent component, PropCollection props = null)
        {
            var id = "root";

            return new Element(id, id, component, props);
        }

        public static Element GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            return Elements.ContainsKey(id) ? Elements[id] : null;
        }

        public void Render(IRootElementBuilder parent, int? key = null, bool initial = false)
        {
            this.Component.Render(parent, this, key, initial);
        }
    }
}
