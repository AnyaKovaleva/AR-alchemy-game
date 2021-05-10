using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class CompoundElementCard : Card
{
    public Button resetCurrentElementButton;

    public TMP_Text elementNameDisplay;
    //compound element that is currently assigned to the card
    private CompoundElementObject currentCompoundElement;
    //list of compound element prefabs ever spawned on this card
    private List<GameObject> compoundElementPrefabsList;

    void Start()
    {
        mixingScript = GameObject.Find("PoitionMixingSysyem").GetComponent<ElementMixingScript>();
        if (mixingScript == null)
        {
            Debug.Log("Mixing script not found");
        }
        if (!GameObject.Find("TransmutationCircle Card").TryGetComponent<PotionMakingSkript>(out potionMakingScirpt))
        {
            Debug.Log("Potion making  script not found");
        }
        UpdateTemperatureVisual();
        compoundElementPrefabsList = new List<GameObject>();
        currentCompoundElement = null;
        canCollide = true;

    }

    public override ElementObject GetElement()
    {
        return currentCompoundElement;
    }

    //called when card image is found or when we reset compound element. 
    public override void ShowCardElementPrefab()
    {
        //if a compound element has been assigned to card and it needs to be shown we find it in compoundElementsPrefabList and activate it
        if (currentCompoundElement != null)
        {
            int index = FindCompoundElementInList(currentCompoundElement.prefab);
            if (index != -1)
            {
                compoundElementPrefabsList[index].SetActive(true);
                elementNameDisplay.text = Regex.Replace(currentCompoundElement.name, "([a-z])([A-Z])", "$1 $2").Trim(); //ObjectNames.NicifyVariableName(currentCompoundElement.name);
                return;
            }
        }

        //if there is no current compound element and there is no compound element crafted by mixing script we show nothing on compound card
        if (mixingScript.GetLastResultElement() == null)
        {
            return;
        }
        //if a compound element has been crafted by mixing script and this card currently has no active compoundElement we fetch compound element from mixing script and set it as current
        if (mixingScript.GetLastResultElement().GetElementType() == ElementType.COMPOUND_ELEMENT && currentCompoundElement == null)
        {
            //setting last crafted compound element of mixing script as current
            currentCompoundElement = (CompoundElementObject)mixingScript.GetLastResultElement();
            mixingScript.ResetResultElement();

            canBeAffectedByTemperature = currentCompoundElement.canBeAffectedByTemperature;

            temperature = Temperature.NORMAL;

            UpdateTemperatureVisual();

            int index = FindCompoundElementInList(currentCompoundElement.prefab);
            if (index != -1)
            {//if compound element we just got from mixingScript had already been spawned on this card before we just find it in prefabs list and activate it and update element name display
                compoundElementPrefabsList[index].SetActive(true);
                elementNameDisplay.text = Regex.Replace(currentCompoundElement.name, "([a-z])([A-Z])", "$1 $2").Trim();
                return;
            }
            else
            {//if the element had never been spawned on this card we instantiate it and add it to list of compoundElementPrefabs
                GameObject tmpCompElemPrefab = null;
                tmpCompElemPrefab = Instantiate(currentCompoundElement.prefab, this.transform, false);
                tmpCompElemPrefab.name = currentCompoundElement.prefab.name;
                compoundElementPrefabsList.Add(tmpCompElemPrefab);
                elementNameDisplay.text = Regex.Replace(currentCompoundElement.name, "([a-z])([A-Z])", "$1 $2").Trim();
            }
        }

    }

    //called when card image is not tracked. Disables current prefab showed on card
    public override void HideCardElementPrefab()
    {
        canCollide = true;
        if (currentCompoundElement != null)
        {
            //if there is an element currently displayed on card we deactivate it
            int index = FindCompoundElementInList(currentCompoundElement.prefab);
            compoundElementPrefabsList[index].SetActive(false);
            elementNameDisplay.text = "";
        }
    }

    //called when "Reset compound element" button is pressed. Hides element currently displayed on card
    //and if a new compound element has been crafted by MixingScript (crafted element displays in top right corner of screen) fetches it and displays it on card
    public void ResetCurrentCompoumdElement()
    {
        HideCardElementPrefab();
        currentCompoundElement = null;
        ShowCardElementPrefab();
    }

    //looks for an element in list of compound elements that have ever been spawned on this card. If this element exists in list - function returns it's index, otherwise returns -1
    private int FindCompoundElementInList(GameObject elementToFind)
    {
        for (int i = 0; i < compoundElementPrefabsList.Count; i++)
        {
            if (compoundElementPrefabsList[i].name == elementToFind.name)
            {
                return i;
            }
        }

        return -1;
    }

}
