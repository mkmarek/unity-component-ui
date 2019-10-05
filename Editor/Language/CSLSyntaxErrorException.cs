using System;

namespace UnityComponentUI.Editor.Language
{
    public class CSLSyntaxErrorException : Exception
    {
        public CSLSyntaxErrorException(string message, int index) : base(message)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }
}
