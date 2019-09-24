using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using Unity.Jobs;

namespace UnityComponentUI.Engine
{
    public abstract class ConnectedSystem : JobComponentSystem
    {
        public PropCollection Props => props ?? DefaultProps;

        private PropCollection props;
        protected virtual PropCollection DefaultProps => null;

        protected override void OnCreate()
        {
            base.OnCreate();

            var currentType = this.GetType();
            var connectedSystemAttr = currentType.GetCustomAttribute<ConnectedSystemRegistrationAttribute>();

            ConnectedSystemRegistry.Instance.Register(connectedSystemAttr.Name, this);
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            props = new PropCollection(GetProps(ref inputDeps));

            return inputDeps;
        }

        protected abstract IDictionary<string, object> GetProps(ref JobHandle inputDeps);
    }
}
