using System.Collections.Generic;
using Assets.Engine.Render;
using MoonSharp.Interpreter;

namespace Assets.Engine.Components
{
    public class UIComponent : IBaseUIComponent
    {
        private Script state;
        private ConnectedSystem system;
        private PropCollection previousProps;

        public UIComponent(Script state, ConnectedSystem system)
        {
            this.state = state;
            this.system = system;
        }

        public IRootElementBuilder Render(Element container)
        {
            var props = MergeProps(container.Props, system?.Props);

            if (previousProps != null)
            {
                if (!HavePropsChanged(previousProps, props))
                {
                    return null;
                }
            }

            var elementId = state.Call(state.Globals["render"], props).CastToString();
            var element = Element.GetById(elementId);

            previousProps = props;

            return new PassThroughElementBuilder(element.Render());
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
