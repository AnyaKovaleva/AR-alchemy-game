using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseElementCard : Card
{
    public BaseElementObject baseElement;

    void Start()
    {
        mixingScript = GameObject.Find("PoitionMixingSysyem").GetComponent<ElementMixingScript>();
        if (mixingScript == null)
        {
            Debug.LogWarning("Mixing script not found");
        }
        UpdateTemperatureVisual();
        canBeAffectedByTemperature = baseElement.canBeAffectedByTemperature;
        element = baseElement;
        if (!GameObject.Find("TransmutationCircle Card").TryGetComponent<PotionMakingSkript>(out potionMakingScirpt))
        {
            Debug.LogWarning("Potion making  script not found");
        }
        canCollide = true;

    }

    public override ElementObject GetElement()
    {
        return baseElement;
    }

}
