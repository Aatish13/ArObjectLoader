using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashboardUI : MonoBehaviour
{

    public GameObject Manual;
    public List<GameObject> ManualPages=new List<GameObject>();
    int ManualIndex = 0;

    public GameObject HelpDesk;

    public GameObject JoinRoom;
    public GameObject JoinRoomP1;
    public GameObject JoinRoomP2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenMenuDesk() {
        ManualIndex = 0;
        foreach (GameObject g in ManualPages) {
            g.SetActive(false);
          
        }
        Manual.SetActive(true);
        ManualPages[0].SetActive(true);
    }
    public void CloseMenuDesk() {
        Manual.SetActive(false);
        foreach (GameObject g in ManualPages)
        {
            g.SetActive(false);

        }
    }

 
    public void NextManual() {
            if (ManualIndex <= ManualPages.Count - 1) {

            ManualPages[ManualIndex].SetActive(false);

            ManualIndex++;
            ManualPages[ManualIndex].SetActive(true);
        }
    }
    public void previousManual()
    {
         if (ManualIndex >0)
            {

            ManualPages[ManualIndex].SetActive(false);

            ManualIndex--;
            ManualPages[ManualIndex].SetActive(true);
        }
    }

    public void OpenHelpDesk() {
        HelpDesk.SetActive(true);

    }
    public void CloseHelpDesk() {
        HelpDesk.SetActive(false);
    }

    public void OpenJoinRoom() {
        JoinRoomP2.SetActive(false);
        JoinRoomP1.SetActive(true);
        JoinRoom.SetActive(true);
    }

    public void CloseJoinRoom() {
        JoinRoom.SetActive(false);
    }

    public void OpenJoinRoomP2() {
        JoinRoomP1.SetActive(false);
        JoinRoomP2.SetActive(true);
    }
    public void CloseJoinRoomP2()
    {
        JoinRoomP2.SetActive(false);
        JoinRoomP1.SetActive(true);
    }

}
