using System;

namespace GameLoop.Exceptions
{
    [Serializable]
    public class TypeMismatchException : Exception
    {
        #region Public Constructors

        public TypeMismatchException(Type required, Type recieved) : base(RegularExceptionMessage(required, recieved))
        {
        }

        public TypeMismatchException(Type required, Type recieved, bool inheritance)
            : base(inheritance ? InheritedExceptionMessage(recieved, required) : RegularExceptionMessage(recieved, required)) { }

        #endregion Public Constructors

        #region Protected Constructors

        protected TypeMismatchException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        #endregion Protected Constructors

        #region Private Constructors

        private TypeMismatchException()
        {
        }

        private TypeMismatchException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static string InheritedExceptionMessage(Type required, Type recieved)
        {
            return string.Format("Recieved object of type {0} which does not inherit from {1}.", recieved, required);
        }

        private static string RegularExceptionMessage(Type required, Type recieved)
        {
            return string.Format("Recieved object of type {0} when {1} was required.", recieved, required);
        }

        #endregion Private Methods
    }
}