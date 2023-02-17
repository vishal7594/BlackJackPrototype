using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ClickOnCard : MonoBehaviour, IPointerClickHandler
{
    private GameObject clickedCard;
    public void OnPointerClick(PointerEventData eventData)
    {
        clickedCard = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("<color=cyan>Card name => " + clickedCard.name + "</color>");
        if (clickedCard.CompareTag("Player_Card") && GameManager.instance.gameState == Global.GameState.Card_Display_Timer) _ = clickedCard.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.3f).OnComplete(() => clickedCard.transform.DOScale(Vector3.one, 0.2f));
    }
}
