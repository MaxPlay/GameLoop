using GameLoop;
using GameLoop.Modules;
using GameLoop.Modules.Logic;
using UnityEditor;
using UnityEngine;

namespace GameLoop.Editor.Modules
{
    public class LogicModuleUI : ModuleBaseUI
    {
        #region Protected Fields

        protected LogicModule module;

        #endregion Protected Fields

        #region Private Fields

        private readonly EvaluationModuleUI evaluator;

        #endregion Private Fields

        #region Public Constructors

        public LogicModuleUI(LogicModule module)
        {
            this.module = module;
            evaluator = Create(module.Evaluator) as EvaluationModuleUI;
            if (evaluator != null)
            {
                evaluator.ModuleChanged += EvaluatorChanged;
                evaluator.ForceRefresh += Evaluator_ForceRefresh;
            }
            Name = module.GetType().Name;
        }

        #endregion Public Constructors

        #region Public Properties

        public override Color Background
        {
            get
            {
                return ModuleSystemPreferences.LogicColor;
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
                    if (!(module is CountLogic || module is BreakLogic))
                    {
                        evaluator.OnGUI(builder);
                    }

                    if (module is ICustomEditor)
                        ((ICustomEditor)module).CustomEditor();
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Evaluator_ForceRefresh(ModuleBaseUI module) => OnForceRefresh();

        private void EvaluatorChanged(EvaluationType type, int index, ModuleBuilder builder)
        {
            evaluator.ModuleChanged -= EvaluatorChanged;
            module.Evaluator = builder.CreateModule(builder.EvaluationModuleNamesByType[type][index]) as EvaluationModule;
            OnForceRefresh();
        }

        #endregion Private Methods
    }
}