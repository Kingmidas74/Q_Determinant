namespace Core.Atoms
{
    public class Block
    {
        public ulong Id { get; set; }
        public string Content { get; set; }
        public BlockTypes Type { get; set; }
        public ulong Level { get; set; }
        
    }
}
