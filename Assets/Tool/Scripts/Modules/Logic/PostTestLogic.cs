using System;

namespace GameLoop.Modules.Logic
{
    [Serializable]
    public class PostTestLogic : LogicModule
    {
        #region Public Constructors

        public PostTestLogic() : base(null)
        {
        }

        public PostTestLogic(EvaluationModule evaluator) : base(evaluator)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public bool PostEvaluate()
        {
            return Evaluator.Evaluate();
        }

        public override void Run()
        {
            OnEvaluated(Evaluator.Evaluate(), LoopType.PostTest);
            State = ModuleState.Done;
        }

        #endregion Public Methods
    }
}