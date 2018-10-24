using System;
using System.Xml;

namespace GameLoop.Modules
{
    [Serializable]
    public abstract class BinaryEvaluationModule : EvaluationModule
    {
        #region Public Properties

        public EvaluationModule A { get; set; }

        public EvaluationModule B { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(A));
            writer.WriteStartElement(A.GetType().FullName);
            A.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement(nameof(B));
            writer.WriteStartElement(B.GetType().FullName);
            B.WriteXml(writer);
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
            if (reader.IsStartElement(nameof(B)))
            {
                reader.Read();
                string typeName = reader.Name;
                B = Activator.CreateInstance(Type.GetType(typeName)) as EvaluationModule;
                B.System = System;
                B.ReadXml(reader);
                reader.Read();
            }
        }

        protected override void SetDefaults()
        {
            A = null;
            B = null;
        }

        protected override void UpdateModuleSystemInChildren()
        {
            if (A != null)
                A.System = System;
            if (B != null)
                B.System = System;
        }

        #endregion Protected Methods
    }
}