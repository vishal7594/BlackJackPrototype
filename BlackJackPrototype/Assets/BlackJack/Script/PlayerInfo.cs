using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [System.Serializable]
    public class PlayerDataList
    {
        public TMPro.TMP_Text playerScoreText;
        public int score;
        public RectTransform allCardsParent;
        public UnityEngine.UI.Image playerProfile;
        public System.Collections.Generic.List<GameObject> cardsList;
    }
    public System.Collections.Generic.List<PlayerDataList> playerDataList;
}