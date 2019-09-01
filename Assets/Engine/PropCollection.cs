using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Engine.Render;
using MoonSharp.Interpreter;

namespace Assets.Engine
{
    public class PropCollection : Dictionary<string, object>
    {
        public PropCollection(IDictionary<string, object> props) : base(props ?? new Dictionary<string, object>())
        {

        }

        public IEnumerable<Element> GetElements(string key)
        {
            if (!ContainsKey(key))
            {
                return null;
            }

            if (!(this[key] is Table table))
            {
                return null;
            }

            return table.Values.Select(child => Element.GetById(child.CastToString())).ToArray();
        }

        public int GetInt(string key, int defaultValue)
        {
            if (!ContainsKey(key)) return defaultValue;

            var value = this[key];

            return Convert.ToInt32(value);
        }
    }
}
