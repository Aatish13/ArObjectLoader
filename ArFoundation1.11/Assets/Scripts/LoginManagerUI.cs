using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Boomlagoon.JSON;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class LoginManagerUI : MonoBehaviour
{

    public GameObject ARCanvas;
    public GameObject LoginCanvas;

    public GameObject HomePanel;
    public GameObject LoginPanal;

    public GameObject DashBoardPanel;
    public GameObject ProjectsPanal;

    public GameObject LoadingPanal;

    public Text UserName;
    public Text ErrorText;

    public GameObject DownloadBtn;
    public GameObject OpenBtn;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            ARCanvas.SetActive(false);
            LoginCanvas.SetActive(true);
            HomePanel.SetActive(false);
            LoginPanal.SetActive(false);
            DashBoardPanel.SetActive(true);
            ProjectsPanal.SetActive(false);
            LoadingPanal.SetActive(false);
            UserName.text = PlayerPrefs.GetString("displayname");
        }
        else
        {
            ARCanvas.SetActive(false);
            LoginCanvas.SetActive(true);
            HomePanel.SetActive(true);
            LoginPanal.SetActive(false);
            DashBoardPanel.SetActive(false);
            ProjectsPanal.SetActive(false);
            LoadingPanal.SetActive(false);
        }
        
    }

    public void GotoLogin()
    {
        HomePanel.SetActive(false);
        LoginPanal.SetActive(true);
    }

    public InputField emailInput;
    public InputField passwordInput;

    public void GotoDashBoard()
    {
        //LoginPanal.SetActive(false);
        //LoginCanvas.SetActive(false);
        //ARCanvas.SetActive(true);
        LoadingPanal.SetActive(true);
        StartCoroutine(Upload());

    }

    string url = "https://studiooneeleven.co/api/user/generate_auth_cookie/";
    IEnumerator Upload()
    {

        // string jsonString = "{ \"email\":\"" + emailInput.text + "\",\"password\":\"" + passwordInput.text+"\"}";
        // Debug.Log(jsonString);

        // byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);

        WWWForm form = new WWWForm();
        form.AddField("email", emailInput.text);
        form.AddField("password", passwordInput.text);
        // request.uploadHandler = (UploadHandler)new UploadHandlerRaw(form);

        var request = UnityWebRequest.Post(url, form);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            ErrorText.text = "Error: " + request.error;
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string responseText = request.downloadHandler.text;
            JSONObject json = JSONObject.Parse(responseText);
            // Print Headers
            Debug.Log(responseText);
            Debug.Log("Form upload complete!");
            Debug.Log(json["status"].ToString());
            //Debug.Log(json["cookie"].ToString());
            if (json["cookie"] != null)
            {
                UserName.text = json.GetObject("user").GetString("displayname");
                Debug.Log(json.GetObject("user").GetString("displayname"));
                username = json.GetObject("user").GetString("username");
                PlayerPrefs.SetString("username",username);
                PlayerPrefs.SetString("displayname", json.GetObject("user").GetString("displayname"));

                LoginPanal.SetActive(false);
                LoadingPanal.SetActive(false);
                ARCanvas.SetActive(false);
                DashBoardPanel.SetActive(true);

            }
            else
            {
                ErrorText.text = json["error"].ToString();
                Debug.Log(json["error"].ToString());
                LoginPanal.SetActive(false);
            }

        }
    }

    class Project {
        public double id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool isloaded = false;
        public GameObject LoadedObj { get; set; }
}
    public string username;
    List<Project> Projects=new List<Project>();

    public Text ProjectName;
    int projectIndex=0;
    IEnumerator fetchProjects() {
        var request = UnityWebRequest.Get("https://studiooneeleven.co/wp-json/wp/v2/wpfm-files?per_page=100");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            ErrorText.text = "Error: " + request.error;

            LoadingPanal.SetActive(false);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            string responseText = request.downloadHandler.text;


        
            var obj = JSONArray.Parse(responseText);
            string username2 = PlayerPrefs.GetString("username");
            foreach (var o in obj) {
                if (username2.Equals(o.Obj.GetString("author_name")))
                {

                    Projects.Add(new Project { id = o.Obj.GetNumber("id"), name = o.Obj.GetString("wpfm_file_name"), url = o.Obj.GetString("wpfm_file_url") });
                }
            }
            foreach(Project p in Projects)
            {
                Debug.Log(p.url);
            }
            ProjectName.text = Projects[0].name;

            LoadingPanal.SetActive(false);
            ProjectsPanal.SetActive(true);
            DashBoardPanel.SetActive(false);
            // Print Headers
            Debug.Log(responseText);
           

        }

    }
    void chekDownloadStatus() {
        if (Projects[projectIndex].isloaded)
        {
            DownloadBtn.SetActive(false);
            OpenBtn.SetActive(true);

        }
        else
        {
            DownloadBtn.SetActive(true);
            OpenBtn.SetActive(false);
        }
    }

    public void NextProject() {

        if (projectIndex + 1 < Projects.Count)
        {
            projectIndex++;
            ProjectName.text = Projects[projectIndex].name;

            chekDownloadStatus();

        }

    }
    public void PreviousProject()
    {
        if (projectIndex - 1 >=0)
        {

            projectIndex--;
            ProjectName.text = Projects[projectIndex].name;

            chekDownloadStatus();

        }

    }


    public void GoToProjects() {
        if (Projects.Count == 0)
        {
            DashBoardPanel.SetActive(true);
            ProjectsPanal.SetActive(false);
            LoginPanal.SetActive(false);
            LoadingPanal.SetActive(true);
            StartCoroutine(fetchProjects());
        }
        else
        {

            chekDownloadStatus();
            DashBoardPanel.SetActive(false);
            ProjectsPanal.SetActive(false);
            LoginPanal.SetActive(false);
            LoadingPanal.SetActive(false);
            ProjectsPanal.SetActive(true);

        }

    }
    public InputField Input;

    public void DownloadProject() {
        Input.text = Projects[projectIndex].url.Replace("\\", "");

        if (Projects[projectIndex].isloaded == false)
        {
            LoadObj();
            Projects[projectIndex].isloaded = true;
        }
        else
        {
            PlaceOnPlane.AssatObj = Projects[projectIndex].LoadedObj;

        }

        chekDownloadStatus();
    }

    public void GoToAr()
    {

        Input.text = Projects[projectIndex].url.Replace("\\", "");
      
        if (Projects[projectIndex].isloaded == false)
        {
           
            LoadObj();
            Projects[projectIndex].isloaded = true;
        }
        else {
            PlaceOnPlane.AssatObj = Projects[projectIndex].LoadedObj;
           
        }
        
        ARCanvas.SetActive(true);
        ProjectsPanal.SetActive(false);
    }



    public void LoadObj()
    {
        if (PlaceOnPlane.AssatObj != null)
        {
            //PlaceOnPlane.AssatObj.active = false;
            PlaceOnPlane.AssatObj = null;
        }
        LoadingPanal.SetActive(true);
        WWW www2 = new WWW(Projects[projectIndex].url.Replace("\\", ""));
        StartCoroutine(WaitForReq(www2));
    }
    IEnumerator WaitForReq(WWW www)
    {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            var names = bundle.GetAllAssetNames();

            GameObject cube = (GameObject)bundle.LoadAsset(names[0]);
            // spawnedObject = Instantiate(cube);
            Projects[projectIndex].LoadedObj = cube;
            PlaceOnPlane.AssatObj = cube;

            LoadingPanal.SetActive(false);
        }
        else
        {
            LoadingPanal.SetActive(false);
            Debug.Log(www.error);
        }
    }



    public void BackToHome() {
        HomePanel.SetActive(true);
        LoginPanal.SetActive(false);
    }
    public void BackToDashBoard()
    {
        HomePanel.SetActive(false);
        LoginPanal.SetActive(false);
        ProjectsPanal.SetActive(false);
        ARCanvas.SetActive(false);
        DashBoardPanel.SetActive(true);
    }
    public void BackToProjects()
    {
        HomePanel.SetActive(false);
        LoginPanal.SetActive(false);
        ProjectsPanal.SetActive(true);
        ARCanvas.SetActive(false);
        DashBoardPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
