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

    private void Update()
    {
        if (currentActiveUI == null) { return; }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentActiveUI == gameOverUI) { return; }

            if (currentActiveUI == buildingUI)
            {
                ShowPauseMenu();
                return;
            }

            if (currentActiveUI == pauseMenuUI)
            {
                ShowBuildMenu();
                return;
            }
        }
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
        currentActiveUI = null;
    }

    public void ShowGameOverDisplay()
    {
        HideAllUI();
        gameOverUI.SetActive(true);
        currentActiveUI = gameOverUI;
    }

    public void ShowPauseMenu()
    {
        HideAllUI();
        pauseMenuUI.SetActive(true);
        currentActiveUI = pauseMenuUI;
    }

    public void ShowBuildMenu()
    {
        HideAllUI();
        buildingUI.SetActive(true);
        currentActiveUI = buildingUI;
    }
}
