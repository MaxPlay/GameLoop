using System;
using System.Xml;

namespace GameLoop.Modules.Evaluation
{
    [Serializable]
    public partial class IsCurrentPlayerEvaluation : EvaluationModule
    {
        #region Public Properties

        public Player Player { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override bool Evaluate()
        {
            return System.Data.CurrentPlayer == Player;
        }

        public override void WriteXml(XmlWriter writer)
        {
            if (Player != null) writer.WriteElementString(nameof(Player), Player.ID);
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Player)))
            {
                string id = reader.ReadInnerXml();
                Player = System.Data.ResolveID(id) as Player;
            }
        }

        protected override void SetDefaults()
        {
            Player = null;
        }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class IsCurrentPlayerEvaluation : EvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            Player = UnityEditor.EditorGUILayout.ObjectField("Player", Player, typeof(Player), false) as Player;
        }
    }

#endif
}