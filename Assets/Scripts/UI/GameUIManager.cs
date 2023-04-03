using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject currentActiveUI = null;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject buildingUI;
    [SerializeField] GameObject pauseMenuUI;

    List<GameObject> gameOverUIList;

    private void Awake()
    {
        gameOverUIList = new()
        {
            gameOverUI,
            buildingUI,
            pauseMenuUI
        };

        currentActiveUI = buildingUI;
    }

    private void Start()
    {
        ShowBuildMenu();
    }

    private void HideAllUI()
    {
        foreach (var item in gameOverUIList)
        {
            item.SetActive(false);
        }
    }

    public void ShowGameOverDisplay()
    {
        HideAllUI();
        gameOverUI.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        HideAllUI();
        pauseMenuUI.SetActive(true);
    }

    public void ShowBuildMenu()
    {
        HideAllUI();
        buildingUI.SetActive(true);
        currentActiveUI = buildingUI;
    }
}
