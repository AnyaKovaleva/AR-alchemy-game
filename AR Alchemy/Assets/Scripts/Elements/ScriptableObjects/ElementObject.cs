using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    BASE_ELEMENT,
    COMPOUND_ELEMENT,
    COMPLEX_ELEMENT,
    CRYSTAL
}


public abstract class ElementObject : ScriptableObject
{
    public GameObject prefab;
    [Range (1, 100)]
    public int price;
    public bool canBeAffectedByTemperature;
    protected ElementType type;

    public ElementType GetElementType()
    {
        return type;
    }       
}
