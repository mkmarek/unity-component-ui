using System;
using System.Collections.Generic;
using Assets.Engine;
using Assets.Engine.Hierarchy;
using Assets.Engine.Utils;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class UIComponent : ScriptableObject, IBaseUIComponent
    {
        [SerializeField]
        private string componentName;

        [SerializeField]
        private List<HierarchyElement> elements;

        [SerializeField]
        private string rootId;

        public string ComponentName
        {
            get => componentName;
            set => componentName = value;
        }

        public List<HierarchyElement> Elements
        {
            get => elements;
            set => elements = value;
        }

        public string RootId
        {
            get => rootId;
            set => rootId = value;
        }

        public void Render()
        {
            var rootElement = elements.Find(e => e.Id == RootId);
            rootElement.Render(ComponentPool.Instance);
        }
    }
}
