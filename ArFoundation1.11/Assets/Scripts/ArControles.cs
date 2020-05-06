using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArControles : MonoBehaviour
{
    void Start() {
     //   ArSession.SetActive(false);
    }

    public GameObject ArSession;
    public GameObject ArModePanel;

    public GameObject ArMenuPanel;
    public GameObject ArMenuHelp;

    public GameObject ArTransformMenuPanel;
    public GameObject ArTransformMenuHelp;

    public GameObject ArMoveXZPanel;
    public GameObject ArMoveXZHelp;

    public GameObject ArRotatePanel;
    public GameObject ArRotateHelp;

    public GameObject ArScalePanel;
    public GameObject ArScaleHelp;

    public GameObject ArMoveYPanel;
    public GameObject ArMoveYHelp;



    public void OpenMenu() {
        ArModePanel.SetActive(false);
        ArMenuPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        ArMenuPanel.SetActive(false);
        ArModePanel.SetActive(true);
    }

    public void OpenhelpMenu() {
        ArMenuHelp.SetActive(true);
    }
    public void ClosehelpMenu()
    {
        ArMenuHelp.SetActive(false);
    }



    public void OpenTransformMenu() {
        ArMenuPanel.SetActive(false);
        ArTransformMenuPanel.SetActive(true);

    }

    public void CloseTransformMenu()
    {
        ArTransformMenuPanel.SetActive(false);
        ArMenuPanel.SetActive(true);

    }

    public void OpenTransformMenuHelp()
    {
   
        ArTransformMenuHelp.SetActive(true);

    }

    public void CloseTransformMenuHelp()
    {
        ArTransformMenuHelp.SetActive(false);
        
    }


    public void OpenMoveXZ() {
        ArTransformMenuPanel.SetActive(false);
        ArMoveXZPanel.SetActive(true);
    }
    public void CloseMoveXZ()
    {
        ArTransformMenuPanel.SetActive(true);
        ArMoveXZPanel.SetActive(false);
    }

    public void OpenMoveXZHelp()
    {
      
        ArMoveXZHelp.SetActive(true);
    }
    public void CloseMoveXZHelp()
    {
      
        ArMoveXZHelp.SetActive(false);
    }

    public void OpenMoveY()
    {
        ArTransformMenuPanel.SetActive(false);
        ArMoveYPanel.SetActive(true);
    }
    public void CloseMoveY()
    {
        ArTransformMenuPanel.SetActive(true);
        ArMoveYPanel.SetActive(false);
    }
    public void OpenMoveYHelp()
    {
      
        ArMoveYHelp.SetActive(true);
    }
    public void CloseMoveYHelp()
    {
       
        ArMoveYHelp.SetActive(false);
    }

    public void OpenScalePanel()
    {
        ArTransformMenuPanel.SetActive(false);
        ArScalePanel.SetActive(true);
    }
    public void CloseScalePanel()
    {
        ArTransformMenuPanel.SetActive(true);
        ArScalePanel.SetActive(false);
    }
    public void OpenScaleHelp()
    {

        ArScaleHelp.SetActive(true);
    }
    public void CloseScaleHelp()
    {
     
        ArScaleHelp.SetActive(false);
    }
    public void OpenRotatePanel()
    {
        ArTransformMenuPanel.SetActive(false);
        ArRotatePanel.SetActive(true);
    }
    public void CloseRotatePanel()
    {
        ArRotatePanel.SetActive(false);
        ArTransformMenuPanel.SetActive(true);
        
    }
    public void OpenRotateHelp()
    {

        ArRotateHelp.SetActive(true);
    }
    public void CloseRotateHelp()
    {

        ArRotateHelp.SetActive(false);

    }
}
