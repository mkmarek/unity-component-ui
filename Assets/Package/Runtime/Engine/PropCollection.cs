using System;
using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Render;
using MoonSharp.Interpreter;

namespace UnityComponentUI.Engine
{
    public class PropCollection : Dictionary<string, object>
    {
        private static List<Action> pendingCallbacks = new List<Action>();

        public PropCollection(IDictionary<string, object> props) : base(props ?? new Dictionary<string, object>())
        {
        }

        public static void FlushCallbacks()
        {
            var copy = pendingCallbacks.ToList();
            foreach (var callback in copy)
            {
                callback?.Invoke();
            }

            pendingCallbacks.Clear();
        }

        public IEnumerable<Element> GetElements(string key)
        {
            if (!ContainsKey(key))
            {
                yield break;
            }

            if (!(this[key] is Table table))
            {
                yield break;
            }

            foreach (var child in table.Values)
            {
                if (child.Table != null)
                {
                    foreach (var nested in child.Table.Values)
                    {
                        yield return Element.GetById(nested.CastToString());
                    }
                }

                else yield return Element.GetById(child.CastToString());
            }
        }

        public int GetInt(string key, int defaultValue)
        {
            if (!ContainsKey(key)) return defaultValue;

            var value = this[key];

            return Convert.ToInt32(value);
        }

        public string GetString(string key, string defaultValue)
        {
            if (!ContainsKey(key)) return defaultValue;

            var value = this[key];

            return Convert.ToString(value);
        }

        public Action GetCallbackAction(string key)
        {
            if (!ContainsKey(key)) return null;

            if (this[key] is CallbackFunction cb)
            {
                return () => pendingCallbacks.Add(() =>
                    cb.ClrCallback(null, new CallbackArguments(new List<DynValue>(), false)));
            }

            if (this[key] is Closure cl)
            {
                return () => pendingCallbacks.Add(() => cl.Call());
            }

            return null;
        }

        public T GetEnum<T>(string key, T defaultValue)
        {
            if (!ContainsKey(key)) return defaultValue;

            var value = this[key];

            var strValue = Convert.ToString(value);

            return (T)Enum.Parse(typeof(T), strValue);
        }
    }
}
