using System;
using System.Xml;

namespace GameLoop.Modules
{
    [Serializable]
    public abstract class UnaryEvaluationModule : EvaluationModule
    {
        #region Public Properties

        public EvaluationModule A { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(A));
            writer.WriteStartElement(A.GetType().FullName);
            A.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(A)))
            {
                reader.Read();
                string typeName = reader.Name;
                A = Activator.CreateInstance(Type.GetType(typeName)) as EvaluationModule;
                A.System = System;
                A.ReadXml(reader);
                reader.Read();
            }
        }

        protected override void SetDefaults()
        {
            A = null;
        }

        protected override void UpdateModuleSystemInChildren()
        {
            if (A != null)
                A.System = System;
        }

        #endregion Protected Methods
    }
}