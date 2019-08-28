using System.Collections.Generic;
using UnityEngine;

namespace Assets.Engine.Render
{
    public class Element
    {
        public static string Create(string name, IDictionary<string, object> props = null)
        {
            Debug.Log("Creating: " + name);
            return name;
        }
    }
}
