using System.Collections.Generic;
using System.Linq;
using UnityComponentUI.Engine.Render;
using MoonSharp.Interpreter;
using UnityEngine;

namespace UnityComponentUI.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;
        private ConnectedSystem system;
        private PropCollection previousProps;
        private IEnumerable<Hook> previousHooks;
        private List<Hook> currentHooks;
        private int currentHookIndex;

        public string Name { get; }

        public UIComponent(string componentName, Script state, ConnectedSystem system)
        {
            this.state = state;
            this.system = system;
            this.Name = componentName;
            currentHooks = new List<Hook>();
        }

        public IRootElementBuilder Render(Element container)
        {
            var haveChanged = false;
            var props = MergeProps(container.Props, system?.Props);

            if (previousProps == null || HavePropsChanged(previousProps, props))
            {
                haveChanged = true;
            }

            if (currentHooks.Any(e => e.Invalidated))
            {
                haveChanged = true;
            }

            if (!haveChanged) return null;

            Hooks.CurrentComponent = this;
            currentHookIndex = 0;

            var elementId = state.Call(state.Globals["render"], props).CastToString();
            Hooks.CurrentComponent = null;
            var element = Element.GetById(elementId);

            previousProps = props;

            return new PassThroughElementBuilder(element.Render());
        }

        public Hook GetOrRgisterHook(Hook hook)
        {
            if (currentHooks.Count <= currentHookIndex)
            {
                currentHooks.Add(hook);
                return hook;
            }

            return currentHooks[currentHookIndex++];
        }

        private bool HavePropsChanged(PropCollection previous, PropCollection current)
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

        private PropCollection MergeProps(PropCollection containerProps, PropCollection systemProps)
        {
            var props = new PropCollection(new Dictionary<string, object>());

            if (systemProps != null)
            {
                foreach (var key in systemProps.Keys)
                {
                    props[key] = systemProps[key];
                }
            }

            if (containerProps != null)
            {
                foreach (var key in containerProps.Keys)
                {
                    props[key] = containerProps[key];
                }
            }

            return props;
        }
    }
}
