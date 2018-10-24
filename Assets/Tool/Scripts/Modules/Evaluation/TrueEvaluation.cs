using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Evaluation
{
    [Serializable]
    public partial class TrueEvaluation : EvaluationModule
    {
        #region Public Methods

        public override bool Evaluate()
        {
            return true;
        }

        public override void WriteXml(XmlWriter writer) { }

        protected override void ParseXmlElement(XmlReader reader) { }

        protected override void SetDefaults() { }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class TrueEvaluation : EvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("This module will always yield true.");
        }
    }

#endif
}