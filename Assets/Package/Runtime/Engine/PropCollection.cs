using System;
using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Render;
using MoonSharp.Interpreter;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class PropCollection : Dictionary<string, object>
    {
        private static List<Action> pendingCallbacks = new List<Action>();

        public PropCollection(IDictionary<string, object> props) : base(props ?? new Dictionary<string, object>())
        {
            if (props != null)
            {
                foreach (var prop in props)
                {
                    var cb = GetCallbackAction(prop.Key);

                    if (cb != null) this[prop.Key] = cb;
                }
            }
        }

        public static void FlushCallbacks()
        {
            var copy = pendingCallbacks.ToList();
            foreach (var callback in copy)
            {
                try { callback?.Invoke();} catch(Exception ex) { Debug.LogError(ex); }
                pendingCallbacks.Remove(callback);
            }
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

            if (this[key] is Action action)
            {
                return action;
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

        public override bool Equals(object other)
        {
            if (!(other is PropCollection collection))
            {
                return false;
            }

            if (this.Keys.Count != collection.Keys.Count)
            {
                return false;
            }

            for (var i = 0; i < Keys.Count; i++)
            {
                if (!Keys.ElementAt(i).Equals(collection.Keys.ElementAt(i)))
                {
                    return false;
                }

                if (!Values.ElementAt(i).Equals(collection.Values.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
