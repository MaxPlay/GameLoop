using System;

namespace GameLoop.Modules
{
    [Serializable]
    public abstract class EvaluationModule : ModuleBase
    {
        #region Public Methods

        public override void Run() { }

        public abstract bool Evaluate();

        #endregion Public Methods
    }
}