using System;

namespace GameLoop.Modules.Logic
{
    [Serializable]
    public class PreTestLogic : LogicModule
    {
        #region Public Constructors

        public PreTestLogic(EvaluationModule evaluator) : base(evaluator)
        {
        }

        public PreTestLogic() : base(null)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            OnEvaluated(Evaluator.Evaluate(), LoopType.PreTest);
            State = ModuleState.Done;
        }

        #endregion Public Methods
    }
}