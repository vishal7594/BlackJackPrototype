using UnityEngine;
using DG.Tweening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerInfo playerInfo;
    public TurnTimer turnTimer;
    public NorasTurn norasTurn;

    public Global.GameState gameState;

    [SerializeField] private GameObject cardPrefab, buttonParent;
    [SerializeField] private RectTransform deckGeneratingSpot;
    public System.Collections.Generic.List<GameObject> allCardList;
    public System.Collections.Generic.List<Sprite> allCardSpriteList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start() => CardGenerate();

    public void CardGenerate()
    {
        System.Collections.Generic.List<int> randomNoList = RandomNumberListReturn(0, 52);

        for (int i = 0; i < randomNoList.Count; i++)
        {
            allCardList.Add(Instantiate(cardPrefab, deckGeneratingSpot));
            allCardList[i].GetComponent<UnityEngine.UI.Image>().sprite = allCardSpriteList[randomNoList[i]];
            allCardList[i].name = allCardSpriteList[randomNoList[i]].name;
            SplitString(allCardList[i]);
        }
        StartCoroutine(CardDistributing());
    }

    public void SplitString(GameObject getName)
    {
        string[] objectNumber = getName.name.Split('-');
        int number = int.Parse(objectNumber[1]);
        getName.GetComponent<CardValue>().cardData.cardnumber = number;

        string[] objectName = getName.name.Split('-');
        string cardName = (objectName[0]);
        getName.GetComponent<CardValue>().cardData.nameOfCard = cardName;

        getName.GetComponent<CardValue>().cardData.cardScore = (number >= 10) ? 10 : number;

        getName.GetComponent<CardValue>().cardData.cardScore = number >= 10 ? ((number == 14) ? 11 : 10) : number;
    }

    private System.Collections.Generic.List<int> RandomNumberListReturn(int minValue, int maxValue)
    {
        System.Collections.Generic.List<int> numberList = new System.Collections.Generic.List<int>();

        for (int i = 0; i < maxValue; i++)
        {
        RANDOMNUMBER: int num = Random.Range(minValue, maxValue);

            if (numberList.Contains(num)) goto RANDOMNUMBER;
            numberList.Add(num);
        }
        return numberList;
    }

    private System.Collections.IEnumerator CardDistributing()
    {
        int number = allCardList.Count - 1;
        for (int i = allCardList.Count - 1; i >= 48; i--)
        {
            yield return new WaitForSeconds(0.01f);
            GameObject card = allCardList[i];
            _ = card.transform.DOMove(playerInfo.playerDataList[(i % 2 == 0) ? 0 : 1].allCardsParent.transform.position, 0.5f)
                .OnComplete(() =>
                {
                    card.transform.SetParent(playerInfo.playerDataList[(number % 2 == 0) ? 0 : 1].allCardsParent);
                    card.transform.tag = (number % 2 == 0) ? "AI_Card" : "Player_Card";
                    playerInfo.playerDataList[(number % 2 == 0) ? 0 : 1].cardsList.Add(card);
                    allCardList.Remove(card);
                    if (playerInfo.playerDataList[1].cardsList.Count == 2 && playerInfo.playerDataList[0].cardsList.Count == 2) SetCardSprite();
                    number--;
                });
        }
    }

    private void SetCardSprite()
    {
        for (int i = 0; i < playerInfo.playerDataList[1].cardsList.Count; i++)
        {
            playerInfo.playerDataList[1].cardsList[i].transform.GetChild(0).gameObject.SetActive(false);
            playerInfo.playerDataList[1].cardsList[i].GetComponent<CardValue>().cardData.cardState = CardState.OpenCard;
        }

        turnTimer.TurnRingSetOn();
        PlayerScoreCalculation();
    }

    public int ScoreCountOfGivenCardList(System.Collections.Generic.List<GameObject> playerCardList, TMPro.TMP_Text scoreText)
    {
        Debug.Log("ScoreCountOfGivenCardList");
        int totalScore = 0;
        System.Collections.Generic.List<int> playerCardNumberList = new System.Collections.Generic.List<int>();
        playerCardList.ForEach((playerCard) =>
        {
            if (playerCard.GetComponent<CardValue>().cardData.cardState == CardState.OpenCard)
            {
                totalScore += playerCard.GetComponent<CardValue>().cardData.cardScore;
                playerCardNumberList.Add(playerCard.GetComponent<CardValue>().cardData.cardnumber);
            }
        });

        if (playerCardNumberList.Contains(14) && totalScore > 21) totalScore -= 10;
        /*if (playerCardNumberList.Contains(1) && totalScore != 21)
        {
            if (totalScore > 21)
            {
                totalScore -= 10;
                scoreText.text = totalScore.ToString();
                return totalScore;
            }
            else
            {
                scoreText.text = "" + (totalScore - 10) + "/" + totalScore;
                return totalScore;
            }
        }*/
        scoreText.text = totalScore.ToString();
        return totalScore;
    }

    public void OnClickHit()
    {
        _ = allCardList[allCardList.Count - 1].transform.DOMove(playerInfo.playerDataList[1].allCardsParent.transform.position, 0.5f)
            .OnComplete(() =>
            {
                allCardList[allCardList.Count - 1].transform.SetParent(playerInfo.playerDataList[1].allCardsParent);
                allCardList[allCardList.Count - 1].transform.GetChild(0).gameObject.SetActive(false);
                allCardList[allCardList.Count - 1].GetComponent<CardValue>().cardData.cardState = CardState.OpenCard;
                playerInfo.playerDataList[1].cardsList.Add(allCardList[allCardList.Count - 1]);
                allCardList[allCardList.Count - 1].transform.tag = "Player_Card";
                allCardList.Remove(allCardList[allCardList.Count - 1]);
                PlayerScoreCalculation();
            });
    }

    public void PlayerScoreCalculation()
    {
        Debug.Log("PlayerScoreShow => ");
        playerInfo.playerDataList[1].score = ScoreCountOfGivenCardList(playerInfo.playerDataList[1].cardsList, playerInfo.playerDataList[1].playerScoreText);
        //ButtonDisplay();
    }

    private void ButtonDisplay()
    {
        if(playerInfo.playerDataList[1].score < 21)
        {
            buttonParent.SetActive(true);
        }
    }
}