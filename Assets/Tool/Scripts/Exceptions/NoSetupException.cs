using System;

namespace GameLoop.Exceptions
{
    [Serializable]
    public class NoSetupException : Exception
    {
        #region Public Constructors

        public NoSetupException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected NoSetupException()
        {
        }

        protected NoSetupException(string message) : base(message)
        {
        }

        protected NoSetupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        #endregion Protected Constructors
    }
}