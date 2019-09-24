using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UnityComponentUI.Editor.Language
{
    public class LuaXComponentElement : LuaXElement
    {
        public List<LuaXElement> Children { get; set; }
        public List<Tuple<string, string>> Props { get; set; }

        public override string GetComponentMarkup()
        {
            return $"Create(\"{Name}\"{GetPropsMarkup()})";
        }

        private string GetPropsMarkup()
        {
            var childrenMarkup = GetChildrenMarkup();
            var props = Props.Select(e => $"{e.Item1} = {e.Item2}").ToList();

            if (!string.IsNullOrWhiteSpace(childrenMarkup))
            {
                props.Add(childrenMarkup);
            }

            var propsInString = string.Join(", ", props);

            return string.IsNullOrWhiteSpace(propsInString)
                ? string.Empty
                : $", {{ {string.Join(", ", props)} }}";
        }

        private string GetChildrenMarkup()
        {
            if (Children.Count == 0)
            {
                return null;
            }

            var childrenComponents = string.Join(", ", Children
                .Select(e => e.GetComponentMarkup())
                .ToArray());

            return $"children = {{ {childrenComponents} }}";
        }
    }
}
