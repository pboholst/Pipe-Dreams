using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseCharacteristic : MonoBehaviour
{
    public GameObject currentObjectDragging;
    public GameObject onMouseObject;
    public bool isMovingEnabled = false;

    private Image onMouseImage;
    //private Transform originalParent;

    private void Awake()
    {
        onMouseImage = onMouseObject.GetComponent<Image>();
        ChangeOnMouseColor(0f);
    }

    public void CurObjDragging(GameObject thisGO)
    {
        //currentObjectDragging = thisGO;
        ChangeOnMouseColor(1f);
        onMouseObject.GetComponent<Image>().sprite = thisGO.GetComponent<Image>().sprite;
        isMovingEnabled = true;
    }

    private void Update()
    {
        if (isMovingEnabled)
        {
            onMouseObject.transform.position = Input.mousePosition;
            onMouseObject.SetActive(true);
        }
        else
        {
            ChangeOnMouseColor(0f);
        }
    }
    //0 hide | 1 show
    private void ChangeOnMouseColor(float alpha)
    {
        var tempColor = onMouseImage.color;
        tempColor.a = alpha;
        onMouseImage.color = tempColor;
    }

}
