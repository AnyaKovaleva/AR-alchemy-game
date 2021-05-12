using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//this script is responsible for making compound elements 
public class ElementMixingScript : MonoBehaviour
{
    //recipes for compound elements script can create
    public RecipeDatabaseObject recipes;
    //Text field in the top right corner of the screen where name of result element is displayed
    public TMP_Text resultCompoundElementName;
    //last compound element crafted
    private ElementObject resultElement;


    private void Start()
    {
        recipes.CreateIngredientDictionaries();
        resultElement = null;
    }

    public ElementObject GetLastResultElement()
    {
        return resultElement;
    }

    public void ResetResultElement()
    {
        resultElement = null;
        resultCompoundElementName.text = "";
    }

    public void MixElements(Card card1, Card card2)
    {
        //indicates if passed cards match any recipe
        bool found = false;

        if (recipes == null)
        {
            //if no recipes passed - no elements can be created
            return;
        }

        if (card1 != null && card2 != null && card1.GetElement() != null && card2.GetElement() != null) //if both cards exist and have valid elements attached to them we proceed
        {
            for (int i = 0; i < recipes.recipesDatabase.Count; i++) // going through all recipes to find if any compound element can be crafted from 2 passed cards
            {

                if (recipes.recipesDatabase[i] == null)
                {
                    Debug.Log("recipe database = null");
                    continue;
                }

                if (recipes.recipesDatabase[i].ingredientTemperatureDictionary.TryGetValue(card1.GetElement(), out Temperature temp1) && recipes.recipesDatabase[i].ingredientTemperatureDictionary.TryGetValue(card2.GetElement(), out Temperature temp2)) // if recipe contains both elements we get the required temperatures of these elements as temp1 and temp2
                {
                    if (temp1 == card1.GetTemperature() && temp2 == card2.GetTemperature())
                    {
                        // if temperature of passed elements matches temperature recipe requires
                        found = true; //found matching recipe
                                      //set resultElement to result element of the matching recipe
                        resultElement = recipes.recipesDatabase[i].GetResultElement();
                    }
                }
            }
        }

        if (found)
        {
            if (resultElement != null)
            {
                //display result element  on screen
                resultCompoundElementName.text = resultElement.displayName;
            }
            else
            {
                resultCompoundElementName.text = " ";
            }
        }
    }
}

