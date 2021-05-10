using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Alchemy/Elements/ElementList")]
public class ListOfElements : ScriptableObject
{
    public List<ElementObject> allElements;

    //returns true if all elements in list are unique. If list has any repeating elements function will return false
    public bool ContainsOnlyUniqeElements()
    {
        HashSet<ElementObject> uniqeElements = new HashSet<ElementObject>();
        foreach (ElementObject element in allElements)
        {
            if (element != null)
            {
                uniqeElements.Add(element);
            }

        }
        if (uniqeElements.Count == allElements.Count)
        {
            return true;
        }

        return false;
    }

    //if list contains any null elements function will return true. If all elements in list are assigned function will return false
    public bool ContainsNullElements()
    {
        foreach (ElementObject element in allElements)
        {
            if (element == null)
            {
                return true;
            }

        }

        return false;
    }

}
