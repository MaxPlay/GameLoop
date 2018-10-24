using GameLoop;
using GameLoop.Modules;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor.Modules
{

    public class ActionModuleUI : ModuleBaseUI
    {
        #region Protected Fields

        protected ActionModule module;

        #endregion Protected Fields

        #region Public Constructors

        public ActionModuleUI(ActionModule module)
        {
            this.module = module;
            Name = module.GetType().Name;
        }

        #endregion Public Constructors

        #region Public Properties

        public override Color Background
        {
            get
            {
                return ModuleSystemPreferences.ActionColor;
            }
        }

        public override int Depth
        {
            get
            {
                return module.Scope;
            }

            set
            {
                module.Scope = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void OnGUI(ModuleBuilder builder)
        {
            Toolbox(module);
            using (var offset = new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(14);
                Foldout = EditorGUILayout.Foldout(Foldout, "Data");
            }
            if (Foldout)
            {
                using (var inner = new EditorGUILayout.VerticalScope())
                {
                    if (module is ICustomEditor)
                        ((ICustomEditor)module).CustomEditor();
                }
            }
        }

        #endregion Public Methods
    }
}