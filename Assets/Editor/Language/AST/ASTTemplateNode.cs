using System.Collections.Generic;

namespace Assets.Editor.Language.AST
{
    public class ASTTemplateNode
    {
        public ASTTemplateNode()
        {
            Children = new List<ASTTemplateNode>();
        }

        public string Name { get; set; }
        public List<ASTTemplateNode> Children { get; set; }
    }
}
