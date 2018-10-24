using GameLoop.Data.CardCollections;
using GameLoop.Data.Cards;
using GameLoop.Data.Setup;
using GameLoop.Data.States;
using GameLoop.Exceptions;
using GameLoop.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameLoop
{
    [Serializable]
    public class DataStorage
    {
        #region Public Fields

        public List<CardStack> CardStacks = new List<CardStack>();

        public GameSetup Setup;

        #endregion Public Fields

        #region Private Fields

        private GameState gameState;

        [SerializeField]
        [HideInInspector]
        private List<SOEntry> soLookup = new List<SOEntry>();

        #endregion Private Fields

        #region Public Delegates

        public delegate void CurrentPlayerChangedEventHandler(Player player);

        #endregion Public Delegates

        #region Public Events

        public event CurrentPlayerChangedEventHandler CurrentPlayerChanged;

        #endregion Public Events

        #region Public Properties

        public Player CurrentPlayer { get { return gameState.Players[gameState.CurrentPlayer]; } }

        public int CurrentPlayerID
        {
            get
            {
                return gameState.CurrentPlayer;
            }

            set
            {
                if (value < 0)
                {
                    gameState.CurrentPlayer = 0;
                    return;
                }

                if (value >= gameState.Players.Length)
                {
                    gameState.CurrentPlayer = gameState.Players.Length - 1;
                    return;
                }

                gameState.CurrentPlayer = value;
            }
        }

        public Player Dealer
        {
            get { return gameState.Players[gameState.CurrentDealer]; }
        }

        public int DealerID
        {
            get { return gameState.CurrentDealer; }
            set
            {
                if (value < 0)
                {
                    gameState.CurrentDealer = 0;
                    return;
                }

                if (value >= gameState.Players.Length)
                {
                    gameState.CurrentDealer = gameState.Players.Length - 1;
                    return;
                }

                gameState.CurrentDealer = value;
            }
        }

        public GameState GameState
        {
            get
            {
                return gameState;
            }

            set
            {
                gameState = value;
            }
        }

        public Player NextPlayer { get { return Players[NextPlayerID]; } }

        public int NextPlayerID { get { return GetNextPlayer(CurrentPlayerID); } }

        public Player[] Players
        {
            get
            {
                return gameState == null ? Setup.Players.ToArray() : gameState.Players;
            }
        }

        public Player PreviousPlayer { get { return Players[PreviousPlayerID]; } }

        public int PreviousPlayerID { get { return GetPreviousPlayer(CurrentPlayerID); } }

        public List<SOEntry> SoLookup
        {
            get
            {
                return soLookup;
            }
            private set
            {
                soLookup = value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public int GetID(Player player)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].ID == player.ID)
                    return i;
            }
            return -1;
        }

        public int GetNextPlayer(int playerID)
        {
            switch (gameState.Direction)
            {
                case PlayDirection.Clockwise:
                    playerID = NextPlayerClockwise(playerID);
                    break;

                case PlayDirection.CounterClockwise:
                    playerID = NextPlayerCounterClockwise(playerID);
                    break;
            }
            return playerID;
        }

        public int GetPreviousPlayer(int playerID)
        {
            switch (gameState.Direction)
            {
                case PlayDirection.CounterClockwise:
                    playerID = NextPlayerClockwise(playerID);
                    break;

                case PlayDirection.Clockwise:
                    playerID = NextPlayerCounterClockwise(playerID);
                    break;
            }
            return playerID;
        }

        public void LoadGame(string file)
        {
            gameState = JsonUtility.FromJson<GameState>(File.ReadAllText(file));
        }

        public GameLoopScriptableObject ResolveID(string id)
        {
            if (soLookup == null)
            {
                soLookup = new List<SOEntry>();
                BuildSoLookup();
            }

            return soLookup.Find(e => e.ID == id)?.ScriptableObject;
        }

        public void SaveGame(string file)
        {
            if (File.Exists(file))
                File.Delete(file);

            File.WriteAllText(file, JsonUtility.ToJson(gameState));
        }

        public void SerializeEntries()
        {
            if (SoLookup == null)
                SoLookup = new List<SOEntry>();
            BuildSoLookup();
        }

        public void SetCurrentPlayer(int id)
        {
            if (GameState == null)
                return;
            gameState.CurrentPlayer = id;
            OnCurrentPlayerChanged();
        }

        public void SetNextPlayerTurn()
        {
            gameState.CurrentPlayer = GetNextPlayer(gameState.CurrentPlayer);
            OnCurrentPlayerChanged();
        }

        public void SetPreviousPlayerTurn()
        {
            gameState.CurrentPlayer = GetPreviousPlayer(gameState.CurrentPlayer);
            OnCurrentPlayerChanged();
        }

        public void SetPlayerTurn(int id)
        {
            if (gameState.CurrentPlayer == id)
                return;
            if (id < 0 || id >= Players.Length)
                return;

            gameState.CurrentPlayer = id;
            OnCurrentPlayerChanged();
        }

        public void StartGame()
        {
            if (Setup == null)
                throw new NoSetupException("There is no Setup to start a game from.", new NullReferenceException("State is null."));

            CardStacks.ForEach(c => c.Cards.DataStorage = this);
            Array.ForEach(Players, c => c.Hand.DataStorage = this);
            gameState = ScriptableObject.CreateInstance<GameState>();
            gameState.CurrentModule = -1;
            gameState.CardStacks = CardStacks.ToArray();
            gameState.CurrentDealer = Setup.Dealer;
            gameState.CurrentPlayer = Setup.Dealer;
            gameState.Direction = Setup.PlayDirection;
            gameState.Packs = Setup.Packs.ToArray();
            gameState.Players = Setup.Players.ToArray();
            gameState.Pot = new BiddingPot(gameState.Players);
            gameState.Trump = Suit.None;
            OnCurrentPlayerChanged();
        }

        public void StopGame()
        {
            if (gameState != null)
                if (!Application.isPlaying && Application.isEditor)
                    ScriptableObject.DestroyImmediate(gameState);
                else
                    ScriptableObject.Destroy(gameState);
            
            if(!Setup)
                return;

            for (int i = 0; i < Players.Length; i++)
                Players[i].Hand.Clear();
            for (int i = 0; i < CardStacks.Count; i++)
                CardStacks[i].Cards.Clear();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void OnCurrentPlayerChanged()
        {
            CurrentPlayerChanged?.Invoke(CurrentPlayer);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddListToLookup<T>(IList<T> assets) where T : GameLoopScriptableObject
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (!AddToLookup(assets[i]))
                    continue;
            }
        }

        private bool AddToLookup<T>(T asset) where T : GameLoopScriptableObject
        {
            if (asset == null)
                return false;

            if (SoLookup.Exists(e => e.ScriptableObject == asset))
                return false;

            while (string.IsNullOrEmpty(asset.ID) || asset.ID == Guid.Empty.ToString() || SoLookup.Exists(e => e.ID == asset.ID))
                asset.GenerateNewID();
            SoLookup.Add(new SOEntry(asset.ID, asset));
            return true;
        }

        private void BuildSoLookup()
        {
            SoLookup.Clear();
            if (Setup != null)
            {
                AddToLookup(Setup);
                if (Setup.Packs != null) AddListToLookup(Setup.Packs);
                if (Setup.Players != null) AddListToLookup(Setup.Players);
            }
            if (CardStacks != null) AddListToLookup(CardStacks);
        }

        private int NextPlayerClockwise(int playerID)
        {
            playerID++;

            if (playerID >= gameState.Players.Length)
                playerID = 0;
            return playerID;
        }

        private int NextPlayerCounterClockwise(int playerID)
        {
            playerID--;

            if (playerID < 0)
                playerID = gameState.Players.Length - 1;
            return playerID;
        }

        #endregion Private Methods

        #region Public Classes

        [Serializable]
        public class SOEntry
        {
            #region Public Fields

            public string ID;
            public GameLoopScriptableObject ScriptableObject;

            #endregion Public Fields

            #region Public Constructors

            public SOEntry(string id, GameLoopScriptableObject testAsset)
            {
                ID = id;
                ScriptableObject = testAsset;
            }

            #endregion Public Constructors
        }

        #endregion Public Classes
    }
}