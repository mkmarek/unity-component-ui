using System;
using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Components;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public enum HookType
    {
        SCREEN_SIZE = 1
    }

    public class Hook
    {
        public bool Invalidated { get; set; }

        public IDictionary<string, object> Value { get; }

        public Hook(IDictionary<string, object> value)
        {
            Value = value;
        }
    }

    public class Hooks
    {
        private static readonly List<IDictionary<string, object>> RegisteredStates = new List<IDictionary<string, object>>();

        private static readonly IDictionary<HookType, Hook> PrepopulatedHookData =
            new Dictionary<HookType, Hook>();

        public static UIComponent CurrentComponent { get; set; }

        public static void PrepopulateHookData()
        {
            lock (PrepopulatedHookData)
            {
                if (!PrepopulatedHookData.ContainsKey(HookType.SCREEN_SIZE))
                {
                    PrepopulatedHookData.Add(HookType.SCREEN_SIZE, new Hook(new Dictionary<string, object>()
                    {
                        {"width", Screen.safeArea.width},
                        {"height", Screen.safeArea.height}
                    }));
                }
                else
                {
                    if (!PrepopulatedHookData[HookType.SCREEN_SIZE].Value["width"].Equals(Screen.safeArea.width) ||
                        !PrepopulatedHookData[HookType.SCREEN_SIZE].Value["height"].Equals(Screen.safeArea.height))
                    {
                        PrepopulatedHookData[HookType.SCREEN_SIZE].Invalidated = true;
                        PrepopulatedHookData[HookType.SCREEN_SIZE].Value["width"] = Screen.safeArea.width;
                        PrepopulatedHookData[HookType.SCREEN_SIZE].Value["height"] = Screen.safeArea.height;
                    }
                }
            }
        }

        public static IDictionary<string, object> UseScreenSize()
        {
            lock (PrepopulatedHookData)
            {
                return CurrentComponent.GetOrRgisterHook(PrepopulatedHookData[HookType.SCREEN_SIZE]).Value;
            }
        }

        public static IDictionary<string, object> UseState(object initialValue)
        {
            var hook = new Hook(new Dictionary<string, object> {{"value", initialValue}});

            hook.Value.Add("change", (Action<object>)((value) =>
            {
                hook.Value["value"] = value;
                hook.Invalidated = true;
            }));

            return CurrentComponent.GetOrRgisterHook(hook).Value;
        }
    }
}
