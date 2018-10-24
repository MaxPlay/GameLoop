using System;

namespace GameLoop.Exceptions
{
    [Serializable]
    public class DataManagerException : Exception
    {
        #region Public Constructors

        public DataManagerException(string message, Type type) : base(message)
        {
            DataType = type;
        }

        public DataManagerException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected DataManagerException()
        {
        }

        protected DataManagerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        #endregion Protected Constructors

        #region Public Properties

        public Type DataType { get; set; }

        #endregion Public Properties
    }
}