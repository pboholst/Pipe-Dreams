using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/** Currently Working
 *  Last Event: 191215 - Worked on rotation. Get the element NumberOfRotations from RIG and
 *  Let that determined how number of rot.
 *  Let the L pipe rotate 2 ways only and back to original.
 * 
 */
namespace PipeDreams
{
    public class PipeBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        //[Tooltip("The amount of time to finalize the movement. After time runs out no movement allowed.")]
        [Range(0.0f, 5.0f)]
        public float TimeToFinalize = 5.0f;

        private RandomItemGenarator RIG;
        private string pipeName;
        private string pipeNumber;
        private string[] pipeFullName;

        public RectTransform rectTransform;

        //timer
        private float secondsCount = 0f;
        private bool startCounting = false;
        private bool finalizeMovementAndRotation = false;

        public NumberOfRotations numOfRot;

        private bool draggingIsEnabled = false;

        private MouseCharacteristic MC;
        public void Awake()
        {
            RIG = GameObject.FindObjectOfType<RandomItemGenarator>();
            MC = GameObject.FindObjectOfType<MouseCharacteristic>();

            /** Reference used for rotation:
              *      Getting reference here so it gets called once instead of getting
              *      called all the time when the item is clicked for rotation.
              */
            rectTransform = this.GetComponent<RectTransform>();
        }
        private void Update()
        {
            TimeCounter();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //print();
            SetNaming();
            if(RIG.pipeIsSelected == true)
            {
                RIG.pipeIsSelected = false;
                if (pipeName == "slot")
                {
                    //Pipe selected will be put in this slot
                    RIG.PutItemInSlot(this.gameObject, eventData);
                }
            }

            if (pipeName == "slotF")
            {
                RIG.pipeIsSelected = false;
                if (numOfRot != NumberOfRotations.None)
                {
                    RotatePipe();
                }
            }
        }
        public void EventTrigger()
        {
            //print("start counting! " + this.name);
            draggingIsEnabled = true;

            //print("PipeBehaviour EventTrigger() is called!");
            //Get the number of rotation for the pipe that was put in.
            numOfRot = RIG.Items[RIG.elementNum].NumberOfRotation;
            //print(RIG.Items[RIG.arrayNum].NumberOfRotation);
            //print((int)RIG.Items[RIG.arrayNum].NumberOfRotation);

            if(numOfRot != NumberOfRotations.None)
            {
                startCounting = true;
            }
        }

        private void SetNaming()
        {
            pipeFullName = this.name.Split('_');
            pipeName = pipeFullName[0];
            pipeNumber = pipeFullName[1];
        }

        private void RotatePipe()
        {
            startCounting = true;
            //secondsCount = 0;
            if (finalizeMovementAndRotation == false)
            {
                //Rotate this slot by 90 degree
                rectTransform.Rotate(new Vector3(0, 0, 90));
            }
            else
            {
                startCounting = false;
            }
        }

        private void TimeCounter()
        {
            if (startCounting && finalizeMovementAndRotation == false)
            {
                secondsCount += Time.deltaTime;
                //print(secondsCount);
                if(secondsCount >= TimeToFinalize)
                {
                    startCounting = false;
                    finalizeMovementAndRotation = true;
                    secondsCount = 0f;
                    //print("ended counting! " + this.name);
                }
            }
        }
        string[] pipeNameFilled = new string[2];
        private string GetName(PointerEventData eventData)
        {
            try
            {
                pipeNameFilled = eventData.pointerCurrentRaycast.gameObject.name.Split('_');
            }
            catch (System.Exception)
            {
            }
            return pipeNameFilled[0];
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            try
            {
                if (GetName(eventData) == "slotF" && finalizeMovementAndRotation == false)
                {
                    if (draggingIsEnabled == true)
                    {
                        print("OnDrag(): where we going!");
                        MC.CurObjDragging(this.gameObject);

                    }
                    else
                    {
                        print("OnDrag(): end of time i am going back!");
                    }
                }
                else
                {
                    print(eventData.pointerCurrentRaycast.gameObject.name);
                    RIG.ActiveHighlight(eventData);
                }
            }
            catch (System.Exception)
            {

            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //print("dragging is done");
            MC.isMovingEnabled = false;
            if (GetName(eventData) == "slotF")
            {
                draggingIsEnabled = false;
                if (finalizeMovementAndRotation == false)
                {

                }
            }
        }
    }
}
