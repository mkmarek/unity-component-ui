using System;
using System.Collections.Generic;
using UnityComponentUI.Engine.Components;
using UnityComponentUI.Engine.Components.Native;
using UnityEditor;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class ComponentPool : IComponentPool
    {
        private static ComponentPool instance;

        public static ComponentPool Instance => instance ?? (instance = new ComponentPool());

        private readonly Dictionary<string, Func<IBaseUIComponent>> components;

        public ComponentPool()
        {
            components = new Dictionary<string, Func<IBaseUIComponent>>
            {
                { "Panel", () => new PanelComponent() },
                { "Button", () => new ButtonComponent() },
                { "Text", () => new TextComponent() }
            };

            var foundObjects = GetAllInstances<UIComponentDefinition>();

            foreach (var obj in foundObjects)
            {
                components.Add(obj.ComponentName, () => obj.Create());
            }
        }

        public IBaseUIComponent GetComponentByName(string name)
        {
            return components.ContainsKey(name) ? components[name]() : null;
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            var a = new T[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;

        }
    }
}
