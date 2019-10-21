﻿using System;
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

        private static readonly Dictionary<Element, List<(BaseHook hook, Action invalidate)>> HooksBoundToComponents =
            new Dictionary<Element, List<(BaseHook hook, Action invalidate)>>();

        private static int _componentHookCounter = 0;

        public static BaseHook GetOrRegisterHook(BaseHook hook)
        {
            if (!HooksBoundToComponents.ContainsKey(CurrentContainer))
            {
                HooksBoundToComponents.Add(CurrentContainer, new List<(BaseHook hook, Action invalidate)>());
            }

            var binding = HooksBoundToComponents[CurrentContainer];

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
            foreach (var binding in HooksBoundToComponents)
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