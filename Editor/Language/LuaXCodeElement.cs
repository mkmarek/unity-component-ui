using Assets.UnityComponentUI.Editor.Language;

namespace UnityComponentUI.Editor.Language
{
    public class LuaXCodeElement : LuaXElement
    {
        public string Code { get; set; }

        public override string GetComponentMarkup()
        {
            return Code;
        }
    }
}
