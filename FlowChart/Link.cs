namespace FlowChart
{
    public enum BrunchTypes
    {
        True,
        False,
        Null
    }
    public class Link
    {
        public ulong From;
        public ulong To;
        public BrunchTypes Brunch = BrunchTypes.Null;
    }
}
