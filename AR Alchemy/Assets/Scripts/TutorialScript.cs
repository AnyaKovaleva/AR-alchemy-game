using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//script that controls all tutorials
public class TutorialScript : MonoBehaviour
{
    public GameObject startPanel;
    //lists with UI panels for each tutorial
    //[0]Task1_1, [1]Tutorial 1 Completed
    public List<GameObject> tutorial1;
    //[0]Task1_2, [1] Tutorial 2 Completed
    public List<GameObject> tutorial2;
    //[0]Task1_3, [1]Task2_3, [2]Task3_3, [3]Tutorial 3 Completed
    public List<GameObject> tutorial3;
    //[0]Task1_4, [1]Task2_4, [2]Task3_4, [3]Task4_4, [4]Task5_4, [5]Tutorial 4 Completed
    public List<GameObject> tutorial4;

    //potion recipes to cook during tutorials
    public RecipeObject newRecipeForTutorial2;
    public RecipeObject newRecipeForTutorial3;
    public RecipeObject newRecipeForTutorial4;

    //cards for 1st and 2nd recipe
    public BaseElementCard blueMountainFlowerCard;
    public BaseElementCard lutiMushroomCard;

    //compound element cards. Tutorial 4
    public CompoundElementCard compoundElementCard;
    public CompoundElementCard compoundElementCard1;

    public GameObject compoundElementsRecipePanel;

    //compound elements for 3d recipe
    public CompoundElementObject water;
    public CompoundElementObject jimsonweed;


    public PotionMakingSkript transmutationCircleCard;
    public GameObject generateRecipeButton;

    public ElementMixingScript mixingScript;

    //UIs on screen which will be turned off/on during tutorials
    public GameObject coinCount;
    public GameObject timer;
    public GameObject resetCompoundElementButton;
    public GameObject resetCompoundElement1Button;
    public GameObject currentCompoundElement;
    public GameObject showRecipesButton;

    //bools to understand on which tutorial player currently is
    private bool isActiveTutorial1;
    private bool isActiveTutorial2;
    private bool isActiveTutorial3;
    private bool isActiveTutorial4;

    //bools to check if player has pointed his camera at transmutation circle card. Tutorial 1
    private bool trackedTransmutationCircle;
    //bools to check if player has pointed his camera at transmutation circle card. Tutorial 4
    private bool trackedCompoundElementCard;

    //bools to iterate between tasks of tutorials
    private bool isActiveTask1;
    private bool isActiveTask2;
    private bool isActiveTask3;
    private bool isActiveTask4;
    private bool isActiveTask5;

    // Start is called before the first frame update
    void Start()
    {
        isActiveTutorial1 = false;
        isActiveTutorial2 = false;
        isActiveTutorial3 = false;
        isActiveTutorial4 = false;

        isActiveTask1 = false;
        isActiveTask2 = false;
        isActiveTask3 = false;
        isActiveTask4 = false;
        isActiveTask5 = false;

        trackedTransmutationCircle = false;
        trackedCompoundElementCard = false;

        //deactivating all tutorials' panels on screen
        foreach (GameObject gameObject in tutorial1)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in tutorial2)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in tutorial3)
        {
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in tutorial4)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Tutorial 1
        if (isActiveTutorial1)
        {
            if (trackedTransmutationCircle)
            {
                //completed Task1_1
                tutorial1[0].SetActive(false);
                tutorial1[1].SetActive(true);
                isActiveTutorial1 = false;
            }
        }

        //Tutorial 2
        if (isActiveTutorial2)
        {
            if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
            {
                //if player placed cards incorrectly we reset recipe so he could try again
                transmutationCircleCard.SetRecipe(newRecipeForTutorial2);
            }
            if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
            {
                //Completed task1_2
                tutorial2[0].SetActive(false);
                tutorial2[1].SetActive(true);
                isActiveTutorial2 = false;
                coinCount.SetActive(true);
                timer.SetActive(true);
            }
        }

        //Tutorial 3
        if (isActiveTutorial3)
        {
            if (isActiveTask1)
            {
                if (blueMountainFlowerCard.GetTemperature() == Temperature.COLD)
                {
                    //completed task1_3
                    tutorial3[0].SetActive(false);
                    tutorial3[1].SetActive(true);
                    isActiveTask1 = false;
                    isActiveTask2 = true;
                }
            }

            if (isActiveTask2)
            {
                if (lutiMushroomCard.GetTemperature() == Temperature.HOT)
                {
                    //completed task2_3
                    tutorial3[1].SetActive(false);
                    tutorial3[2].SetActive(true);
                    isActiveTask2 = false;
                    isActiveTask3 = true;
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial3);

                }
            }

            if (isActiveTask3)
            {
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
                {
                    //if player placed cards incorrectly we reset recipe so he could try again
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial3);
                }
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
                {
                    //completed task3_3
                    tutorial3[2].SetActive(false);
                    tutorial3[3].SetActive(true);
                    isActiveTutorial3 = false;
                    isActiveTask3 = false;
                }
            }
        }

        if (isActiveTutorial4)
        {
            if (isActiveTask1)
            {
                if (mixingScript.GetLastResultElement() == water)
                {
                    //completed task1_4
                    tutorial4[0].SetActive(false);
                    tutorial4[1].SetActive(true);
                    isActiveTask1 = false;
                    isActiveTask2 = true;
                    trackedCompoundElementCard = false;
                }
            }

            if (isActiveTask2)
            {
                if (trackedCompoundElementCard)
                {
                    //completed task2_4
                    tutorial4[1].SetActive(false);
                    tutorial4[2].SetActive(true);
                    isActiveTask2 = false;
                    isActiveTask3 = true;
                    showRecipesButton.SetActive(true);
                }
            }

            if (isActiveTask3)
            {
                if (compoundElementsRecipePanel.activeInHierarchy)
                {
                    //hiding tutorial's panel when panel with compound element's recipes so it won't get in the way 
                    tutorial4[2].SetActive(false);
                }
                else
                {
                    //when panel  with compound element's recipes is closed showing tutorial's panel 
                    tutorial4[2].SetActive(true);
                    //freezing the timer. Because when compoundElementPanel is opened in play mode it fezzes timer and when it is closed timer continues
                    //we don't want timer counting down in tutorial mode 
                    timer.GetComponent<TimerScript>().StopTimer();
                }
                if (mixingScript.GetLastResultElement() == jimsonweed)
                {
                    //completed task3_4
                    tutorial4[2].SetActive(false);
                    tutorial4[3].SetActive(true);
                    isActiveTask3 = false;
                    isActiveTask4 = true;
                    resetCompoundElementButton.SetActive(true);
                    resetCompoundElement1Button.SetActive(true);

                }
            }

            if (isActiveTask4)
            {
                if (compoundElementCard.GetElement() == jimsonweed || compoundElementCard1.GetElement() == jimsonweed)
                {
                    //completed task4_4
                    tutorial4[3].SetActive(false);
                    tutorial4[4].SetActive(true);
                    isActiveTask4 = false;
                    isActiveTask5 = true;
                    //setting recipe for next task
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial4);
                }
            }

            if (isActiveTask5)
            {
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
                {
                    //if player placed cards incorrectly we reset recipe so he could try again
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial4);
                }
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
                {
                    //completed task5_4
                    tutorial4[4].SetActive(false);
                    tutorial4[5].SetActive(true);
                    isActiveTask5 = false;
                    isActiveTutorial4 = false;
                }
            }

        }

    }


    public void StartTutorial1()
    {
        startPanel.SetActive(false);
        //disabling button on transmutation circle card to prevent player from going off tutorial's recipes
        generateRecipeButton.SetActive(false);

        //disabling main game's UI
        coinCount.SetActive(false);
        timer.SetActive(false);
        resetCompoundElementButton.SetActive(false);
        resetCompoundElement1Button.SetActive(false);
        currentCompoundElement.SetActive(false);
        showRecipesButton.SetActive(false);

        //setting correct recipe on transmutation circle
        transmutationCircleCard.SetRecipe(newRecipeForTutorial2);

        //starting Tutorial 1 and its 1st task
        isActiveTutorial1 = true;
        isActiveTask1 = true;
        //activating corresponding panels on screen
        tutorial1[0].SetActive(true);
    }

    public void StartTutorial2()
    {
        //disabling 1st tutorial's last panel
        tutorial1[1].SetActive(false);

        //starting Tutorial 2 and activating correct panel for it
        isActiveTutorial2 = true;
        tutorial2[0].SetActive(true);
    }

    public void StartTutorial3()
    {
        //disabling 2nd tutorial's last panel
        tutorial2[1].SetActive(false);

        //starting Tutorial 3 and activating correct panel for it
        isActiveTutorial3 = true;
        tutorial3[0].SetActive(true);
    }

    public void StartTutorial4()
    {
        //activating current compound element panel in top right corner of screen
        currentCompoundElement.SetActive(true);

        //disabling 3d tutorial's last panel
        tutorial3[3].SetActive(false);

        //starting Tutorial 4 and starting its first task, activating correct panel for it
        isActiveTutorial4 = true;
        tutorial4[0].SetActive(true);
        isActiveTask1 = true;
    }

    //called when button on last panel of tutorial 4 is presed
    public void FinishTutorials()
    {
        //returning everything to state it was before tutorials
        tutorial4[5].SetActive(false);
        startPanel.SetActive(true);
        generateRecipeButton.SetActive(true);
        coinCount.GetComponentInChildren<TMP_Text>().text = "0";
    }

    public void TrackedTransmutationCircleCard()
    {
        trackedTransmutationCircle = true;
    }

    public void TrackedCompoundElementCard()
    {
        trackedCompoundElementCard = true;
    }

}
