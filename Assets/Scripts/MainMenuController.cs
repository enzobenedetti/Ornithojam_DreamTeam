using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject PaneMainMenu;
    public GameObject PanelLevels;
    public GameObject PanelCredits;
    public GameObject PanelControls;

    public Button BPPlay;
    public Button BPLevels;
    public Button BPControls;
    public Button BPCredits;
    public Button BPLvl1;
    public Button BPLevelReturn;
    public Button BPCreditReturn;
    public Button BPControlsReturn;

    private void Start() {
        BPPlay.Select();
    }

    public void UIBPPlay() {
        Debug.Log("Lance le Jeu");
        SceneManager.LoadScene(7);
    }

    public void UIBPLevel() {
        PaneMainMenu.SetActive(false);
        PanelLevels.SetActive(true);
        BPLvl1.Select();
    }

    public void UIBPLevelReturn() {
        PaneMainMenu.SetActive(true);
        PanelLevels.SetActive(false);
        BPLevels.Select();
    }
    
    public void UIBPCredits() {
        PaneMainMenu.SetActive(false);
        PanelCredits.SetActive(true);
        BPCreditReturn.Select();
    }

    public void UIBPCreditReturn() {
        PaneMainMenu.SetActive(true);
        PanelCredits.SetActive(false);
        BPCredits.Select();
    }
    public void UIBPControl() {
        PaneMainMenu.SetActive(false);
        PanelControls.SetActive(true);
        BPControlsReturn.Select();
    }

    public void UIBPControlReturn() {
        PaneMainMenu.SetActive(true);
        PanelControls.SetActive(false);
        BPControls.Select();
    }

    public void UIBPLoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    
}
