using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    //text to display the countdown timer
    public TMP_Text timerText;

    public delegate void CountdownEndHandler();
    public event CountdownEndHandler OnCountdownEnd;

    //time player has to earn as many coins as he can
    private int playtimeInSeconds;
    //time left to play
    private float timeLeft;
    private bool isCountdownRunning;

    // Start is called before the first frame update
    void Start()
    {
        isCountdownRunning = false;
        //default play time is 90 seconds
        playtimeInSeconds = 90;
        timeLeft = playtimeInSeconds;
        timerText.color = Color.white;
    }

    public void StartTimer()
    {
        isCountdownRunning = true;
    }

    public void StopTimer()
    {
        isCountdownRunning = false;
    }

    //sets playtime
    public void SetPlaytime(int seconds)
    {

        if (seconds > 0)
        {
            playtimeInSeconds = seconds;
        }
        else
        {
            //if passed parameter is less than 0 then playtime is set to default 90 seconds
            playtimeInSeconds = 90;
        }

        timeLeft = playtimeInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountdownRunning)
        {
            if (timeLeft >= 1)
            {
                //counting down
                timeLeft -= Time.deltaTime;
                DisplayTime();
            }
            else
            {
                isCountdownRunning = false;
                if (OnCountdownEnd != null)
                {
                    //when countdown ends OnCountdownEnd event is invoked
                    OnCountdownEnd.Invoke();
                }

            }
        }
    }

    //displays time left to play in 00:00 format
    private void DisplayTime()
    {
        int minutesLeft = Mathf.FloorToInt(timeLeft / 60);
        int secondsLeft = Mathf.FloorToInt(timeLeft % 60);

        if (minutesLeft == 0 && secondsLeft <= 10)
        {
            //changing color of timer to red when less than 10 seconds left to play
            timerText.color = Color.red;
        }

        timerText.text = string.Format("{0:00}:{1:00}", minutesLeft, secondsLeft);
    }

    public void ResetTimer()
    {
        isCountdownRunning = true;
        timeLeft = playtimeInSeconds;
        timerText.color = Color.white;
    }

    public void AddTime(int seconds)
    {
        if (seconds < 0)
        {
            return;
        }
        timeLeft += seconds;
        DisplayTime();
    }

}
