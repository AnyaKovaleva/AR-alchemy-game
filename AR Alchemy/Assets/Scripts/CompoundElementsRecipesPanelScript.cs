using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundElementsRecipesPanelScript : MonoBehaviour
{
    public GameObject recipesPanel;
    public TimerScript timer;

    private void Start()
    {
        recipesPanel.SetActive(false);
    }

    //activates panel with compound element recipes and stops the countdown. Called when "Show recipes" button is pressed
    public void ShowRecipes()
    {
        recipesPanel.SetActive(true);
        timer.StopTimer();
    }

    //hides  panel with compound element recipes and resumes the countdown. Called when "X" button in top right corner of the panel is pressed
    public void HideRecipes()
    {
        recipesPanel.SetActive(false);
        timer.StartTimer();
    }
}
