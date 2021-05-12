using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;

public class EndGamePanelScript : MonoBehaviour
{
    public TMP_Text coinCountText;
    public TMP_Text outputCoinCount;
    public TimerScript timer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //when countdown ends ActivateEndGamePanel will fire
        timer.OnCountdownEnd += ActivateEndGamePanel;
    }

    //Activates panel when playtime is up. Shows how many coins you managed to earn 
    public void ActivateEndGamePanel()
    {
        gameObject.SetActive(true);
        outputCoinCount.text = "Тебе удалось заработать " + coinCountText.text + " монеток!";
    }

    //Hides endgame panel, resets timer and coinCount. Called when "Play again" button is pressed
    public void RestartGame()
    {
        gameObject.SetActive(false);
        coinCountText.text = "0";
        timer.ResetTimer();
    }
}
