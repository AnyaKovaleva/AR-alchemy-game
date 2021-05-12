using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCard : Card
{
    public CrystalElementObject crystal;

    // turns true when crystal collided with another card and turns false when crystal is brought close enough to other card to apply temperature effect second time
    private bool canCheckIfObjectIsTooClose;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTemperatureVisual();
        mixingScript = GameObject.Find("PoitionMixingSysyem").GetComponent<ElementMixingScript>();
        canBeAffectedByTemperature = crystal.canBeAffectedByTemperature;
        element = crystal;
        if (!GameObject.Find("TransmutationCircle Card").TryGetComponent<PotionMakingSkript>(out potionMakingScirpt))
        {
            Debug.Log("Potion making  script not found");
        }
        canCollide = true;
    }

    public override ElementObject GetElement()
    {
        return crystal;
    }

    //if crystal has a temperature effect and other card can be affected by temperature crystal changes temperature of the other card
    private void ApplyTemperatureEffect(Card card)
    {
        if (card.GetCanBeAffectedByTemperature())
        {
            Debug.Log("TEEEMP CHANGEEEEE");
            if (crystal.GetCrystalTemperatureEffect() == TemperatureEffect.HEAT_UP)
            {
                if (card.GetTemperature() == Temperature.COLD)
                {
                    card.SetTemperature(Temperature.NORMAL);
                }
                else
                {
                    card.SetTemperature(Temperature.HOT);
                    //in 40 seconds card will cool back down to normal temperature
                    StartCoroutine(card.ReturnToNormalTemperature());
                }
                card.UpdateTemperatureVisual();
                
                return;
            }

            if (crystal.GetCrystalTemperatureEffect() == TemperatureEffect.COOL_DOWN)
            {
                if (card.GetTemperature() == Temperature.HOT)
                {
                    card.SetTemperature(Temperature.NORMAL);
                }
                else
                {
                    card.SetTemperature(Temperature.COLD);
                    //in 40 seconds card will heat back down to normal temperature
                    StartCoroutine(card.ReturnToNormalTemperature());
                }
                card.UpdateTemperatureVisual();

            }
            Debug.Log("Card temp = " + card.GetTemperature());
        }
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if (!canCollide)
        {
            //if card is placed on ingredient slot - no collision with other cards
            return;
        }
        Card otherCard = collision.gameObject.GetComponent<Card>(); //check if crystal collided with another card
        if (otherCard != null)
        {
            canCheckIfObjectIsTooClose = true;
            //applies temperature effect 1 time
            ApplyTemperatureEffect(otherCard);
            canCollide = false;
            if (otherCard.GetElement() != null && otherCard.GetElement().GetElementType() == ElementType.CRYSTAL)
            {
                //check if combination of this crystal and card it collided with can make a compound element
                mixingScript.MixElements(this, otherCard);
            }
        }
    }



    private void OnCollisionStay(Collision collision)
    {
        //if (!canCollide)
        //{
        //    return;
        //}
        if (canCheckIfObjectIsTooClose)
        {
            Card otherCard = collision.gameObject.GetComponent<Card>(); //check if crystal is in collision with another card
            if (otherCard != null)
            {
                //if crystal is brought too close to another card it applies it's temperature effect 2nd time
                //For example when crystal with temperature effect HEAT_UP enters collision with COLD element element's temperature will be only changed to NORMAL.
                //But when distance between crystal and element becomes less than 0.19 crystal will apply it's temperature effect for the second time and change element's temperature to HOT
                if (Vector3.Distance(this.transform.position, collision.gameObject.transform.position) < 0.21)
                {
                    canCheckIfObjectIsTooClose = false;
                    ApplyTemperatureEffect(otherCard);
                    //after changing temperature we check again if a compound element could be made
                    mixingScript.MixElements(this, otherCard);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canCollide = true;
    }



}
