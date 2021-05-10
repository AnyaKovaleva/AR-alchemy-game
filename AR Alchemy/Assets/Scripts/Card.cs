using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Temperature
{
    NORMAL,
    HOT,
    COLD
}

// Class describing basic behavior of element cards 
public abstract class Card : MonoBehaviour
{
    //Temperature of the card. Can be NORMAL, HOT or COLD.
    [SerializeField]
    protected Temperature temperature;

    protected ElementObject element;
    //if set to true then this element's temperature can be changed by other cards that have ability to change temperature
    protected bool canBeAffectedByTemperature;
    //prefab of the element which spawns on top of the card
    protected GameObject cardElementPrefab;
    [SerializeField]
    //red panel placed over card 
    protected GameObject hotIndicator;
    [SerializeField]
    //blue panel placed over card
    protected GameObject coldIndicator;
    //determines if card can collide with other objects. It is set to false when card is placed on ingredient slot of transmutation circle
    protected bool canCollide;
    protected PotionMakingSkript potionMakingScirpt;
    protected ElementMixingScript mixingScript;

    // Start is called before the first frame update
    void Start()
    {
        temperature = Temperature.NORMAL;
        cardElementPrefab = null;
        UpdateTemperatureVisual();
        mixingScript = GameObject.Find("PoitionMixingSysyem").GetComponent<ElementMixingScript>();
        if (mixingScript == null)
        {
            Debug.Log("Mixing script not found");
        }
        if (!GameObject.Find("TransmutationCircle Card").TryGetComponent<PotionMakingSkript>(out potionMakingScirpt))
        {
            Debug.Log("Potion making  script not found");
        }
        canCollide = true;
    }


    public virtual ElementObject GetElement()
    {
        return element;
    }

    public bool GetCanBeAffectedByTemperature()
    {
        return canBeAffectedByTemperature;
    }

    public Temperature GetTemperature()
    {
        return temperature;
    }


    public virtual void SetTemperature(Temperature newTemperature)
    {
        temperature = newTemperature;
    }

    //called when card image is found
    public virtual void ShowCardElementPrefab()
    {
        GetElement();
        if (element == null)
        {
            Debug.Log("Element is NULL");
            return;
        }
        if (cardElementPrefab == null)
        {
            //when card image is found for the first time card's prefab is instantiated
            cardElementPrefab = Instantiate(element.prefab, this.transform, false);
            cardElementPrefab.name = element.prefab.name;
        }
        else
        {
            //when card is found not for the first time prefab is just being activated
            cardElementPrefab.SetActive(true);
        }
    }

    //called when image is lost
    public virtual void HideCardElementPrefab()
    {
        canCollide = true;
        if (cardElementPrefab != null)
        {
            //deactivates card's prefab
            cardElementPrefab.SetActive(false);
        }
    }

    //called when temperature of card has been changed. Activates temperature indicator panel according to temperature of the card
    public void UpdateTemperatureVisual()
    {
        if (hotIndicator == null)
        {
            Debug.LogWarning("Hot temperature indicator is null");
            return;
        }

        if (coldIndicator == null)
        {
            Debug.LogWarning("Cold temperature indicator is null");
            return;
        }

        if (temperature == Temperature.NORMAL)
        {
            hotIndicator.SetActive(false);
            coldIndicator.SetActive(false);
            return;
        }

        if (temperature == Temperature.HOT)
        {
            hotIndicator.SetActive(true);
            coldIndicator.SetActive(false);
            return;
        }

        if (temperature == Temperature.COLD)
        {
            hotIndicator.SetActive(false);
            coldIndicator.SetActive(true);
            return;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!canCollide)
        {
            return;
        }
        Card otherCard = collision.gameObject.GetComponent<Card>();
        if (otherCard != null)
        {
            //if collided with another card we check if  cards can be mixed to make a compound element 
            mixingScript.MixElements(this, otherCard);
        }
    }

    //last ingredient slot this card has triggered. Required to prevent this element from triggering same ingredient slot twice 
    protected GameObject prevSlot;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IngredientSlot")
        {
            if (prevSlot == other.gameObject)
            {
                return;
            }
            prevSlot = other.gameObject;
            //Adding this ingredient to potion
            potionMakingScirpt.MixInNewIngredient(this, other.gameObject);
            canCollide = false;

        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        //while element is placed on ingredient slot it can not collide
        canCollide = false;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "IngredientSlot")
        {
            canCollide = true;
        }

    }

}
