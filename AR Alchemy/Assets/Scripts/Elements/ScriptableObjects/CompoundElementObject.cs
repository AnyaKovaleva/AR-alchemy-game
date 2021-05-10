using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//compound elements are elements that are crafted from 2 other elements. They cost more than base elements.

[CreateAssetMenu(fileName = "New Compound Element", menuName = "Alchemy/Elements/CompoundElement")]
public class CompoundElementObject : ElementObject
{
    private void Awake()
    {
        type = ElementType.COMPOUND_ELEMENT;
        price = 30;
        canBeAffectedByTemperature = true;
    }
}
