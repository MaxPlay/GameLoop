using System;

namespace GameLoop.Modules.Logic
{
    [Serializable]
    public class ConditionLogic : LogicModule
    {
        #region Public Constructors

        public ConditionLogic() : base(null)
        {
        }

        public ConditionLogic(EvaluationModule evaluator) : base(evaluator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            OnEvaluated(Evaluator.Evaluate());
            State = ModuleState.Done;
        }

        #endregion Public Methods
    }
}