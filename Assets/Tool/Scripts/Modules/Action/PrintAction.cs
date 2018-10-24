using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Action
{
    [Serializable]
    public partial class PrintAction : ActionModule
    {
        #region Public Constructors

        public PrintAction()
        {
            Text = string.Empty;
        }

        public PrintAction(object obj)
        {
            Text = obj.ToString();
        }

        public PrintAction(string text)
        {
            Text = text;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Text { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Run()
        {
            Debug.Log(Text);
            State = ModuleState.Done;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Text", Text ?? string.Empty);
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement("Text"))
                Text = reader.ReadInnerXml();
        }

        protected override void SetDefaults()
        {
            Text = "";
        }

        #endregion Public Methods
    }

#if UNITY_EDITOR

    public partial class PrintAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            Text = UnityEditor.EditorGUILayout.TextField("Text", Text);
        }
    }

#endif
}