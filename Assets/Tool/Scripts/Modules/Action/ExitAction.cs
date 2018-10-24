using System.Xml;

namespace GameLoop.Modules.Action
{
    public partial class ExitAction : ActionModule
    {
        public override void Run()
        {
            System.Stop();
        }

        public override void WriteXml(XmlWriter writer) { }

        protected override void ParseXmlElement(XmlReader reader) { }

        protected override void SetDefaults() { }
    }

#if UNITY_EDITOR

    public partial class ExitAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            UnityEngine.GUILayout.Label("This exits the gameloop.");
        }
    }

#endif
}