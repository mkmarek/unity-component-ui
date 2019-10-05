using UnityEngine;

namespace UnityComponentUI.Engine
{
    public interface IObjectPool
    {
        Transform Root { get; }
        void MarkForDestruction(GameObject go);
    }
}
