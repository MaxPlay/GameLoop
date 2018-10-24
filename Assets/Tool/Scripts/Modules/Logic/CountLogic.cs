using System;
using System.Xml;

namespace GameLoop.Modules.Logic
{
    [Serializable]
    public partial class CountLogic : LogicModule
    {
        #region Private Fields

        private int currentCount;

        #endregion Private Fields

        #region Public Constructors

        public CountLogic(int count) : base(null)
        {
            Count = count;
        }

        public CountLogic() : base(null)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int Count { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Run()
        {
            State = ModuleState.Running;
            bool result = currentCount++ < Count;
            OnEvaluated(result, LoopType.PreTest);
            if (!result)
                currentCount = 0;
            State = ModuleState.Done;
        }

        #endregion Public Methods

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Count), Count.ToString());
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Count)))
            {
                int count = 0;
                if (int.TryParse(reader.ReadInnerXml(), out count))
                    Count = count;
            }
        }

        protected override void SetDefaults()
        {
            Count = 0;
        }
    }

#if UNITY_EDITOR

    public partial class CountLogic : LogicModule, ICustomEditor
    {
        public void CustomEditor()
        {
            Count = UnityEditor.EditorGUILayout.IntField("Count", Count);
        }
    }

#endif
}