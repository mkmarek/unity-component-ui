using Unity.Entities;
using UnityEngine;

namespace UnityComponentUI.Example
{
    public sealed class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene()
        {
            var entityManager = World.Active.EntityManager;

            var counterArchetype = entityManager.CreateArchetype(
                typeof(CounterComponent));

            var counterEntity = entityManager.CreateEntity(counterArchetype);

            entityManager.SetComponentData(counterEntity, new CounterComponent {Count = 0});
        }
    }
}
