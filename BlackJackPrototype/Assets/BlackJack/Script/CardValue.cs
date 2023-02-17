using UnityEngine;

[System.Serializable]
public class CardValue : MonoBehaviour
{
    public CardData cardData = new CardData();
}
public enum CardState
{
    OpenCard,
    CloseCard
}

[System.Serializable]
public class CardData
{
    public int cardnumber;
    public int cardScore;
    public string nameOfCard;
    public CardState cardState;

    public CardData()
    {
        cardnumber = 0;
        cardScore = 0;
        nameOfCard = "";
        cardState = CardState.CloseCard;
    }
}