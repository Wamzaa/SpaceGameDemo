using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool isInGame;
    public GameObject gameUI;
    public GameObject editorUI;

    private ComponentBehaviour selectedComponent;
    private ComponentBehaviour hoveredComponent;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("ERROR : MenuManager was already instanciated");
        }
    }

    private void Start()
    {
        SetGameMode(true);
    }


    public void SetGameMode(bool _isInGame)
    {
        isInGame = _isInGame;
        gameUI.SetActive(isInGame);
        editorUI.SetActive(!isInGame);
        SetSelected(null);
        SetHovered(null);
    }

    public void SetHovered(ComponentBehaviour comp)
    {
        if(comp != selectedComponent)
        {
            if (comp != hoveredComponent && hoveredComponent != null)
            {
                hoveredComponent.GetComponent<EditorComponent>().SetHovered(false);
            }
            hoveredComponent = comp;
            if (hoveredComponent != null)
            {
                hoveredComponent.GetComponent<EditorComponent>().SetHovered(true);
            }
        }
    }

    public void SetSelected(ComponentBehaviour comp)
    {
        if (comp != selectedComponent && selectedComponent != null)
        {
            selectedComponent.GetComponent<EditorComponent>().SetSelected(false);
        }
        selectedComponent = comp;
        if (selectedComponent != null)
        {
            selectedComponent.GetComponent<EditorComponent>().SetSelected(true);
        }
    }
}
