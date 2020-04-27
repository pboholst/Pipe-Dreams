using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveSlotBehavior : MonoBehaviour
{
    //118
    public Image Slot;
    public List<Image> slots = new List<Image>();
    Animation slotAnimation;


    public void Awake()
    {

        Transform panel = this.transform.GetChild(0);


        //// Instantiate 117 slots 9x13
        for (int i = 0; i <= 116; i++)
        {
            Image slot_ = Instantiate(Slot);
            slot_.name = "slotActive_" + i;
            slots.Add(slot_);
            slots[i].transform.SetParent(panel.transform, false);
            slots[i].enabled = false;
        }
    }
}
