using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Logic
{
    [Serializable]
    public partial class BreakLogic : LogicModule
    {
        #region Public Constructors

        public BreakLogic() : base(null)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Run()
        {
            OnEvaluated(false, LoopType.Break);
            State = ModuleState.Done;
        }

        public override void WriteXml(XmlWriter writer)
        {
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
        }

        protected override void SetDefaults()
        {
        }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class BreakLogic : LogicModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("This module will break the current loop.");
        }
    }

#endif
}