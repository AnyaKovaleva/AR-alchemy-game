using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//temperature effects crystals can have on other cards
public enum TemperatureEffect
{
    NONE,
    HEAT_UP,
    COOL_DOWN
}

//crystals are elements that can affect other cards by changing their temperature
[CreateAssetMenu(fileName = "New Crystal Element", menuName = "Alchemy/Elements/Crystal")]
public class CrystalElementObject : ElementObject
{
    [SerializeField]
    private TemperatureEffect crystalTemperatureEffect;
    private void Awake()
    {
        type = ElementType.CRYSTAL;
        canBeAffectedByTemperature = false;
    }

    public TemperatureEffect GetCrystalTemperatureEffect()
    {
        return crystalTemperatureEffect;
    }

}
