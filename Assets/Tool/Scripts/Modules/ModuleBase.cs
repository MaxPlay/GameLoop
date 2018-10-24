using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace GameLoop.Modules
{
    [Serializable]
    public abstract class ModuleBase : IXmlSerializable
    {
        #region Private Fields

        private ModuleSystem data;

        #endregion Private Fields

        #region Public Constructors

        public ModuleBase()
        {
            State = ModuleState.Done;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Scope { get; set; }

        public bool BreakPoint { get; set; }

        [XmlIgnore]
        public ModuleState State { get; protected set; }

        [XmlIgnore]
        public ModuleSystem System
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                UpdateModuleSystemInChildren();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            SetDefaults();
            int _depth = 0;
            int _breakPoint = 0;
            Scope = 0;
            BreakPoint = false;
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.IsEmptyElement)
                {
                    reader.Read();
                    continue;
                }

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.IsStartElement((nameof(Scope))))
                    {
                        if (int.TryParse(reader.ReadInnerXml(), out _depth))
                            Scope = _depth;
                    }
                    if(reader.IsStartElement((nameof(BreakPoint))))
                    {
                        if (int.TryParse(reader.ReadInnerXml(), out _breakPoint))
                            BreakPoint = _breakPoint > 0;
                    }
                    else
                        ParseXmlElement(reader);
                }
            }
        }

        public abstract void Run();

        public abstract void WriteXml(XmlWriter writer);

        #endregion Public Methods

        #region Protected Methods

        protected abstract void ParseXmlElement(XmlReader reader);

        protected abstract void SetDefaults();

        protected virtual void UpdateModuleSystemInChildren()
        {
        }

        #endregion Protected Methods
    }
}