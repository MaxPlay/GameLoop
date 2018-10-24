using GameLoop.Data.CardCollections;
using System;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Evaluation
{
    public partial class CardCountEvaluation : EvaluationModule
    {
        public CardStack Stack { get; set; }

        public NumberComparison Comparison { get; set; }

        public int Value { get; set; }

        public override bool Evaluate()
        {
            if (Stack == null)
                return false;

            int count = Stack.Cards.Count;

            switch (Comparison)
            {
                case NumberComparison.Equals:
                    return count == Value;
                case NumberComparison.NotEquals:
                    return count != Value;
                case NumberComparison.GreaterThan:
                    return count > Value;
                case NumberComparison.GreaterThanEquals:
                    return count >= Value;
                case NumberComparison.LessThan:
                    return count < Value;
                case NumberComparison.LessThenEquals:
                    return count <= Value;
                default:
                    return false;
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            if (Stack != null) writer.WriteElementString(nameof(Stack), Stack.ID);
            writer.WriteElementString(nameof(Comparison), Comparison.ToString());
            writer.WriteElementString(nameof(Value), Value.ToString());
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Stack)))
            {
                string id = reader.ReadInnerXml();
                Stack = System.Data.ResolveID(id) as CardStack;
            }
            if (reader.IsStartElement(nameof(Comparison)))
            {
                NumberComparison comparison;
                if (Enum.TryParse(reader.ReadInnerXml(), out comparison))
                    Comparison = comparison;
            }
            if (reader.IsStartElement(nameof(Value)))
            {
                int value;
                if (int.TryParse(reader.ReadInnerXml(), out value))
                    Value = value;
            }
        }

        protected override void SetDefaults()
        {
            Stack = null;
            Comparison = NumberComparison.Equals;
            Value = 0;
        }
    }

#if UNITY_EDITOR

    public partial class CardCountEvaluation : EvaluationModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("Returns [Stack.Cards.Count] [Comparison] [Value]");
            Stack = UnityEditor.EditorGUILayout.ObjectField("Stack", Stack, typeof(CardStack), false) as CardStack;
            Comparison = (NumberComparison)UnityEditor.EditorGUILayout.EnumPopup("Comparison", Comparison);
            Value = UnityEditor.EditorGUILayout.IntField("Value", Value);
        }
    }

#endif
}
