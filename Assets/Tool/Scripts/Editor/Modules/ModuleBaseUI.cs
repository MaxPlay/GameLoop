using GameLoop.Modules;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor.Modules
{

    public abstract class ModuleBaseUI
    {
        #region Public Delegates

        public delegate void MoveModuleEventHandler(ModuleBase module);

        public delegate void RefreshModuleEventHandler(ModuleBaseUI module);

        #endregion Public Delegates

        #region Public Events

        public event MoveModuleEventHandler DecreaseDepth;

        public event RefreshModuleEventHandler ForceRefresh;

        public event MoveModuleEventHandler IncreaseDepth;

        public event MoveModuleEventHandler MoveDown;

        public event MoveModuleEventHandler MoveUp;

        public event MoveModuleEventHandler Remove;

        #endregion Public Events

        #region Public Properties

        public abstract Color Background { get; }
        public abstract int Depth { get; set; }
        public bool Foldout { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static ModuleBaseUI Create(ModuleBase module)
        {
            if (module is InterActionModule)
                return new InterActionModuleUI(module as InterActionModule);
            if (module is ActionModule)
                return new ActionModuleUI(module as ActionModule);
            if (module is LogicModule)
                return new LogicModuleUI(module as LogicModule);
            if (module is EvaluationModule)
                return new EvaluationModuleUI(module as EvaluationModule);
            return null;
        }

        public abstract void OnGUI(ModuleBuilder builder);

        public void Toolbox(ModuleBase module)
        {
            using (var toolbox = new GUILayout.HorizontalScope())
            {
                GUILayout.Label(Name, EditorStyles.boldLabel);
                if (GUILayout.Button("<", GUILayout.Width(30)))
                    OnDecreaseDepth(module);
                if (GUILayout.Button(">", GUILayout.Width(30)))
                    OnIncreaseDepth(module);
                if (GUILayout.Button("˄", GUILayout.Width(30)))
                    OnMoveUp(module);
                if (GUILayout.Button("˅", GUILayout.Width(30)))
                    MoveDown(module);
                if (GUILayout.Button("-", GUILayout.Width(30)))
                    OnRemove(module);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnDecreaseDepth(ModuleBase module) => DecreaseDepth?.Invoke(module);

        protected void OnForceRefresh()
        {
            ForceRefresh?.Invoke(this);
        }

        protected void OnIncreaseDepth(ModuleBase module) => IncreaseDepth?.Invoke(module);

        protected void OnMoveDown(ModuleBase module) => MoveDown?.Invoke(module);

        protected void OnMoveUp(ModuleBase module) => MoveUp?.Invoke(module);

        protected void OnRemove(ModuleBase module) => Remove?.Invoke(module);

        #endregion Protected Methods
    }
}