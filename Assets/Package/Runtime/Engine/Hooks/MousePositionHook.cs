using System.Collections.Generic;
using Assets.Package.Runtime.Engine.Hooks;
using UnityEngine;

namespace UnityComponentUI.Engine.Hooks
{
    public class MousePositionHook
    {
        private static BaseHook _prepopulated;
        private static readonly object Locker = new object();
        
        public static void Prepopulate()
        {
            lock (Locker)
            {
                if (_prepopulated == null)
                {
                    _prepopulated = new BaseHook(nameof(MousePositionHook), new Dictionary<string, object>()
                    {
                        {"x", Input.mousePosition.x},
                        {"y", Input.mousePosition.y}
                    });
                }
                else
                {
                    if (_prepopulated.Value["x"].Equals(Input.mousePosition.x) &&
                        _prepopulated.Value["y"].Equals(Input.mousePosition.y)) return;

                    _prepopulated.Value["x"] = Input.mousePosition.x;
                    _prepopulated.Value["y"] = Input.mousePosition.y;

                    HookComponentRegistration.InvalidateHooks(hook => hook.Name == nameof(MousePositionHook));
                }
            }
        }

        public static IDictionary<string, object> Use()
        {
            lock (Locker)
            {
                return HookComponentRegistration.GetOrRegisterHook(_prepopulated).Value;
            }
        }
    }
}
