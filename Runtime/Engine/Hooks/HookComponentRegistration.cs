using System;
using System.Collections.Generic;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;

namespace Assets.Package.Runtime.Engine.Hooks
{
    public static class HookComponentRegistration
    {
        public static UIComponent CurrentComponent { get; set; }
        public static IRootElementBuilder CurrentParent { get; set; }
        public static Element CurrentContainer { get; set; }
        public static int? CurrentKey { get; set; }

        private static Dictionary<Element, List<(BaseHook hook, Action invalidate)>> hooksBoundToComponents =
            new Dictionary<Element, List<(BaseHook hook, Action invalidate)>>();

        private static int _componentHookCounter = 0;

        public static BaseHook GetOrRegisterHook(BaseHook hook)
        {
            if (!hooksBoundToComponents.ContainsKey(CurrentContainer))
            {
                hooksBoundToComponents.Add(CurrentContainer, new List<(BaseHook hook, Action invalidate)>());
            }

            var binding = hooksBoundToComponents[CurrentContainer];

            if (_componentHookCounter < binding.Count)
            {
                return binding[_componentHookCounter++].hook;
            }

            var parent = CurrentParent;
            var container = CurrentContainer;
            var key = CurrentKey;

            binding.Add((hook, () =>
            {
                container.Render(parent, key);
            }));

            _componentHookCounter++;

            return hook;
        }

        public static void InvalidateHooks(Predicate<BaseHook> predicate)
        {
            foreach (var binding in hooksBoundToComponents)
            {
                foreach (var hook in binding.Value)
                {
                    if (predicate(hook.hook))
                    {
                        hook.invalidate();
                    }
                }
            }
        }

        public static void ResetHookCounter()
        {
            _componentHookCounter = 0;
        }
    }
}
