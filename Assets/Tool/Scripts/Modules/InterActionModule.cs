using System.Collections;

namespace GameLoop.Modules
{
    public abstract class InterActionModule : ActionModule
    {
        #region Protected Fields

        protected IEnumerator execution;

        #endregion Protected Fields

        #region Public Methods

        public void Reset()
        {
            execution = null;
        }

        public override void Run()
        {
            if (execution == null)
            {
                State = ModuleState.Running;
                Start();

                execution = Update();
            }

            if (!execution.MoveNext())
            {
                OnExit();
                State = ModuleState.Done;
                execution = null;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void OnExit();

        protected abstract void Start();

        protected abstract IEnumerator Update();

        #endregion Protected Methods
    }
}