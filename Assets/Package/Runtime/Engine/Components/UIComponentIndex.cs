using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityComponentUI.Engine.Components
{
    [Serializable]
    public class UIComponentIndexItem
    {
        [SerializeField] private string path;

        [SerializeField] private UIComponentDefinition component;

        public string Path
        {
            get => path;
            set => path = value;
        }

        public UIComponentDefinition Component
        {
            get => component;
            set => component = value;
        }
    }

    [Serializable]
    public class UIComponentIndex : ScriptableObject
    {
        [SerializeField]
        private List<UIComponentIndexItem> components;

        public List<UIComponentIndexItem> Components
        {
            get => components;
            set => components = value;
        }
    }
}
