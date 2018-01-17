using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LiveDirectorySyncEngineLogic.Settings
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string message) : base(message)
        {
        }

        public InvalidInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
