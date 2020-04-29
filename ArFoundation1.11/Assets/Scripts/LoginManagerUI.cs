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
    // Start is called before the first frame update
    void Start()
    {
        ARCanvas.SetActive(false);
        LoginCanvas.SetActive(true);
        HomePanel.SetActive(true);
        LoginPanal.SetActive(false);
        DashBoardPanel.SetActive(false);
        ProjectsPanal.SetActive(false);

        LoadingPanal.SetActive(false);
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

    class Project{
        public double id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
}
    public string username;
    List<Project> Projects=new List<Project>();

    public Text ProjectName;
    int projectIndex=0;
    IEnumerator fetchProjects() {
        var request = UnityWebRequest.Get("https://studiooneeleven.co/wp-json/wp/v2/wpfm-files");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         request.SetRequestHeader("Content-Type", "application/json");
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


        
            var obj = JSONArray.Parse(responseText);
           
            foreach (var o in obj) {
                if (username.Equals(o.Obj.GetString("author_name")))
                {

                    Projects.Add(new Project { id = o.Obj.GetNumber("id"), name = o.Obj.GetString("wpfm_file_name"), url = o.Obj.GetString("wpfm_file_url") });
                }
            }
            foreach(Project p in Projects)
            {
                Debug.Log(p.url);
            }
            ProjectName.text = Projects[0].name;
            LoginPanal.SetActive(false);
            // Print Headers
            Debug.Log(responseText);
           

        }

    }

    public void NextProject() {
        if (projectIndex + 1 < Projects.Count)
        {
            projectIndex++;
            ProjectName.text = Projects[projectIndex].name;

        }

    }
    public void PreviousProject()
    {
        if (projectIndex - 1 >=0)
        {
            projectIndex--;
            ProjectName.text = Projects[projectIndex].name;

        }

    }


    public void GoToProjects() {

        DashBoardPanel.SetActive(false);
        ProjectsPanal.SetActive(true);
        LoginPanal.SetActive(true);
        StartCoroutine(fetchProjects());
    }
    public InputField Input;

    public void GoToAr()
    {
        Input.text = Projects[projectIndex].url.Replace("\\","");
        ARCanvas.SetActive(true);
        ProjectsPanal.SetActive(false);
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
