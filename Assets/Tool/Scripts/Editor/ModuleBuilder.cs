using GameLoop.Editor.Modules;
using GameLoop.Modules;
using GameLoop.Modules.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameLoop.Editor
{
    public class ModuleBuilder
    {
        #region Private Fields

        private readonly Type actionType = typeof(ActionModule);
        private readonly Type interActionType = typeof(InterActionModule);
        private readonly Type baseType = typeof(ModuleBase);
        private readonly Type binaryEvalType = typeof(BinaryEvaluationModule);
        private readonly Type evaluationType = typeof(EvaluationModule);
        private readonly Type logicType = typeof(LogicModule);
        private readonly Type unaryEvalType = typeof(UnaryEvaluationModule);

        private Dictionary<string, Type> moduleLookup;

        #endregion Private Fields

        #region Public Constructors

        public ModuleBuilder()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public string[] ActionModuleNames { get; private set; }
        public string[] InterActionModuleNames { get; private set; }
        public Type[] ActionModules { get; private set; }
        public Type[] InterActionModules { get; private set; }
        public string[] AllModuleNames { get; private set; }
        public Type[] AllModules { get; private set; }
        public string[] EvaluationModuleNames { get; set; }
        public Dictionary<EvaluationType, string[]> EvaluationModuleNamesByType { get; set; }
        public Type[] EvaluationModules { get; private set; }

        public string[] LogicModuleNames { get; private set; }
        public Type[] LogicModules { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public ModuleBase CreateModule(Type type)
        {
            if (!AllModules.Contains(type))
                return null;

            ModuleBase module = Activator.CreateInstance(type) as ModuleBase;
            SetEvaluators(module);
            return module;
        }

        public ModuleBase CreateModule(string name)
        {
            if (!moduleLookup.ContainsKey(name))
                return null;

            ModuleBase module = Activator.CreateInstance(moduleLookup[name]) as ModuleBase;
            SetEvaluators(module);
            return module;
        }

        public T CreateModule<T>() where T : ModuleBase
        {
            ModuleBase module = Activator.CreateInstance<T>();
            SetEvaluators(module);
            return (T)module;
        }

        public void Refresh()
        {
            AllModules = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract).OrderBy(t => t.Name).ToArray();
            ActionModules = AllModules.Where(t => actionType.IsAssignableFrom(t) && !interActionType.IsAssignableFrom(t)).ToArray();
            InterActionModules = AllModules.Where(t => interActionType.IsAssignableFrom(t)).ToArray();
            EvaluationModules = AllModules.Where(t => evaluationType.IsAssignableFrom(t)).ToArray();
            LogicModules = AllModules.Where(t => logicType.IsAssignableFrom(t)).ToArray();

            AllModuleNames = AllModules.Select(t => t.Name).ToArray();
            ActionModuleNames = ActionModules.Select(t => t.Name).ToArray();
            InterActionModuleNames = InterActionModules.Select(t => t.Name).ToArray();
            EvaluationModuleNames = EvaluationModules.Select(t => t.Name).ToArray();
            LogicModuleNames = LogicModules.Select(t => t.Name).ToArray();

            EvaluationModuleNamesByType = new Dictionary<EvaluationType, string[]>
        {
            { EvaluationType.Singular, EvaluationModules.Where(t => !(binaryEvalType.IsAssignableFrom(t) || unaryEvalType.IsAssignableFrom(t))).Select(t => t.Name).ToArray() },
            { EvaluationType.Unary, EvaluationModules.Where(t => unaryEvalType.IsAssignableFrom(t)).Select(t => t.Name).ToArray() },
            { EvaluationType.Binary, EvaluationModules.Where(t => binaryEvalType.IsAssignableFrom(t)).Select(t => t.Name).ToArray() }
        };

            moduleLookup = AllModuleNames.Zip(AllModules, (a, b) => new { a, b }).ToDictionary(kvp => kvp.a, kvp => kvp.b);
        }

        #endregion Public Methods

        #region Private Methods

        private void SetEvaluators(ModuleBase module)
        {
            if (module is LogicModule)
            {
                ((LogicModule)module).Evaluator = CreateModule<FalseEvaluation>();
            }
            else if (module is UnaryEvaluationModule)
            {
                ((UnaryEvaluationModule)module).A = CreateModule<FalseEvaluation>();
            }
            else if (module is BinaryEvaluationModule)
            {
                ((BinaryEvaluationModule)module).A = CreateModule<FalseEvaluation>();
                ((BinaryEvaluationModule)module).B = CreateModule<FalseEvaluation>();
            }
        }

        #endregion Private Methods
    }
}