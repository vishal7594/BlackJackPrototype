using System.Linq;
using UnityEngine;

public class NorasTurn : MonoBehaviour
{
    GameObject selectedCard;
    public void RobotTurn()
    {
        for (int i = 0; i < GameManager.instance.playerInfo.playerDataList[0].cardsList.Count; i++)
        {
            int maxValue = GameManager.instance.playerInfo.playerDataList[0].cardsList.Max(x => x.GetComponent<CardValue>().cardData.cardnumber);

            selectedCard = GameManager.instance.playerInfo.playerDataList[0].cardsList.Find(Y => Y.GetComponent<CardValue>().cardData.cardnumber == maxValue);
            selectedCard.GetComponent<CardValue>().cardData.cardState = CardState.OpenCard;
            Debug.Log("High card => " + selectedCard);
            selectedCard.transform.GetChild(0).gameObject.SetActive(false);
        }
        GameManager.instance.playerInfo.playerDataList[0].score = GameManager.instance.ScoreCountOfGivenCardList(GameManager.instance.playerInfo.playerDataList[0].cardsList, GameManager.instance.playerInfo.playerDataList[0].playerScoreText);
    }
}