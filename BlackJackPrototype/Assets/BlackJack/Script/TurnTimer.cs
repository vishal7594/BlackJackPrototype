using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image turnRingImage, timerBgImage;
    [SerializeField] private TMPro.TMP_Text timerText;
    [SerializeField] private float remainTime, turnTime;

    public void TurnRingSetOn()
    {
        GameManager.instance.gameState = Global.GameState.Card_Display_Timer;
        remainTime = turnTime;
        turnRingImage.enabled = true;
        timerBgImage.enabled = true;
        timerText.enabled = true;
        InvokeRepeating(nameof(UpdateTime), 0, 0.02f);
    }

    private void UpdateTime()
    {
        if (remainTime > 0)
        {
            timerText.text = ((int)Mathf.Round(remainTime)) + "s";
            remainTime -= Time.deltaTime;
            turnRingImage.fillAmount = remainTime / turnTime;
        }
        else
        {
            GameManager.instance.norasTurn.RobotTurn();
            //GameManager.instance.PlayerScoreCalculation();
            TurnRingSetOff();
            CancelInvoke(nameof(UpdateTime));
        }
    }

    public void TurnRingSetOff()
    {
        GameManager.instance.gameState = Global.GameState.Idle;
        turnRingImage.enabled = false;
        timerBgImage.enabled = false;
        timerText.enabled = false;
        turnRingImage.fillAmount = 1;
    }
}