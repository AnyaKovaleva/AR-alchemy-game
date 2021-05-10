using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Recipe for compound elements. Defines which 2 elements of certain temperature should be brought together to create specific compound element

[CreateAssetMenu(fileName = "New Compound Element Recipe", menuName = "Alchemy/Recipe/CompoundElementRecipe")]
public class CompoundElementRecipeObject : RecipeObject
{
    [SerializeField]
    public CompoundElementObject resultCompoundElement;
 
    private void Awake()
    {
        resultElement = resultCompoundElement;
        maxNumberOfIngredients = 2;
        Note = "Max number of ingredients in compound element recipe is 2. Elements of temperatureOfIngredients list represent temperature of ingredients of same index of ingredients list.  If there are more ingredients passed they will be ignored. If temperature is not passed it will be set to NORMAL";
    }

    public override ElementObject GetResultElement()
    {
        return resultCompoundElement;
    }

}
