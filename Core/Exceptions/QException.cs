using System;

namespace Core.Exceptions
{
    public class QException:Exception
    {
        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
        public QException()
        {

        }

        public QException(string message)
        {
            

        }
    }
}
