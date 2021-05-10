using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

//responsible for making potions. attached to transmutation circle
public class PotionMakingSkript : MonoBehaviour
{
    //list of base elements (and crystals) that can appear as potion's ingredients
    public ListOfElements listOfElements;
    //list of compound elements that can appear as ingredients
    public ListOfElements listOfCompoundElements;
    //ingredient slots are where cards placed to make a potion
    public List<GameObject> ingredientSlots;
    //visualization of potion's state. [0] - empty bottle (no state. potion is not ready yet). [1] - green bottle (all ingredients mixed correctly and potion is ready). [2] -red bottle (placed incorrect ingredient and potion is spoiled)
    public List<GameObject> potionStatePrefabs;
    public TMP_Text potionStateText;
    //max number of ingredients in recipe. 
    [Range(2, 6)]
    public int maxNumberOfIngredients;
    //text in top left corner of screen where number of earned coins is displayed
    public TMP_Text coinCountText;

    //recipe to make potion
    private RecipeObject potionRecipe;
    private int potionPrice;
    //ingredients that are placed on ingredient slots
    private Card[] currentlyMixedIngredients;
    //if true - potion has been spoiled
    private bool isSpoiled;
    //number of elements in current recipe. number between 2 and maxNumberOfIngredients
    private int numOfIngredientsInPotion;
    //number of compound elements in recipe. Number from 0 to 2
    private int numOfCompoundIngredientsInPotion;
    //if true - passed list of elements contains only valid base and crystal elements
    private bool isEligibleListOfElements;
    //if true - passed list of compound elements contains only valid compound elements
    private bool isEligibleListOfCompoundElements;

    private enum PotionState
    {
        NO_STATE,
        SPOILED,
        READY
    }
    // Start is called before the first frame update
    void Start()
    {
        isEligibleListOfElements = true;
        isEligibleListOfCompoundElements = true;
        CheckElementListsForEligibility();
        if (isEligibleListOfElements)
        {
            if (listOfElements.allElements.Count < maxNumberOfIngredients)
            {
                //if passed maxNumperOfIngredients in recipe is grater than number of passed elements in list than maxNumberOfIngredients is set to number of elements in listOfElements
                maxNumberOfIngredients = listOfElements.allElements.Count;
            }
            potionRecipe = new RecipeObject();
            currentlyMixedIngredients = new Card[6];
            isSpoiled = false;
            numOfIngredientsInPotion = 0;
            GeneratePotionRecipe();
        }


    }

    //check if passed lists of elements meet the requirements and contain only valid  unique elements
    private void CheckElementListsForEligibility()
    {
        if (listOfElements == null)
        {
            Debug.LogError("List of elements is not assigned");
            isEligibleListOfElements = false;
            return;
        }
        if (listOfElements.allElements.Count < 2)
        {
            Debug.LogError("list of elements should have at least 2 elements to choose from");
            isEligibleListOfElements = false;
            return;
        }

        if (listOfElements.ContainsNullElements())
        {
            Debug.LogError("list of elements can not contain null elements");
            isEligibleListOfElements = false;
            return;
        }

        if (!listOfElements.ContainsOnlyUniqeElements())
        {
            Debug.LogError("list of elements can not contain repeating elements. All elements in list must be unique");
            isEligibleListOfElements = false;
            return;
        }

        for (int i = 0; i < listOfElements.allElements.Count; i++)
        {
            if (listOfElements.allElements[i].GetElementType() != ElementType.BASE_ELEMENT && listOfElements.allElements[i].GetElementType() != ElementType.CRYSTAL)
            {
                Debug.LogError("List of element contains invalid element. It can contain only base elements and crystals. " + listOfElements.allElements[i].name
                    + " (" + i + " index in list) is " + listOfElements.allElements[i].GetElementType());
                isEligibleListOfElements = false;
                return;
            }
        }

        if (listOfCompoundElements != null)
        {
            if (listOfCompoundElements.ContainsNullElements())
            {
                Debug.LogError("list of compound elements can not contain null elements");
                isEligibleListOfCompoundElements = false;
                return;
            }

            if (!listOfCompoundElements.ContainsOnlyUniqeElements())
            {
                Debug.LogError("list of compound elements can not contain repeating elements. All elements in list must be unique");
                isEligibleListOfCompoundElements = false;
                return;
            }

            for (int i = 0; i < listOfCompoundElements.allElements.Count; i++)
            {

                if (listOfCompoundElements.allElements[i].GetElementType() != ElementType.COMPOUND_ELEMENT)
                {
                    Debug.LogError("List of compound elements should contain only compound elements. " + listOfCompoundElements.allElements[i].name
                    + " (" + i + " index in list) is " + listOfCompoundElements.allElements[i].GetElementType());
                    isEligibleListOfCompoundElements = false;
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("List of compound elements is not assigned. There will be no compound elements in potion recipe");
            isEligibleListOfCompoundElements = false;
        }
    }


    //generates potion recipe
    public void GeneratePotionRecipe()
    {
        if (!isEligibleListOfElements)
        {
            return;
        }
        //reset potion state, currently mixed ingredients, potionRecipe
        ChangePotionStatePrefab(PotionState.NO_STATE);
        isSpoiled = false;
        currentlyMixedIngredients = new Card[6];
        potionRecipe.ingredients.Clear();
        potionRecipe.temperatureOfIngredients.Clear();
        //generating random number of ingredients this recipe will have
        numOfIngredientsInPotion = Random.Range(2, maxNumberOfIngredients + 1);
        //activating ingredient slots
        ActivateIngredientSlots();
        //generating random number of compound ingredients in potion
        numOfCompoundIngredientsInPotion = Random.Range(0, 3);

        //filling potion recipe with random non-repeating elements and giving them random temperature
        for (int i = 0; i < numOfIngredientsInPotion; i++)
        {
            ElementObject newIngredient = RandomElement();
            while (potionRecipe.ingredients.Contains(newIngredient)) //potion can contain only unique ingredients
            {
                newIngredient = RandomElement();
            }

            potionRecipe.ingredients.Add(newIngredient);
            if (newIngredient.canBeAffectedByTemperature == true)
            {
                Temperature temp = RandomTemperature();
                Debug.Log(newIngredient.name + " can be affected by temp so it's temperature is " + temp);
                potionRecipe.temperatureOfIngredients.Add(temp);
            }
            else
            {
                //if added ingredient can not be affected by temperature then it's temperature will be set to normal
                potionRecipe.temperatureOfIngredients.Add(Temperature.NORMAL);
            }
        }
        AddCompoundElementsToRecipe();
        CalculatePotionPrice();
        UpdateIngredientSlotsText();

    }

    //updates text boxes under ingredient slots to display ingredient's names according to potionRecipe
    private void UpdateIngredientSlotsText()
    {
        if (ingredientSlots == null)
        {
            Debug.LogError("Ingredient slots are not assigned");
            return;
        }
        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            TMP_Text text = ingredientSlots[i].GetComponentInChildren<TMP_Text>();
            if (text == null)
            {
                return;
            }

            if (i >= numOfIngredientsInPotion)
            {
                text.text = " ";
                text.color = Color.white;
                continue;
            }
            //displaying ingredient's name
            text.text = Regex.Replace(potionRecipe.ingredients[i].name, "([a-z])([A-Z])", "$1 $2").Trim();
            Temperature temp = potionRecipe.temperatureOfIngredients[i];

            //changing color of text according to temperature
            if (temp == Temperature.COLD)
            {
                text.color = Color.blue;
            }
            else if (temp == Temperature.HOT)
            {
                text.color = Color.red;
            }
            else
            {
                text.color = Color.white;
            }


        }
    }

    private void AddCompoundElementsToRecipe()
    {
        if (!isEligibleListOfCompoundElements)
        {
            return;
        }
        int compoundElementIndex = 0;
        int finalPlaceIndex = 0;
        for (int i = 0; i < numOfCompoundIngredientsInPotion; i++)
        {
            //index of compound element from listOfCompoundElements
            compoundElementIndex = Random.Range(0, listOfCompoundElements.allElements.Count);
            //index in potion recipe where compound element will be placed 
            finalPlaceIndex = Random.Range(0, potionRecipe.ingredients.Count);
            //replace ingredient with index finalPlaceIndex with compound element
            potionRecipe.ingredients[finalPlaceIndex] = listOfCompoundElements.allElements[compoundElementIndex];
            if (!potionRecipe.ingredients[finalPlaceIndex].canBeAffectedByTemperature)
            {
                //if compound element can not be affected by temperature - temperature of corresponding element in recipe will be changed to normal
                potionRecipe.temperatureOfIngredients[finalPlaceIndex] = Temperature.NORMAL;
            }

        }
    }

    //returns random element from listOfElements
    private ElementObject RandomElement()
    {
        int index = Random.Range(0, listOfElements.allElements.Count);
        return listOfElements.allElements[index];
    }

    //returns random temperature
    private Temperature RandomTemperature()
    {
        int index = Random.Range(0, 3);
        Debug.Log("TEMP index is " + index);
        Debug.Log("Temp should be" + (Temperature.NORMAL + index));
        switch (index)
        {
            case 0: return Temperature.NORMAL;
            case 1: return Temperature.HOT;
            case 2: return Temperature.COLD;
        }

        return Temperature.NORMAL;
    }

    private int CalculatePotionPrice()
    {
        potionPrice = 0;
        for (int i = 0; i < numOfIngredientsInPotion; i++)
        {
            if (potionRecipe.ingredients[i] != null)
            {
                potionPrice += potionRecipe.ingredients[i].price;
            }
            else
            {
                Debug.Log("Null ingredient");
                return 0;
            }
            if (potionRecipe.temperatureOfIngredients[i] != Temperature.NORMAL)
            {
                //extra 5 coins if ingredient's temperature is not NORMAL
                potionPrice += 5;
            }
            if (potionRecipe.ingredients[i].GetElementType() == ElementType.COMPOUND_ELEMENT)
            {
                //extra 5 coins if ingredient is compound element
                potionPrice += 5;
            }
        }
        potionPrice *= numOfIngredientsInPotion;
        return potionPrice;
    }

    //this function is called when card is placed on ingredient slot
    public void MixInNewIngredient(Card newIngredient, GameObject slot)
    {
        int slotIndex = ingredientSlots.IndexOf(slot);
        if (slotIndex < 0)
        {
            Debug.LogWarning("slot is not found");
            return;
        }


        currentlyMixedIngredients[slotIndex] = newIngredient;
        //check if after adding new ingredient currentlyMixedIngredients follow the recipe
        CheckIfFollowsRecipe(slotIndex);

        if (isSpoiled) //if potion is spoiled we hide all ingredient slots and set potion state to spoiled
        {
            ChangePotionStatePrefab(PotionState.SPOILED);
            DeactivateAllIngredientSlots();
            return;
        }

        if (slotIndex == numOfIngredientsInPotion - 1)  //if we mixed all required ingredients 
        {
            int coinCount = 0;
            if (!int.TryParse(coinCountText.text, out coinCount))
            {
                coinCount = 0;
            }
            //adding potion price to total earned coins
            coinCount += potionPrice;
            ChangePotionStatePrefab(PotionState.READY);
            //displaying coin count
            coinCountText.text = coinCount.ToString();
            DeactivateAllIngredientSlots();

        }

    }

    //checks if added ingredients match the recipe
    private void CheckIfFollowsRecipe(int lastAddedElementIndex)
    {
        for (int i = 0; i < currentlyMixedIngredients.Length; i++)
        {
            if (currentlyMixedIngredients[i] == null)
            {
                if (i < lastAddedElementIndex)
                {
                    //card placed in wrong order. Previous card slot is empty. 
                    isSpoiled = true;
                    return;
                }
                continue;
            }

            if (currentlyMixedIngredients[i].GetElement() == potionRecipe.ingredients[i])
            {
                if (currentlyMixedIngredients[i].GetTemperature() != potionRecipe.temperatureOfIngredients[i])
                {
                    //element has incorrect temperature
                    isSpoiled = true;
                    return;
                }
            }
            else
            {
                //element does not match recipe
                isSpoiled = true;
                return;
            }
        }
    }

    //activates required number of ingredient slots based on number of ingredients in potion's recipe
    private void ActivateIngredientSlots()
    {
        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            if (i >= numOfIngredientsInPotion)
            {
                ingredientSlots[i].SetActive(false);
            }
            else
            {
                ingredientSlots[i].SetActive(true);
            }

        }
    }

    private void DeactivateAllIngredientSlots()
    {
        foreach (GameObject slot in ingredientSlots)
        {
            slot.SetActive(false);
        }
    }

    //activates potionStatePrefab based on passed state and updates potionStateText
    private void ChangePotionStatePrefab(PotionState state)
    {
        if (state == PotionState.NO_STATE)
        {
            potionStatePrefabs[0]?.SetActive(true);
            potionStatePrefabs[1]?.SetActive(false);
            potionStatePrefabs[2]?.SetActive(false);
            potionStateText.text = "";
        }

        if (state == PotionState.READY)
        {
            potionStatePrefabs[0]?.SetActive(false);
            potionStatePrefabs[1]?.SetActive(true);
            potionStatePrefabs[2]?.SetActive(false);
            potionStateText.text = "Potion is ready!";
        }

        if (state == PotionState.SPOILED)
        {
            potionStatePrefabs[0]?.SetActive(false);
            potionStatePrefabs[1]?.SetActive(false);
            potionStatePrefabs[2]?.SetActive(true);
            potionStateText.text = "PotionIsSpoiled";
        }
    }
}
