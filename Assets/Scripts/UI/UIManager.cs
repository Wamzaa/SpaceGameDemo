using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool isInGame;
    public GameObject gameUI;
    public GameObject editorUI;

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
    }
}
