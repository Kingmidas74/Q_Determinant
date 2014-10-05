namespace Core
{
    public class Operation
    {
        private readonly string _signature;
        private readonly int _priority;

        public Operation(int priority, string signature)
        {
            _priority = priority;
            _signature = signature;
        }

        public string Signature
        {
            get { return _signature; }
        }

        public int Priority
        {
            get { return _priority; }
        }
    }
}
