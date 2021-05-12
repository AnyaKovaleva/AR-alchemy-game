using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Vuforia;

public class TutorialScript : MonoBehaviour
{
    public GameObject startPanel;
    public List<GameObject> tutorial1;
    public List<GameObject> tutorial2;
    public List<GameObject> tutorial3;
    public List<GameObject> tutorial4;

    public RecipeObject newRecipeForTutorial2;
    public RecipeObject newRecipeForTutorial3;
    public RecipeObject newRecipeForTutorial4;

    public BaseElementCard blueMountainFlowerCard;
    public BaseElementCard lutiMushroomCard;

    public CompoundElementCard compoundElementCard;
    public CompoundElementCard compoundElementCard1;

    public GameObject compoundElementsRecipePanel;

    public CompoundElementObject water;
    public CompoundElementObject jimsonweed;

    public PotionMakingSkript transmutationCircleCard;
    public GameObject generateRecipeButton;

    public ElementMixingScript mixingScript;

    public GameObject coinCount;
    public GameObject timer;
    public GameObject resetCompoundElementButton;
    public GameObject resetCompoundElement1Button;
    public GameObject currentCompoundElement;
    public GameObject showRecipesButton;

    private bool isActiveTutorial1;
    private bool isActiveTutorial2;
    private bool isActiveTutorial3;
    private bool isActiveTutorial4;

    private bool trackedTransmutationCircle;
    private bool trackedCompoundElementCard;

    private bool isActiveTask1;
    private bool isActiveTask2;
    private bool isActiveTask3;
    private bool isActiveTask4;
    private bool isActiveTask5;

    //private RectTransform[] textComponents;
    //private GameObject task1;
    //private GameObject task1Completed;

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

        foreach(GameObject gameObject in tutorial1)
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
        if (isActiveTutorial1)
        {
            if (trackedTransmutationCircle)
            {
                tutorial1[0].SetActive(false);
                tutorial1[1].SetActive(true);
                isActiveTutorial1 = false;
            }
        }

        if (isActiveTutorial2)
        {
            if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
            {
                transmutationCircleCard.SetRecipe(newRecipeForTutorial2);
            }
            if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
            {
                tutorial2[0].SetActive(false);
                tutorial2[1].SetActive(true);
                isActiveTutorial2 = false;
                coinCount.SetActive(true);
                timer.SetActive(true);
            }
        }

        if(isActiveTutorial3)
        {
            if(isActiveTask1)
            {
                if(blueMountainFlowerCard.GetTemperature() == Temperature.COLD)
                {
                    tutorial3[0].SetActive(false);
                    tutorial3[1].SetActive(true);
                    isActiveTask1 = false;
                    isActiveTask2 = true;
                }
            }

            if(isActiveTask2)
            {
                if(lutiMushroomCard.GetTemperature() == Temperature.HOT)
                {
                    tutorial3[1].SetActive(false);
                    tutorial3[2].SetActive(true);
                    isActiveTask2 = false;
                    isActiveTask3 = true;
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial3);

                }
            }

            if(isActiveTask3)
            {
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
                {
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial3);
                }
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
                {
                    tutorial3[2].SetActive(false);
                    tutorial3[3].SetActive(true);
                    isActiveTutorial3 = false;
                    isActiveTask3 = false;
                }
            }
        }

        if(isActiveTutorial4)
        {
            if(isActiveTask1)
            {
                if(mixingScript.GetLastResultElement() == water)
                {
                    tutorial4[0].SetActive(false);
                    tutorial4[1].SetActive(true);
                    isActiveTask1 = false;
                    isActiveTask2 = true;
                }
            }

            if(isActiveTask2)
            {
                if(trackedCompoundElementCard)
                {
                    tutorial4[1].SetActive(false);
                    tutorial4[2].SetActive(true);
                    isActiveTask2 = false;
                    isActiveTask3 = true;
                    showRecipesButton.SetActive(true);
                }
            }

            if(isActiveTask3)
            {
                if(compoundElementsRecipePanel.activeInHierarchy)
                {
                    tutorial4[2].SetActive(false);
                }
                else
                {
                    tutorial4[2].SetActive(true);
                }
                if (mixingScript.GetLastResultElement() == jimsonweed)
                {
                    tutorial4[2].SetActive(false);
                    tutorial4[3].SetActive(true);
                    isActiveTask3 = false;
                    isActiveTask4 = true;
                    resetCompoundElementButton.SetActive(true);
                    resetCompoundElement1Button.SetActive(true);
                    
                }
            }

            if(isActiveTask4)
            {
                if(compoundElementCard.GetElement() == jimsonweed || compoundElementCard1.GetElement() == jimsonweed)
                {
                    tutorial4[3].SetActive(false);
                    tutorial4[4].SetActive(true);
                    isActiveTask4 = false;
                    isActiveTask5 = true;
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial4);
                }
            }

            if(isActiveTask5)
            {
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.SPOILED)
                {
                    transmutationCircleCard.SetRecipe(newRecipeForTutorial4);
                }
                if (transmutationCircleCard.GetPotionState() == PotionMakingSkript.PotionState.READY)
                {
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
        generateRecipeButton.SetActive(false);

        coinCount.SetActive(false);
        timer.SetActive(false);
        resetCompoundElementButton.SetActive(false);
        resetCompoundElement1Button.SetActive(false);
        currentCompoundElement.SetActive(false);
        showRecipesButton.SetActive(false);

        transmutationCircleCard.SetRecipe(newRecipeForTutorial2);

        isActiveTutorial1 = true;
        isActiveTask1 = true;
        tutorial1[0].SetActive(true);
        tutorial1[1].SetActive(false);
        
    }

    public void StartTutorial2()
    {
        tutorial1[1].SetActive(false);

        transmutationCircleCard.SetRecipe(newRecipeForTutorial2);

        isActiveTutorial2 = true;
        tutorial2[0].SetActive(true);
        tutorial2[1].SetActive(false);

    }

    public void StartTutorial3()
    {
        tutorial2[1].SetActive(false);
        isActiveTutorial3 = true;
        tutorial3[0].SetActive(true);
    }

    public void StartTutorial4()
    {
        currentCompoundElement.SetActive(true);
        tutorial3[3].SetActive(false);
        isActiveTutorial4 = true;
        tutorial4[0].SetActive(true);
        isActiveTask1 = true;
    }

    public void FinishTutorials()
    {
        tutorial4[5].SetActive(false);
        startPanel.SetActive(true);
        generateRecipeButton.SetActive(true);
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
