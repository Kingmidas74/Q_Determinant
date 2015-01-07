using System;

namespace Core.Exceptions
{
    [Serializable]
    public class QException:Exception
    {
        public QException()
        {
            
        }

        public QException(string message) : base(message)
        {
            
        }

        public QException(string message, Exception ex) : base(message)
        {
            
        }

        protected QException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext contex)
            : base(info, contex)
        {
            
        }
    }
}
