using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG_SlotBehaviour : MonoBehaviour
{
    //118
    public Image Slot;
    public List<Image> slots = new List<Image>();


    public void Awake()
    {

        Transform panel = this.transform.GetChild(0);


        //// Instantiate 117 slots 9x13
        for (int i = 0; i <= 116; i++)
        {
            Image slot_ = Instantiate(Slot);
            slot_.name = "slotBG_" + i;
            slots.Add(slot_);
            slots[i].transform.SetParent(panel.transform, false);
        }
    }
}
