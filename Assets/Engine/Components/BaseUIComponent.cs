using System;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public abstract class BaseUIComponent : ScriptableObject
    {
        public abstract void Render();
    }
}
