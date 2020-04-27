using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PipeDreams
{
    public class PanelSlotBehaviour : MonoBehaviour
    {
        //118
        public Image Slot;
        public List<Image> slots = new List<Image>();
        private string instanceName = "_";

        public void Awake()
        {

            Transform panel = this.transform.GetChild(0);
            if(panel.name == "PanelSlotBG")
            {
                instanceName = "BG_";
            }
            //Debug.Log(panel.name);

            //// Instantiate 117 slots 9x13
            for (int i = 0; i <= 116; i++)
            {
                Image slot_ = Instantiate(Slot);
                slot_.name = "slot" + instanceName + i;
                slots.Add(slot_);
                slots[i].transform.SetParent(panel.transform, false);
            }
        }

        private void CreateObjects()
        {

        }
    }
}