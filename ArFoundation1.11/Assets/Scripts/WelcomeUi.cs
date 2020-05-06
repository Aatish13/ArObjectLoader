using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeUi : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject LandingPanel;
    public GameObject Guide1;
    public GameObject Guide2;
    public GameObject Guide3;
    public GameObject SigneUpPanel;
    void Start()
    {
        if (PlayerPrefs.HasKey("intro")==false)
        {
            LandingPanel.SetActive(true);
        }
        else {
            LandingPanel.SetActive(false);
            //SigneUpPanel.SetActive(true);
        }
      
    }
    public void  GoToGuide1() {
        LandingPanel.SetActive(false);
        Guide1.SetActive(true);
    }
    public void GoToGuide2()
    {
      
        Guide1.SetActive(false);
        Guide2.SetActive(true);
    }
    public void GoToGuide3()
    {
        Guide2.SetActive(false);
        Guide3.SetActive(true);
    }

    public void GoTOSignUp() {
        Guide3.SetActive(false);
        SigneUpPanel.SetActive(true);
        PlayerPrefs.SetString("intro","true");
    }
    // Update is called once per frame

}
