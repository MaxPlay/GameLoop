using GameLoop.Exceptions;
using GameLoop.Modules;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace GameLoop.Serialization
{
    public class XmlModuleSystemSerializer
    {
        #region Private Fields

        private const string ROOT_STRING = "Modules";

        #endregion Private Fields

        #region Public Constructors

        public XmlModuleSystemSerializer(ModuleSystem moduleSystem)
        {
            ModuleSystem = moduleSystem;
        }

        #endregion Public Constructors

        #region Public Properties

        public ModuleSystem ModuleSystem { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Deserialize(string xml)
        {
            ModuleSystem.Clear();
            if (string.IsNullOrWhiteSpace(xml))
                return;
            using (TextReader reader = new StringReader(xml))
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement(ROOT_STRING))
                    {
                        ReadModules(xmlReader);
                    }
                }
            }
        }

        public string Serialize()
        {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(builder, new XmlWriterSettings() { Encoding = Encoding.UTF8 }))
            {
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement(ROOT_STRING);

                for (int i = 0; i < ModuleSystem.Count; i++)
                {
                    ModuleBase module = ModuleSystem[i];
                    xmlWriter.WriteStartElement(module.GetType().FullName);
                    xmlWriter.WriteElementString(nameof(module.Scope), module.Scope.ToString());
                    ModuleSystem[i].WriteXml(xmlWriter);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            return builder.ToString();
        }

        #endregion Public Methods

        #region Private Methods

        private void ReadModules(XmlReader xmlReader)
        {
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    Type moduleType = Type.GetType(xmlReader.Name);
                    if (moduleType == null)
                        throw new TypeMismatchException(typeof(ModuleBase), null, true);

                    ModuleBase module = Activator.CreateInstance(moduleType) as ModuleBase;
                    if (module == null)
                        throw new TypeMismatchException(typeof(ModuleBase), moduleType, true);

                    module.System = ModuleSystem;
                    module.ReadXml(xmlReader);
                    ModuleSystem.Add(module);
                }
            }
        }

        #endregion Private Methods
    }
}