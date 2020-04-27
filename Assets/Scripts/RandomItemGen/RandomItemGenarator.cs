using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using DevAll.DropChance;
using DevAll.Testing;

namespace PipeDreams{

    [RequireComponent(typeof(SpriteRenderer))]
    public class RandomItemGenarator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        //for testing
        public TestSettings DevTest;

        public Image Slot;
        public Image randomizedItem;
        public List<ItemPipeFunction> Items;

        private Vector2 offset;
        private Transform originalParent;
        Vector3 curPosition;

        public short elementNum;

        GameObject currentSlot;
        string[] curSlotName = new string[2];

        string[] curActiveSlotName = new string[2];

        PipeBehaviour slotTrigger; // This will grab the slot function.
        ActiveSlotBehavior ASB;

        Animator slotAnimator;
        //for click functionality

        bool isDraggingEnabled = false;

        ItemDropChance IDC;

        public bool pipeIsSelected = false;


        /**
         * Working on how the button is click. need to implement on drag item will go to mouse location
         * and will be able to drop it in the scene
         */
        private void Awake()
        {
            DevTest = GameObject.FindObjectOfType<TestSettings>();
            ASB = GameObject.FindObjectOfType<ActiveSlotBehavior>();

            //init random items
            InitItemsToRandomize();
        }

        private void InitItemsToRandomize()
        {
            IDC = GameObject.FindObjectOfType<ItemDropChance>();
            for (int i = 0; i < Items.Count; i++)
            {
                IDC.AddItems(Items[i].ItemID, Items[i].DropChance);
            }
            IDC.ShuffleItems();
        }
        public void Start()
        {
            originalParent = this.transform.GetChild(0);
            RandomNewItem();
            curPosition = randomizedItem.transform.position;
        }

        #region Drag Events
        public void OnBeginDrag(PointerEventData eventData)
        {
            try
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "NewlySpawned")
                {
                    isDraggingEnabled = true;
                }
                if (randomizedItem != null)
                {
                    originalParent = randomizedItem.transform.parent;

                    offset = eventData.position - new Vector2(randomizedItem.transform.position.x, randomizedItem.transform.position.y);
                    randomizedItem.transform.position = eventData.position - offset;
                    randomizedItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
            catch (Exception)
            {
            }
        }

        public void OnDrag(PointerEventData eventData)
        {

            try
            {
                if (isDraggingEnabled)
                {
                    if (randomizedItem != null)
                    {
                        randomizedItem.transform.position = Input.mousePosition;
                        //randomizedItem.transform.position = eventData.position - offset;
                    }
                    ActiveHighlight(eventData);
                }
            }
            catch (Exception){}
        }

        public void ActiveHighlight(PointerEventData eventData)
        {
            //This is to implement the highlighting of the slot for better view.
            currentSlot = eventData.pointerCurrentRaycast.gameObject;
            curSlotName = currentSlot.name.Split('_');
            //curActiveSlotName = ASB.slots[int.Parse(curSlotName[1])].name.Split('_');
            print(curSlotName[1]);
            //this is getting called about 8 times. need to go thorough just once because it might affect playing the animation.
            if (curSlotName[0] == "slot")
            {
                IfSelectedChanged(curSlotName[1]);
            }
        }

        public string curActiveNumberSelected = "";
        bool isChangedCalled = true;
        private void IfSelectedChanged(string activeNum) {
            if (curActiveNumberSelected != activeNum && isChangedCalled)
            {
                isChangedCalled = false;
                if (curActiveNumberSelected != "")
                {
                    SelectedSlotActivation(int.Parse(curActiveNumberSelected), false);
                }
                SelectedSlotActivation(int.Parse(activeNum), true);
                curActiveNumberSelected = activeNum;
            }
            else
            {
                isChangedCalled = true;
            }
        }

        private void SelectedSlotActivation(int itemNumber, bool isEnabled)
        {
            ASB.slots[itemNumber].enabled = isEnabled;

            slotAnimator = ASB.slots[itemNumber].GetComponent<Animator>();
            slotAnimator.SetBool("slotSelected", isEnabled);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDraggingEnabled = false;
            try
            {
                if(curActiveNumberSelected != "")
                {
                    SelectedSlotActivation(int.Parse(curActiveNumberSelected), false);
                }
                
                PutItemInSlot(eventData.pointerCurrentRaycast.gameObject, eventData);
            }
            catch (Exception){
            }
        }
        #endregion

        #region Click Events
        public void OnPointerClick(PointerEventData eventData)
        {
            try
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "NewlySpawned")
                {
                    pipeIsSelected = true;
                    randomizedItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
            catch (Exception)
            {

            }
            //print("PipeSelected!");

        }
        #endregion

        #region Functionalities
        public void PutItemInSlot(GameObject currentSlot, PointerEventData eventData)
        {
            curSlotName = new string[2];
            //print(eventData.pointerCurrentRaycast.gameObject.name);
            if (currentSlot != null)
            {
                curSlotName = currentSlot.name.Split('_');
            }
            if (curSlotName[0] == "slot")
            {
                //rename it so it does not activate again. this should be the first call
                currentSlot.name = "slotF_" + curSlotName[1];

                //NOT YET IMPLEMENTED
                //Event trigger to able to rotate the image which get triggered trigger
                slotTrigger = eventData.pointerCurrentRaycast.gameObject.GetComponent<PipeBehaviour>();
                slotTrigger.EventTrigger();

                //Then show the image in the slot
                Image slotsImage = currentSlot.GetComponent<Image>();
                slotsImage.sprite = randomizedItem.sprite;
                //randomizedItem.transform.SetParent(currentSlot.transform, false);

                //It was already 
                Destroy(randomizedItem.gameObject);

                randomizedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
                RandomNewItem();
            }
            else
            {
                randomizedItem.transform.position = originalParent.transform.position;
                randomizedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        //// /
        private void RandomNewItem()
        {
            elementNum = IDC.GetItem();
            if (DevTest.chk_DropChance) { print("Pipe element: " + elementNum); }
            SpawnItem();
        }

        public void SpawnItem()
        {
            randomizedItem = Instantiate(Slot);
            randomizedItem.transform.SetParent(originalParent, false);
            randomizedItem.name = "NewlySpawned";

            randomizedItem.sprite = Items[elementNum].Item;
        }
        #endregion
    }
}
