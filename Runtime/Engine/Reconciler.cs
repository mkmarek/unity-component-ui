using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Package.Runtime.Engine.Hooks;
using MoonSharp.Interpreter;
using UnityComponentUI.Engine.Hooks;
using UnityComponentUI.Engine.Render;
using UnityEngine;

namespace UnityComponentUI.Engine
{
    public class Reconciler
    {
        public static readonly Script State = new Script();

        static Reconciler()
        {
            State.Globals["Create"] = (Func<string, IDictionary<string, object>, string>)Element.Create;
            State.Globals["UseScreenSize"] = (Func<object>)ScreenSizeHook.Use;
            State.Globals["UseState"] = (Func<object, IDictionary<string, object>>)StateHook.Use;
            State.Globals["UseSystem"] = (Func<string, IDictionary<string, object>>)SystemHook.Use;
            State.Globals["UseMousePosition"] = (Func<IDictionary<string, object>>)MousePositionHook.Use;
        }

        public static void CreateElementTree(Element root)
        {
            var renderQueue = new Queue<Element>();
            renderQueue.Enqueue(root);

            while (renderQueue.Count > 0)
            {
                var element = renderQueue.Dequeue();

                element.Render();

                foreach (var child in element.Children)
                {
                    renderQueue.Enqueue(child);
                }
            }
        }

        public static void InvokeBuilders(Element root, IObjectPool pool, Transform parent)
        {
            if (root.Builder != null)
            {
                var gameObject = root.Builder.Build(null, pool, parent);

                foreach (var child in root.Children)
                {
                    InvokeBuilders(child, pool, gameObject.transform);
                }
            }
            else
            {
                foreach (var child in root.Children)
                {
                    InvokeBuilders(child, pool, parent);
                }
            }
        }

        public static void Reconcile(Element existing, IObjectPool pool, Element replacement, Transform rootTransform, Element parent = null)
        {
            if (existing == null && replacement == null)
            {
                return;
            }
            else if (existing == null)
            {
                System.Diagnostics.Debug.Assert(parent != null, nameof(parent) + " != null");

                CreateElementTree(replacement);
                InvokeBuilders(replacement, pool, rootTransform);
                parent.Children.Add(replacement);
            }
            else if (replacement == null)
            {
                System.Diagnostics.Debug.Assert(parent != null, nameof(parent) + " != null");

                DestroyElement(existing, pool);
                parent.Children.Remove(existing);
            }
            else if (existing.Component.Name != replacement.Component.Name)
            {
                Debug.Log($"{existing.Component.Name} != {replacement.Component.Name}");

                DestroyElement(existing, pool);

                CreateElementTree(replacement);
                InvokeBuilders(replacement, pool, GetParentTransform(existing.Parent, rootTransform));

                replacement.Parent = existing.Parent;
                existing.Parent.Children[existing.Parent.Children.IndexOf(existing)] = replacement;
            }
            else if (!existing.Props.Equals(replacement.Props) || existing.Invalidated || MismatchingChildren(existing, replacement))
            {
                var (children, builder) = existing.Component.Render(existing, replacement.Props, replacement.Children);

                replacement.Children = children;
                replacement.Builder = builder;

                try
                {
                    builder?.Build(existing.Builder, pool, existing.Builder.RootGameObject.transform.parent);
                    existing.Builder = builder;
                    existing.Props = replacement.Props;
                }
                catch (Exception ex)
                {
                    int a = 0;
                }

                var maxCount = Mathf.Max(existing.Children.Count, replacement.Children.Count);

                for (var i = 0; i < maxCount; i++)
                {
                    Reconcile(
                        existing.Children.Count > i ? existing.Children[i] : null,
                        pool,
                        replacement.Children.Count > i ? replacement.Children[i] : null,
                        GetParentTransform(existing, rootTransform),
                        existing
                    );
                }
            }
        }

        private static bool MismatchingChildren(Element existing, Element replacement)
        {
            var maxCount = Mathf.Max(existing.Children.Count, replacement.Children.Count);

            for (var i = 0; i < maxCount; i++)
            {
                var child1 = existing.Children.Count > i ? existing.Children[i] : null;
                var child2 = replacement.Children.Count > i ? replacement.Children[i] : null;

                if (child1 == null && child2 != null) return true;
                if (child2 == null && child1 != null) return true;

                System.Diagnostics.Debug.Assert(child1 != null, nameof(child1) + " != null");
                System.Diagnostics.Debug.Assert(child2 != null, nameof(child2) + " != null");

                if (!child1.Props.Equals(child2.Props) || MismatchingChildren(child1, child2))
                {
                    return true;
                }
            }

            return false;
        }

        private static Transform GetParentTransform(Element element, Transform rootTransform)
        {
            if (element == null)
            {
                return rootTransform;
            }

            if (element.Builder != null)
            {
                return element.Builder.RootGameObject.transform;
            }

            return GetParentTransform(element.Parent, rootTransform);
        }

        public static void DestroyElement(Element existing, IObjectPool pool)
        {
            if (existing.Builder != null)
            {
                existing.Builder.Destroy(pool);
            }
            else
            {
                foreach (var child in existing.Children)
                {
                    DestroyElement(child, pool);
                }
            }

            HookComponentRegistration.RemoveHooksFor(existing);
        }
    }
}
