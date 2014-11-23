namespace Core.Atoms
{
    public class Link
    {
        public ulong Id { get; set; }
        public ulong From { get; set; }
        public LinkTypes Type { get; set; }
    }
}
