using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Action
{
    public partial class NextPlayerAction : ActionModule
    {
        public override void Run()
        {
            System.Data.SetNextPlayerTurn();
        }

        public override void WriteXml(XmlWriter writer) { }

        protected override void ParseXmlElement(XmlReader reader) { }

        protected override void SetDefaults() { }
    }
#if UNITY_EDITOR

    public partial class NextPlayerAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("Let's the next player have her turn.");
        }
    }

#endif
}
