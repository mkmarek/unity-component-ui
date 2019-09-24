using System;

namespace UnityComponentUI.Engine.Components.Native
{
    public class NativeComponentRegistrationAttribute : Attribute 
    {
        public NativeComponentRegistrationAttribute(string markupName)
        {
            this.MarkupName = markupName;
        }

        public string MarkupName { get; }
    }
}
