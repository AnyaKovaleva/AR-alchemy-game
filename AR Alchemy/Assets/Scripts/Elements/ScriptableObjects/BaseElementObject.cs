using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base elements are the simplest elements. They don't need to be crafted, they are depicted on cards.

[CreateAssetMenu(fileName = "New Base Element", menuName = "Alchemy/Elements/Base Element")]
public class BaseElementObject : ElementObject
{
    private void Awake()
    {
        type = ElementType.BASE_ELEMENT;
        price = 10;
        canBeAffectedByTemperature = true;
    }

}
