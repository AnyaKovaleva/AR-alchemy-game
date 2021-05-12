using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Alchemy/Recipe/Recipe")]
public class RecipeObject : ScriptableObject
{
    //element that should be created if recipe is followed
    protected ElementObject resultElement;
    //list of ingredients required to make resultElement 
    public List<ElementObject> ingredients;
    //list of temperatures. Each temperature corresponds to an element from list of ingredients
    public List<Temperature> temperatureOfIngredients;
    //dictionary in which each ingredient is tied together with its temperature
    public Dictionary<ElementObject, Temperature> ingredientTemperatureDictionary = new Dictionary<ElementObject, Temperature>();

    //max number of ingredients that recipe can hold in ingredientTemperatureDictionary
    protected int maxNumberOfIngredients;
    [TextArea]
    public string Note = "Max number of ingredients in recipe is 6. Elements of temperatureOfIngredients list represent temperature of ingredients of same index of ingredients list.  If there are more ingredients passed they will be ignored. If temperature is not passed it will be set to NORMAL";

    public RecipeObject()
    {
        maxNumberOfIngredients = 6;
        ingredients = new List<ElementObject>();
        temperatureOfIngredients = new List<Temperature>();
    }

    public void SetMaxNumberOfIngredients(int newMaxNumber)
    {
        maxNumberOfIngredients = newMaxNumber;
    }

    public virtual ElementObject GetResultElement()
    {
        return resultElement;
    }

    public virtual int GetNumberOfCompoundElementsInRecipe()
    {
        int numOfCompoundElements = 0;

        foreach(ElementObject ingredient in ingredients)
        {
            if(ingredient != null && ingredient.GetElementType() == ElementType.COMPLEX_ELEMENT)
            {
                numOfCompoundElements++;
            }
        }

        return numOfCompoundElements;
    }

    //ties ingredients with their temperatures. Required to find temperature of ingredient without knowing its index
    public void CreateIngredientTemperatureDictionary()
    {
        for (int i = 0; i < maxNumberOfIngredients && i < ingredients.Count; i++)
        {
            if (ingredientTemperatureDictionary.ContainsKey(ingredients[i]))
            {
                continue;
            }
            if (temperatureOfIngredients.Count <= i)
            {
                //if temperature was not assigned we set it to NORMAL
                ingredientTemperatureDictionary.Add(ingredients[i], Temperature.NORMAL);
            }
            else
            {
                ingredientTemperatureDictionary.Add(ingredients[i], temperatureOfIngredients[i]);
            }
        }
    }
}
