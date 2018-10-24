using System;
using UnityEngine;

namespace GameLoop.Modules.Evaluation
{
    [Serializable]
    public partial class NotEvaluation : UnaryEvaluationModule
    {
        #region Public Constructors

        public NotEvaluation()
        {
        }

        public NotEvaluation(EvaluationModule a)
        {
            A = a;
        }

        #endregion Public Constructors

        #region Public Methods

        public override bool Evaluate()
        {
            return !A.Evaluate();
        }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class NotEvaluation : UnaryEvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("This module will always yield true.");
        }
    }

#endif
}