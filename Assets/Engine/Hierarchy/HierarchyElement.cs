using System;
using System.Collections.Generic;
using Assets.Engine.Components;
using UnityEditor;
using UnityEngine;

namespace Assets.Engine.Hierarchy
{
    [Serializable]
    public class HierarchyElement
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private string componentName;

        [SerializeField]
        private List<string> children;

        public string Id
        {
            get => id;
            set => id = value;
        }

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        public List<string> Children
        {
            get => children;
            set => children = value;
        }

        public void Render(IComponentPool componentPool)
        {
            var component = componentPool.GetComponentByName(ComponentName);

            component.Render();
        }
}
}
