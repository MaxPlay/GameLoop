using System;

namespace GameLoop.Modules.Evaluation
{
    [Serializable]
    public class AndEvaluation : BinaryEvaluationModule
    {
        #region Public Constructors

        public AndEvaluation()
        {
        }

        public AndEvaluation(EvaluationModule a, EvaluationModule b)
        {
            A = a;
            B = b;
        }

        #endregion Public Constructors

        #region Public Methods

        public override bool Evaluate()
        {
            return A.Evaluate() && B.Evaluate();
        }

        #endregion Public Methods
    }
}