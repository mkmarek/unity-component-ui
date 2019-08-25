using System;

namespace Assets.Editor.Parser
{
    public class CSLSyntaxErrorException : Exception
    {
        public CSLSyntaxErrorException(string message, string source, int index) : base(message)
        {
            this.SourceCode = source;
            this.Index = index;
        }

        public string SourceCode { get; set; }
        public int Index { get; set; }
    }
}
