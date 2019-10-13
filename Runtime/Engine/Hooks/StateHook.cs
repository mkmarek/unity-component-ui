using System;
using System.Collections.Generic;
using Assets.Package.Runtime.Engine.Hooks;

namespace UnityComponentUI.Engine.Hooks
{
    public class StateHook
    {
        public static IDictionary<string, object> Use(object initialValue)
        {
            var hook = new BaseHook(nameof(StateHook), new Dictionary<string, object> { { "value", initialValue } });
            var container = HookComponentRegistration.CurrentContainer;
            var parent = HookComponentRegistration.CurrentParent;

            hook.Value.Add("change", (Action<object>)((value) =>
            {
                hook.Value["value"] = value;

                container.Render(parent);
            }));

            return HookComponentRegistration.GetOrRegisterHook(hook).Value;
        }
    }
}
