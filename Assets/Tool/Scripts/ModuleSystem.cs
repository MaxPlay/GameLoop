using GameLoop.Compression;
using GameLoop.Data;
using GameLoop.Data.CardCollections;
using GameLoop.Exceptions;
using GameLoop.Internal;
using GameLoop.Modules;
using GameLoop.Modules.Action;
using GameLoop.Modules.Logic;
using GameLoop.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameLoop
{
    public enum SystemState
    {
        Stopped,
        Paused,
        Running
    }

    [CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.CORE_DIRECTORY + "Module System")]
    public partial class ModuleSystem : GameLoopScriptableObject, IList<ModuleBase>
    {
        #region Private Fields

        [SerializeField]
        private bool compressed;

        [SerializeField]
        private DataStorage dataStorage = new DataStorage();

        [SerializeField]
        private bool debug;

        [SerializeField]
        private CardStack initialCardStack;

        [SerializeField]
        private InterfaceControl interfaceControl;

        private Stack<LoopPosition> loopPositions;

        [SerializeField]
        private int maxDepth = -1;

        [SerializeField]
        private List<ModuleBase> modules = new List<ModuleBase>();

        [SerializeField]
        private string serializationData;

        #endregion Private Fields

        #region Public Properties

        public int Count
        {
            get
            {
                return modules.Count;
            }
        }

        public int CurrentDepth { get { return CurrentModule == null ? -1 : CurrentModule.Scope; } }

        public ModuleBase CurrentModule
        {
            get { return CurrentPosition >= modules.Count || CurrentPosition < 0 ? null : modules[CurrentPosition]; }
        }

        public int CurrentPosition { get { return dataStorage.GameState.CurrentModule; } set { dataStorage.GameState.CurrentModule = value; } }

        public DataStorage Data
        {
            get
            {
                return dataStorage;
            }
            set
            {
                dataStorage = value;
            }
        }

        public BreakMode DebuggerMode { get; private set; }

        public bool DebugMode
        {
            get
            {
                return debug;
            }
            set
            {
                debug = value;
            }
        }

        public float DeltaTime { get { return UsesFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime; } }

        public CardStack InitialCardStack
        {
            get { return initialCardStack; }
            set { initialCardStack = value; }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public int MaxDepth
        {
            get
            {
                return maxDepth;
            }

            set
            {
                maxDepth = value;
            }
        }

        public List<ModuleBase> Modules { get { return modules; } }

        public XmlModuleSystemSerializer Serializer { get; set; }

        public SystemState State { get; private set; }

        public InterfaceControl UIControl
        {
            get
            {
                return interfaceControl;
            }
        }

        public bool UseCompression
        {
            get
            {
                return compressed;
            }

            set
            {
                compressed = value;
            }
        }

        public bool UsesFixedDeltaTime { get; set; }

        #endregion Public Properties

        #region Public Indexers

        public ModuleBase this[int index]
        {
            get
            {
                return modules[index];
            }

            set
            {
            }
        }

        #endregion Public Indexers

        #region Public Methods

        public void Add(ModuleBase item)
        {
            item.System = this;
            modules.Add(item);
            if (item is LogicModule)
                ((LogicModule)item).Evaluated += ModuleSystem_Evaluated;
        }

        public void Clear()
        {
            modules.Clear();
        }

        public bool Contains(ModuleBase item)
        {
            return modules.Contains(item);
        }

        public void CopyTo(ModuleBase[] array, int arrayIndex)
        {
        }

        public IEnumerator<ModuleBase> GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)modules).GetEnumerator();
        }

        public int IndexOf(ModuleBase item)
        {
            return modules.IndexOf(item);
        }

        public void Insert(int index, ModuleBase item)
        {
            modules.Insert(index, item);
        }

        public void LoadData()
        {
            string xml = compressed ? GZip.Unzip(Convert.FromBase64String(serializationData)) : serializationData;
            if (Serializer == null)
                Serializer = new XmlModuleSystemSerializer(this);
            Serializer.Deserialize(xml);
        }

        public void Pause()
        {
            if (Application.isEditor && !Application.isPlaying)
                return;

            State = SystemState.Paused;
        }

        public bool Remove(ModuleBase item)
        {
            return modules.Remove(item);
        }

        public void RemoveAt(int index)
        {
            modules.RemoveAt(index);
        }

        public void SaveData()
        {
            dataStorage.SerializeEntries();
            if (Serializer == null)
                Serializer = new XmlModuleSystemSerializer(this);
            string xml = Serializer.Serialize();
            serializationData = compressed ? Convert.ToBase64String(GZip.Zip(xml)) : xml;
        }

        public void SetDataStorage()
        {
            modules.ForEach(m => m.System = this);
        }

        public void Start()
        {
            if (Application.isEditor && !Application.isPlaying)
                return;

            interfaceControl.Setup(this);
            dataStorage.StartGame();
            State = SystemState.Running;
            DebuggerMode = BreakMode.Inactive;
            Setup();
            SetMaxDepth();
            RunNextModule();
            if (DebugMode)
            {
                for (int i = 0; i < modules.Count; i++)
                    Debug.Log(string.Format("{0}, {1}", modules[i].GetType(), modules[i].Scope));
            }
        }

        public void Step()
        {
            if (DebuggerMode == BreakMode.Active && CurrentModule.State == ModuleState.Done)
                RunNextModule();
        }

        public void Stop()
        {
            if (dataStorage.GameState != null)
            {
                if (CurrentModule is InterActionModule)
                    ((InterActionModule)CurrentModule).Reset();

                CurrentPosition = -1;
            }
            State = SystemState.Stopped;
            loopPositions?.Clear();
            dataStorage.StopGame();
        }

        public void Update()
        {
            if (Application.isEditor && !Application.isPlaying)
                if (State != SystemState.Stopped)
                    Stop();


            if (State == SystemState.Running && CurrentModule != null)
                switch (CurrentModule.State)
                {
                    case ModuleState.Running:
                        CurrentModule.Run();
                        break;

                    case ModuleState.Error:
                        Debug.LogError($"{CurrentModule} failed. Check Debugger.");
                        Stop();
                        break;

                    case ModuleState.Done:
                        RunNextModule();
                        break;
                }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Awake()
        {
            base.Awake();
            Serializer = new XmlModuleSystemSerializer(this);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddLoopPosition(int depth, LoopType type)
        {
            loopPositions.Push(new LoopPosition(CurrentPosition, depth, type));
        }

        private void EvaluatePossibleLoop()
        {
            if (loopPositions.Count > 0)
            {
                if (loopPositions.Peek().Depth >= CurrentDepth)
                {
                    if (loopPositions.Peek().Type == LoopType.PostTest)
                    {
                        LoopPosition lp = loopPositions.Pop();
                        if (!(modules[lp.Position] is PostTestLogic))
                            throw new TypeMismatchException(modules[lp.Position].GetType(), typeof(PostTestLogic));
                        PostTestLogic logic = modules[lp.Position] as PostTestLogic;
                        if (logic.PostEvaluate())
                            CurrentPosition = lp.Position;
                    }
                    else
                        CurrentPosition = loopPositions.Pop().Position;
                }
            }
        }

        private void ModuleSystem_Evaluated(LogicModule sender, LogicResult result)
        {
            if (result.Looping == LoopType.Break)
            {
                var lp = loopPositions.Pop();
                SetMaxDepth(lp != null ? lp.Depth : -1);
                return;
            }

            if (result.Success || result.Looping == LoopType.PostTest)
            {
                if (result.Looping != LoopType.None)
                    AddLoopPosition(result.Depth, result.Looping);
            }
            else
                SetMaxDepth(result.Depth);
        }

        private void RunDebug()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CurrentDepth; i++)
            {
                sb.Append(" ");
            }
            Debug.Log($"{CurrentPosition} {sb.ToString()}{CurrentModule?.GetType().Name}");
            if (CurrentModule is PrintAction)
                Debug.Log($"{CurrentPosition} {sb.ToString()}");
        }

        private void RunNextModule()
        {
            CurrentPosition++;

            if (maxDepth != -1)
                SkipMaxDepth();

            EvaluatePossibleLoop();

            if (CurrentModule == null)
            {
                Stop();
                return;
            }

            if (DebugMode)
                RunDebug();

            CurrentModule.Run();
        }

        private void SetMaxDepth(int depth = -1)
        {
            maxDepth = depth;
        }

        private void Setup()
        {
            if (loopPositions == null)
                loopPositions = new Stack<LoopPosition>();
            else
                loopPositions.Clear();
            for (int i = 0; i < modules.Count; i++)
                if (modules[i].System == null)
                    modules[i].System = this;

            Array.ForEach(dataStorage.Players, p => p.System = this);

            if (InitialCardStack != null)
                dataStorage.Setup.Packs.ForEach(p => p.Cards.ForEach(c => InitialCardStack.Cards.LayOnTop(c)));
        }

        private void SkipMaxDepth()
        {
            while (CurrentDepth > maxDepth)
                CurrentPosition++;
            SetMaxDepth();
        }

        #endregion Private Methods

        #region Private Classes

        private class LoopPosition
        {
            #region Public Constructors

            public LoopPosition(int position, int depth, LoopType type)
            {
                Position = position;
                Depth = depth;
                Type = type;
            }

            #endregion Public Constructors

            #region Public Properties

            public int Depth { get; set; }
            public int Position { get; set; }
            public LoopType Type { get; set; }

            #endregion Public Properties
        }

        #endregion Private Classes
    }
}