using System.Collections.Generic;

namespace Assets.Package.Runtime.Engine.Hooks
{
    public class BaseHook
    {
        public string Name { get; set; }

        public IDictionary<string, object> Value { get; set; }

        public BaseHook(string name, IDictionary<string, object> value)
        {
            Value = value;
            Name = name;
        }
    }
}
