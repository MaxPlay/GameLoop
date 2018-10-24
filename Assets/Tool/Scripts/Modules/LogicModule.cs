using System;
using System.Xml;

namespace GameLoop.Modules
{
    [Serializable]
    public abstract class LogicModule : ModuleBase
    {
        #region Public Constructors

        public LogicModule(EvaluationModule evaluator)
        {
            Evaluator = evaluator;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void LogicResultHandler(LogicModule sender, LogicResult result);

        #endregion Public Delegates

        #region Public Events

        public event LogicResultHandler Evaluated;

        #endregion Public Events

        #region Public Properties

        public EvaluationModule Evaluator { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void OnEvaluated(bool result, LoopType looping = LoopType.None)
        {
            Evaluated?.Invoke(this, new LogicResult() { Depth = Scope, Success = result, Looping = looping });
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(Evaluator));
            writer.WriteStartElement(Evaluator.GetType().FullName);
            Evaluator.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;
            reader.Read();
            string typeName = reader.Name;
            Evaluator = Activator.CreateInstance(Type.GetType(typeName)) as EvaluationModule;
            Evaluator.System = System;
            Evaluator.ReadXml(reader);
        }

        protected override void SetDefaults()
        {
            Evaluator = null;
        }

        protected override void UpdateModuleSystemInChildren()
        {
            if (Evaluator != null)
                Evaluator.System = System;
        }

        #endregion Protected Methods
    }

    public class LogicResult
    {
        #region Public Properties

        public int Depth { get; set; }
        public LoopType Looping { get; set; }
        public bool Success { get; set; }

        #endregion Public Properties
    }
}