using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Evaluation
{
    public partial class NoCardsLeftToPlayEvaluation : EvaluationModule
    {
        public override bool Evaluate()
        {
            for (int i = 0; i < System.Data.Players.Length; i++)
            {
                if (System.Data.Players[i].Hand.Count > 0)
                    return false;
            }
            return true;
        }

        public override void WriteXml(XmlWriter writer) { }

        protected override void ParseXmlElement(XmlReader reader) { }

        protected override void SetDefaults() { }
    }

#if UNITY_EDITOR
    public partial class NoCardsLeftToPlayEvaluation : EvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("Returns true when all players are out of cards.");
        }
    }
#endif
}
