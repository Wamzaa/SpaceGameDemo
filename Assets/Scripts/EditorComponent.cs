using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorComponent : MonoBehaviour
{
    public GameObject selectFilter;
    public GameObject hoverFilter;
    
    private ComponentBehaviour comp;

    private void Start()
    {
        comp = this.transform.GetComponent<ComponentBehaviour>();
        selectFilter.SetActive(false);
        hoverFilter.SetActive(false);
    }

    private void OnMouseOver()
    {
        if (!UIManager.Instance.isInGame)
        {
            UIManager.Instance.SetHovered(comp);
        }
    }

    private void OnMouseDown()
    {
        if (!UIManager.Instance.isInGame)
        {
            UIManager.Instance.SetSelected(comp);
        }
    }

    public void SetSelected(bool selected)
    {
        Debug.Log("Set Selected : " + selected);
        if (selected)
        {
            selectFilter.SetActive(true);
            hoverFilter.SetActive(false);
        }
        else
        {
            selectFilter.SetActive(false);
        }
    }

    public void SetHovered(bool hovered)
    {
        Debug.Log("Set Hovered : " + hovered);
        if (hovered)
        {
            hoverFilter.SetActive(true);
            selectFilter.SetActive(false);
        }
        else
        {
            hoverFilter.SetActive(false);
        }
    }
}
