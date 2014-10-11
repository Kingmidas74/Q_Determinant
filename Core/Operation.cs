namespace Core
{
    public class Operation
    {
        private readonly string _signature;
        private readonly int _priority;
        private readonly bool _isUnary;

        public Operation(int priority, string signature, bool isUnary)
        {
            _priority = priority;
            _signature = signature;
            _isUnary = isUnary;
        }

        public string Signature
        {
            get { return _signature; }
        }

        public int Priority
        {
            get { return _priority; }
        }

        public bool IsUnary
        {
            get { return _isUnary; }
        }
    }
}
