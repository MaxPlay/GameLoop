using System;
using UnityEngine;

namespace GameLoop.Components
{
    public class GameLoopComponent : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private ModuleSystem moduleSystem;

        #endregion Private Fields

        #region Public Properties

        public ModuleSystem ModuleSystem
        {
            get
            {
                return moduleSystem;
            }
            set
            {
                moduleSystem = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void PauseGame()
        {
            ModuleSystem.Pause();
        }

        public void Start()
        {
            if (ModuleSystem == null)
                throw new Exception("The GameLoop can't be started without a Module System attached");
            ModuleSystem.LoadData();
        }

        public void StartGame()
        {
            ModuleSystem.Start();
        }

        public void StopGame()
        {
            ModuleSystem.Stop();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnApplicationQuit()
        {
            ModuleSystem.Stop();
        }

        private void FixedUpdate()
        {
            ModuleSystem.Update();
        }

        #endregion Private Methods
    }
}