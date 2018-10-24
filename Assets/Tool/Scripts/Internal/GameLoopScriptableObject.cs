using System;
using UnityEngine;

namespace GameLoop.Internal
{
    public class GameLoopScriptableObject : ScriptableObject
    {
        #region Private Fields

        [SerializeField]
        private string id;

        #endregion Private Fields

        #region Public Properties

        public string ID
        {
            get
            {
                return id;
            }
        }

        public ModuleSystem System { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void GenerateNewID()
        {
            id = Guid.NewGuid().ToString();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(id))
                GenerateNewID();
        }

        #endregion Protected Methods
    }
}