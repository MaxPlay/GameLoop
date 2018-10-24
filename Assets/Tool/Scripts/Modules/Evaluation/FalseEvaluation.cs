using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Evaluation
{
    [Serializable]
    public partial class FalseEvaluation : EvaluationModule
    {
        #region Public Methods

        public override bool Evaluate()
        {
            return false;
        }

        public override void WriteXml(XmlWriter writer) { }

        protected override void ParseXmlElement(XmlReader reader) { }

        protected override void SetDefaults() { }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class FalseEvaluation : EvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("This module will always yield false.");
        }
    }

#endif
}