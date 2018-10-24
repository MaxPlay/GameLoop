using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Action
{
    public partial class WaitAction : ActionModule
    {
        public float WaitTime { get; set; }

        private float passedTime;

        public override void Run()
        {
            State = ModuleState.Running;

            if (passedTime >= WaitTime)
            {
                passedTime = 0;
                State = ModuleState.Done;
            }
            else
                passedTime += System.DeltaTime;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(WaitTime), WaitTime.ToString(CultureInfo.InvariantCulture));
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(WaitTime)))
            {
                float waitTime = 0f;
                if (float.TryParse(reader.ReadInnerXml(), NumberStyles.Any, CultureInfo.InvariantCulture, out waitTime))
                    WaitTime = waitTime;
            }
        }

        protected override void SetDefaults()
        {
            WaitTime = 0f;
        }
    }
#if UNITY_EDITOR
    public partial class WaitAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("This module pauses the game flow for a moment.");
            WaitTime = UnityEditor.EditorGUILayout.FloatField("Seconds", WaitTime);
        }
    }
#endif
}
