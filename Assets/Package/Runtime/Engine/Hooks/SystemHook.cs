using System.Collections.Generic;
using Assets.Package.Runtime.Engine.Hooks;

namespace UnityComponentUI.Engine.Hooks
{
    public class SystemHook
    {
        private static Dictionary<string, BaseHook> _usedSystems = new Dictionary<string, BaseHook>();
        private static readonly object Locker = new object();
        
        public static void Prepopulate()
        {
            lock (Locker)
            {
                foreach (var used in _usedSystems)
                {
                    var system = ConnectedSystemRegistry.Instance.Get(used.Key);
                    var newProps = system.Props;

                    if (HavePropsChanged(newProps, used.Value.Value))
                    {
                        used.Value.Value = newProps;
                        HookComponentRegistration.InvalidateHooks(hook => hook.Name == $"{nameof(ScreenSizeHook)}_{used.Key}");
                    }
                }
            }
        }

        private static bool HavePropsChanged(IDictionary<string, object> previous, IDictionary<string, object> current)
        {
            if (previous.Count != current.Count) return true;

            foreach (var key in previous.Keys)
            {
                if (!current.ContainsKey(key))
                {
                    return true;
                }

                if (!current[key].Equals(previous[key]))
                {
                    return true;
                }
            }

            return false;
        }

        public static IDictionary<string, object> Use(string systemName)
        {
            lock (Locker)
            {
                var system = ConnectedSystemRegistry.Instance.Get(systemName);

                if (system == null) return null;

                if (_usedSystems.ContainsKey(systemName))
                    return HookComponentRegistration.GetOrRegisterHook(_usedSystems[systemName]).Value;

                var hook = new BaseHook($"{nameof(ScreenSizeHook)}_{systemName}", new Dictionary<string, object>(system.Props));
                _usedSystems.Add(systemName, hook);

                return HookComponentRegistration.GetOrRegisterHook(_usedSystems[systemName]).Value;
            }
        }
    }
}
