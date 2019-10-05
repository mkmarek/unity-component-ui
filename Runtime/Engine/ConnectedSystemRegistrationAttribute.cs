using System;

namespace UnityComponentUI.Engine
{
    public class ConnectedSystemRegistrationAttribute : Attribute
    {
        public ConnectedSystemRegistrationAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
