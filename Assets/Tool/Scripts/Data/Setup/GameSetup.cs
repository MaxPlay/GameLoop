using GameLoop.Data.Cards;
using GameLoop.Data.States;
using GameLoop.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Data.Setup
{
    [CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.CORE_DIRECTORY + "Game Setup")]
    public class GameSetup : GameLoopScriptableObject
    {
        #region Public Fields

        public int Dealer;
        public List<Pack> Packs = new List<Pack>();
        public PlayDirection PlayDirection;
        public List<Player> Players = new List<Player>();

        #endregion Public Fields
    }
}