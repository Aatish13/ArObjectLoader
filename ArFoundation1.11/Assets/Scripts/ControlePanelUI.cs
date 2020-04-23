using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ControlePanelUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ControlePanel;
    public GameObject MoveXZPanel;
    public GameObject ScalePanel;
    public GameObject PhotoPanel;
    public GameObject MoveYPanel;
    public GameObject RotatePanel;
    public GameObject ControleBtn;
    void Start()
    {
        ControlePanel.SetActive(false);
        MoveXZPanel.SetActive(false);
        ScalePanel.SetActive(false);
        PhotoPanel.SetActive(false);
        MoveYPanel.SetActive(false);
        RotatePanel.SetActive(false);
    }

    public void ShowControlePanel() {
        ControleBtn.SetActive(false);
        ControlePanel.SetActive(true);
    }

    public void ShowMoveXZPanel()
    {
        ControlePanel.SetActive(false);
        MoveXZPanel.SetActive(true);
    }
    public void ShowScalePanel()
    {
        ControlePanel.SetActive(false);
        ScalePanel.SetActive(true);
    }
    public void ShowPhotoPanel()
    {
        ControlePanel.SetActive(false);
        PhotoPanel.SetActive(true);

    }
    public void ShowMoveYPanel()
    {
        ControlePanel.SetActive(false);
        MoveYPanel.SetActive(true);
    }
    public void ShowRotatePanel()
    {
        ControlePanel.SetActive(false);
        RotatePanel.SetActive(true);
    }

    public void CloseRotatePanel() {
        RotatePanel.SetActive(false);
        ControlePanel.SetActive(true); 
    }
    public void CloseMoveYPanel()
    {
        MoveYPanel.SetActive(false);
        ControlePanel.SetActive(true);
    }
    public void ClosePhotoPanel()
    {
        PhotoPanel.SetActive(false);
        ControlePanel.SetActive(true);
    }
    public void CloseScalePanel()
    {
        ScalePanel.SetActive(false);
        ControlePanel.SetActive(true);
    }
    public void CloseMoveXZPanel()
    {
        MoveXZPanel.SetActive(false);
        ControlePanel.SetActive(true);
    }


    public void CloseAll() {
        ControlePanel.SetActive(false);
        MoveXZPanel.SetActive(false);
        ScalePanel.SetActive(false);
        PhotoPanel.SetActive(false);
        MoveYPanel.SetActive(false);
        RotatePanel.SetActive(false);
        ControleBtn.SetActive(true);
    }
}