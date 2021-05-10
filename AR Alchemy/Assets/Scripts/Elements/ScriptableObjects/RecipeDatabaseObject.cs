using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//list of recipes

[CreateAssetMenu(fileName = "New Recipe Database", menuName = "Alchemy/Recipe/Database")]
public class RecipeDatabaseObject : ScriptableObject
{
    public List<RecipeObject> recipesDatabase;

    public void CreateIngredientDictionaries()
    {
        for(int i = 0; i < recipesDatabase.Count; i++)
        {
            if(recipesDatabase[i] == null)
            {
                recipesDatabase.Remove(recipesDatabase[i]);
            }
            recipesDatabase[i].CreateIngredientTemperatureDictionary();
        }
    }
}
