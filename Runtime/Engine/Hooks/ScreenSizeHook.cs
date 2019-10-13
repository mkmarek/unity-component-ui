using System.Collections.Generic;
using Assets.Package.Runtime.Engine.Hooks;
using UnityEngine;

namespace UnityComponentUI.Engine.Hooks
{
    public class ScreenSizeHook
    {
        private static BaseHook _prepopulated;
        private static readonly object Locker = new object();
        
        public static void Prepopulate()
        {
            lock (Locker)
            {
                if (_prepopulated == null)
                {
                    _prepopulated = new BaseHook(nameof(ScreenSizeHook), new Dictionary<string, object>()
                    {
                        {"width", Screen.safeArea.width},
                        {"height", Screen.safeArea.height}
                    });
                }
                else
                {
                    if (_prepopulated.Value["width"].Equals(Screen.safeArea.width) &&
                        _prepopulated.Value["height"].Equals(Screen.safeArea.height)) return;

                    _prepopulated.Value["width"] = Screen.safeArea.width;
                    _prepopulated.Value["height"] = Screen.safeArea.height;

                    HookComponentRegistration.InvalidateHooks(hook => hook.Name == nameof(ScreenSizeHook));
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
