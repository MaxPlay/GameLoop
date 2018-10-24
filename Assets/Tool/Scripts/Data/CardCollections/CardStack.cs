using GameLoop.Internal;
using UnityEngine;

namespace GameLoop.Data.CardCollections
{
    [CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.COLLECTION_DIRECTORY + "Card Stack")]
    public partial class CardStack : GameLoopScriptableObject
    {
        #region Private Fields

        [SerializeField]
        private CardCollection collection;

        [SerializeField]
        private CardVisibility visibility;

        #endregion Private Fields

        #region Public Properties

        public CardCollection Cards
        {
            get
            {
                return collection;
            }

            set
            {
                collection = value;
            }
        }

        public CardVisibility Visibility
        {
            get
            {
                return visibility;
            }

            set
            {
                visibility = value;
            }
        }

        #endregion Public Properties
    }
}