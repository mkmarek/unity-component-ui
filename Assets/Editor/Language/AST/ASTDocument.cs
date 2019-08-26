using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Editor.Language.AST
{
    public class ASTDocument
    {
        public ASTDocument()
        {
            Props = new List<ASTPropDefinition>();
        }

        public ASTTemplateDefinition Template { get; set; }
        public List<ASTPropDefinition> Props { get; set; }
    }
}
