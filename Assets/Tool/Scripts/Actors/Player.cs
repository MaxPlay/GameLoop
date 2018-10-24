using GameLoop.Data.CardCollections;
using GameLoop.Data.Cards;
using GameLoop.Internal;
using UnityEngine;

[CreateAssetMenu(menuName = MenuStructure.MAIN_DIRECTORY + MenuStructure.DATA_DIRECTORY + "Player")]
public class Player : GameLoopScriptableObject
{
    #region Private Fields

    private bool cardSelect;

    [SerializeField]
    private int credits;

    [SerializeField]
    private CardCollection hand;

    [SerializeField]
    private int teamID;

    #endregion Private Fields

    #region Public Properties

    public Card CardToPlay { get; private set; }

    public int Credits
    {
        get
        {
            return credits;
        }

        set
        {
            credits = value;
        }
    }

    public CardCollection Hand
    {
        get
        {
            return hand;
        }

        set
        {
            hand = value;
        }
    }

    public bool IsCurrentPlayer { get { return System != null && System.Data.CurrentPlayer == this; } }

    public int SkatGameValue { get; set; }

    public int TeamID
    {
        get
        {
            return teamID;
        }

        set
        {
            teamID = value;
        }
    }

    #endregion Public Properties

    #region Public Methods

    public void ClearCardToPlay()
    {
        CardToPlay = null;
    }

    public void DisableCardSelect()
    {
        cardSelect = false;
    }

    public void EnableCardSelect()
    {
        cardSelect = true;
        CardToPlay = null;
    }

    public void PlayCard(Card card)
    {
        CardToPlay = (cardSelect) ? card : null;
    }

    #endregion Public Methods
}