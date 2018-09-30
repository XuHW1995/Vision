using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject rightpage;
    public GameObject buttonhide;
    public GameObject buttonshow;
    public void OnPointerEnter(PointerEventData eventData)
    {      
        //Debug.Log("进入");
        if (rightpage.activeSelf)
        {
            buttonhide.SetActive(true);
            buttonshow.SetActive(false);
        }
        else
        {
            buttonhide.SetActive(false);
            buttonshow.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("移出");
        buttonhide.SetActive(false);
        buttonshow.SetActive(false);
    }

}
