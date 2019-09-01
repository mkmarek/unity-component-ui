using System.Collections.Generic;
using Assets.Engine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Assets.Example
{
    public struct CounterComponent : IComponentData
    {
        public int Count;
    }

    public struct CounterJob : IJobForEach<CounterComponent>
    {
        public bool increment;
        public bool decrement;
        public NativeQueue<CounterComponent>.Concurrent counters;

        public void Execute(ref CounterComponent c0)
        {
            if (increment) c0.Count++;
            if (decrement) c0.Count--;

            counters.Enqueue(c0);
        }
    }

    [ConnectedSystemRegistration(nameof(CounterSystem))]
    public class CounterSystem : ConnectedSystem
    {
        private Invokable onPlusClicked = new Invokable();
        private Invokable onMinusClicked = new Invokable();

        protected override IDictionary<string, object> GetProps(ref JobHandle inputDeps)
        {
            var counters = new NativeQueue<CounterComponent>(Allocator.TempJob);

            var job = new CounterJob()
            {
                increment = onPlusClicked.Invoked,
                decrement = onMinusClicked.Invoked,
                counters = counters.ToConcurrent()
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
