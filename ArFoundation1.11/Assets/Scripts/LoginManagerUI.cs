using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManagerUI : MonoBehaviour
{

    public GameObject ARCanvas;
    public GameObject LoginCanvas;

    public GameObject HomePanel;
    public GameObject LoginPanal;
    // Start is called before the first frame update
    void Start()
    {
        ARCanvas.SetActive(false);
        LoginCanvas.SetActive(true);
        HomePanel.SetActive(true);
        LoginPanal.SetActive(false);
    }

    public void GotoLogin()
    {
        HomePanel.SetActive(false);
        LoginPanal.SetActive(true);
    }

    public void GotoDashBoard()
    {
        LoginPanal.SetActive(false);
        LoginCanvas.SetActive(false);
        ARCanvas.SetActive(true);
    }

    public void BackToHome() {
        HomePanel.SetActive(true);
        LoginPanal.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
