using GameLoop.Data;
using GameLoop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace GameLoop.Modules.Action
{
    public partial class DisplayUIAction : ActionModule
    {
        public bool Show { get; set; }

        public int SelectedType { get; set; }

        public override void Run()
        {
            if (UnlinkedInterface.AssignableTypes.Count == 0)
                return;

            if (System.UIControl == null)
                State = ModuleState.Error;
            else
                InterfaceControl.DisplayUIReflection.MakeGenericMethod(UnlinkedInterface.AssignableTypes[SelectedType]).Invoke(System.UIControl, new object[] { Show });
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Show), (Show ? "1" : "0"));
        }

        protected override void ParseXmlElement(XmlReader reader)
        {
            if (reader.IsStartElement(nameof(Show)))
            {
                int showValue = 0;
                if (int.TryParse(reader.ReadInnerXml(), out showValue)) { }
                Show = showValue > 0;
            }
            if (reader.IsStartElement(nameof(SelectedType)))
            {
                int selected = 0;
                if (int.TryParse(reader.ReadInnerXml(), out selected))
                    SelectedType = selected;
            }
        }

        protected override void SetDefaults()
        {
            Show = false;
            SelectedType = 0;
        }
    }

#if UNITY_EDITOR

    public partial class DisplayUIAction : ActionModule, ICustomEditor
    {
        public void CustomEditor()
        {
            GUILayout.Label("Enables or disables the given UI.");
            Show = UnityEditor.EditorGUILayout.Toggle("Show UI", Show);
            using (var scope = new UnityEditor.EditorGUI.DisabledScope(UnlinkedInterface.AssignableTypes.Count == 0))
                SelectedType = UnityEditor.EditorGUILayout.Popup("Selected Interface", SelectedType, UnlinkedInterface.AssignableTypeNames);
        }
    }

#endif
}
