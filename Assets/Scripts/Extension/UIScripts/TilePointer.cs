
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilePointer : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Title;
    public GameObject ControlPanel;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)&& !Title.activeSelf && !ControlPanel.activeSelf)
        {           
            StartCoroutine(ShowBySecound());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Title.activeSelf && !ControlPanel.activeSelf)
        {
            StartCoroutine(ShowBySecound());
        }
    }

    IEnumerator ShowBySecound()
    {
        Title.SetActive(true);
        ControlPanel.SetActive(true);
        yield return new WaitForSeconds(10);
        Title.SetActive(false);
        ControlPanel.SetActive(false);
        StopCoroutine(ShowBySecound());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Title.SetActive(false);
        ControlPanel.SetActive(false);
    }
}
