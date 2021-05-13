using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelScript : MonoBehaviour
{
    public TimerScript timer;

    //called when "Play!" button is pressed. Hides start panel and starts the timer
    public void Play()
    {
        gameObject.SetActive(false);
        timer.SetPlaytime(90);
        timer.StartTimer();
    }
}
