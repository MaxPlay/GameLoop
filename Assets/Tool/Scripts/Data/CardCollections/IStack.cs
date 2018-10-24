namespace GameLoop.Data.CardCollections
{
    public interface IStack<T>
    {
        #region Public Methods

        T Peek();

        T Pop();

        void Push(T item);

        #endregion Public Methods
    }
}