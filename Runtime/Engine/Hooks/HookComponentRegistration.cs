using System;
using System.Linq;
using System.Collections.Generic;
using UnityComponentUI.Engine;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Render;

namespace Assets.Package.Runtime.Engine.Hooks
{
    public static class HookComponentRegistration
    {
        public static UIComponent CurrentComponent { get; set; }
        public static Element CurrentContainer { get; set; }

        private static readonly Dictionary<Element, List<BaseHook>> HooksBoundToComponents =
            new Dictionary<Element, List<BaseHook>>();

        private static int _componentHookCounter = 0;

        public static BaseHook GetOrRegisterHook(BaseHook hook)
        {
            if (!HooksBoundToComponents.ContainsKey(CurrentContainer))
            {
                HooksBoundToComponents.Add(CurrentContainer, new List<BaseHook>());
            }

            var binding = HooksBoundToComponents[CurrentContainer];

            if (_componentHookCounter < binding.Count)
            {
                return binding[_componentHookCounter++];
            }
            
            binding.Add(hook);

            _componentHookCounter++;

            return hook;
        }

        public static void InvalidateHooks(Predicate<BaseHook> predicate)
        {
            foreach (var binding in HooksBoundToComponents.ToList())
            {
                foreach (var hook in binding.Value)
                {
                    if (predicate(hook))
                    {
                        binding.Key.Invalidated = true;
                        RenderQueue.Instance.Enqueue(new RenderQueueItem { elementToUpdate = binding.Key });
                    }
                }
            }
        }

        public static void ResetHookCounter()
        {
            _componentHookCounter = 0;
        }

        public static void RemoveHooksFor(Element element)
        {
            HooksBoundToComponents.Remove(element);
        }
    }
}
