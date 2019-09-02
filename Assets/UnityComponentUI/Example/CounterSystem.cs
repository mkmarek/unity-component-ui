using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityComponentUI.Engine;

namespace UnityComponentUI.Example
{
    public struct CounterComponent : IComponentData
    {
        public int Count;
    }

    public struct CounterJob : IJobForEach<CounterComponent>
    {
        public bool Increment;
        public bool Decrement;
        public NativeQueue<CounterComponent>.Concurrent Counters;

        public void Execute(ref CounterComponent c0)
        {
            if (Increment) c0.Count++;
            if (Decrement) c0.Count--;

            Counters.Enqueue(c0);
        }
    }

    [ConnectedSystemRegistration(nameof(CounterSystem))]
    public class CounterSystem : ConnectedSystem
    {
        private readonly Invokable onPlusClicked = new Invokable();
        private readonly Invokable onMinusClicked = new Invokable();

        protected override IDictionary<string, object> GetProps(ref JobHandle inputDeps)
        {
            var counters = new NativeQueue<CounterComponent>(Allocator.TempJob);

            var job = new CounterJob()
            {
                Increment = onPlusClicked.Invoked,
                Decrement = onMinusClicked.Invoked,
                Counters = counters.ToConcurrent()
            };

            inputDeps = job.Schedule(this, inputDeps);

            inputDeps.Complete();

            var counter = counters.Dequeue();

            counters.Dispose();

            onPlusClicked.Reset();
            onMinusClicked.Reset();

            return new Dictionary<string, object>()
            {
                { "count", counter.Count },
                { "onPlusClicked", onPlusClicked.Action },
                { "onMinusClicked", onMinusClicked.Action }
            };
        }
    }
}
