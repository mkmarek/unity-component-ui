using UnityComponentUI.Editor.Language;

namespace Assets.UnityComponentUI.Editor.Language
{
    public abstract class LuaXElement
    {
        public Token Start { get; set; }
        public Token End { get; set; }
        public string Name { get; set; }

        public string Replace(string source, ref int offset)
        {
            var componentString = GetComponentMarkup();
            var result = source
                .Remove(Start.Start + offset, End.End - Start.Start)
                .Insert(Start.Start + offset, componentString);

            // adjust the offset because of changed string size
            offset = offset + componentString.Length - (End.End - Start.Start);

            return result;
        }

        public abstract string GetComponentMarkup();
    }
}
