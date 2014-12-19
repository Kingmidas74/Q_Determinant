using CodeGeneration.Enums;
using Core.Atoms;

namespace CodeGeneration.InternalClasses
{
    public class CGBlock:Block
    {
        public string Alias { get; set; }
        public VariableTypes CGType { get; set; }
    }
}
