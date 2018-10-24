using GameLoop.Modules;
using System;
using UnityEngine;

namespace GameLoop.Interfaces
{
    public abstract class UserInterface : MonoBehaviour, IDisposable
    {
        #region Private Fields

        private bool destroyed;

        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Properties

        public ModuleState State { get; set; }

        public ModuleSystem System { get; set; }

        #endregion Public Properties

        #region Public Methods

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public virtual void Remove()
        {
            if (!destroyed)
            {
                Destroy(gameObject);
                destroyed = true;
            }
        }

        public abstract void Show();

        #endregion Public Methods

        #region Protected Methods

        // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Remove();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (System.State == SystemState.Stopped)
                Remove();
        }

        protected virtual void Start()
        {
            destroyed = false;
        }

        #endregion Protected Methods

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserInterface() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }
    }
}