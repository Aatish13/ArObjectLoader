using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Boomlagoon.JSON;
using Newtonsoft.Json.Linq;
using System;


using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using SimpleFileBrowser;
public class LoginManagerUI : MonoBehaviour
{




    public GameObject LoginPanal;

    public GameObject DashBoardPanel;
    public GameObject ProjectsPanal;

    public GameObject LoadingPanal;

    public Text UserName;
    public RawImage ProfileImage;
    public Text ErrorText;

    public GameObject DownloadBtn;
    public GameObject OpenBtn;

    public GameObject DashboardMenu;

    public Texture2D DefaultProjectTexture;
    // Start is called before the first frame update
    void Start()
    {
        ArSession.SetActive(false);
        if (PlayerPrefs.HasKey("username"))
        {
            StartCoroutine(setImage(PlayerPrefs.GetString("profileimg"), ProfileImage));


            LoginPanal.SetActive(false);
            DashBoardPanel.SetActive(true);
            ProjectsPanal.SetActive(false);
            LoadingPanal.SetActive(false);
            UserName.text = PlayerPrefs.GetString("displayname");


        }
        else if (PlayerPrefs.HasKey("intro"))
        {
        
            LoginPanal.SetActive(true);
            DashBoardPanel.SetActive(false);
            ProjectsPanal.SetActive(false);
            LoadingPanal.SetActive(false);
        }
        else {
  
            LoginPanal.SetActive(false);
            DashBoardPanel.SetActive(false);
            ProjectsPanal.SetActive(false);
            LoadingPanal.SetActive(false);
        }
        
    }
    public void OpenRegistrationLink() {
        StartCoroutine(waitfor1sec());
        Application.OpenURL("https://studiooneeleven.co/registration/");

    }
    public void OpenResetPasswordLink() {
        StartCoroutine(waitfor1sec());

        Application.OpenURL("https://studiooneeleven.co/password-reset/");
    }
    IEnumerator waitfor1sec() {
        LoadingPanal.SetActive(true);
        yield return new WaitForSeconds(1);
        LoadingPanal.SetActive(false);
    }
    public void OpenDashboardMenu() {
        DashboardMenu.SetActive(true);
    }
    public void CloseDashboardMenu()
    {
        DashboardMenu.SetActive(false);
    }
    public void LogOut() {
        Projects = new List<Project>();
        projectIndex = 0;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("intro", "true");
        DashboardMenu.SetActive(false);

        LoginPanal.SetActive(true);
        DashBoardPanel.SetActive(false);
        ProjectsPanal.SetActive(false);
        LoadingPanal.SetActive(false);
    }

    public void GotoLogin()
    {
        
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
                Debug.Log("Profile image link :::" + json.GetObject("user").GetString("avatar").Replace("\\", ""));
                PlayerPrefs.SetString("profileimg", json.GetObject("user").GetString("avatar").Replace("\\", ""));
                StartCoroutine(setImage(PlayerPrefs.GetString("profileimg"), ProfileImage));
                ErrorText.text = "";
                LoginPanal.SetActive(false);
                LoadingPanal.SetActive(false);
            
                DashBoardPanel.SetActive(true);

            }
            else
            {
               
                ErrorText.text = json["error"].ToString();
                Debug.Log(json["error"].ToString());
                LoginPanal.SetActive(true);
                LoadingPanal.SetActive(false);
            }

        }
    }
    IEnumerator setImage(string url, RawImage rowimg)
    {
      //  Texture2D texture = ProfileImage.canvasRenderer.GetMaterial().mainTexture as Texture2D;

        WWW www = new WWW(url);
        yield return www;

        // calling this function with StartCoroutine solves the problem
        Debug.Log("Why on earh is this never called?");
        rowimg.color = Color.white;
        rowimg.texture = www.texture;
      // www.LoadImageIntoTexture((Texture2D)ProfileImage.texture);
        www.Dispose();
        www = null;
    }

    class Project {
        public double id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public bool isloaded = false;
        public GameObject LoadedObj { get; set; }

        public string size { get; set; }
        public string description { get; set; }

        public string photourl { get; set; }
        public Texture2D imgTexture { get; set; }

}

    IEnumerator setProjectImage(string url, RawImage rowimg,int index)
    {
        //  Texture2D texture = ProfileImage.canvasRenderer.GetMaterial().mainTexture as Texture2D;

        WWW www = new WWW(url);
        yield return www;

        // calling this function with StartCoroutine solves the problem
        Debug.Log("Why on earh is this never called?");
       
        rowimg.color = Color.white;
        rowimg.texture = www.texture;
        if(index==projectIndex)
            Projects[index].imgTexture = www.texture;
       // www.LoadImageIntoTexture((Texture2D)ProfileImage.texture);
       www.Dispose();
        www = null;
    }
    public string username;
    List<Project> Projects=new List<Project>();
    public RawImage ProjectImage;
    public Text ProjectName;
    public Text ProjectTitle;
    int projectIndex=0;
    IEnumerator fetchProjects() {
        var request = UnityWebRequest.Get("https://studiooneeleven.co/wp-json/wp/v2/wpfm-files?per_page=100&_embed");
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
                   
                      Debug.Log( o.Obj.GetString("post_image"));

                    Projects.Add(new Project { id = o.Obj.GetNumber("id"),
                        name = o.Obj.GetString("wpfm_file_name"),
                        url = o.Obj.GetString("wpfm_file_url"), size = o.Obj.GetString("wpfm_file_size"), description = o.Obj.GetString("wpfm_discription"),
                        photourl = o.Obj.GetString("post_image").Replace("\\", ""),
                        title = o.Obj.GetString("wpfm_title")
                    });
                }
            }
           /* if (Projects.Count == 0) {
                Projects.Add(new Project
                {
                    id = 0,
                    name = "Sample Project",
                    url = "",
                    size = "0 MB",
                    description = "This is Sample Project Proveided By Studio One eleven",
                    photourl = "",
                    title = "Sample Project"
                    ,
                    imgTexture = SampleTexture,
                    LoadedObj=DefaultObject
                });
            }
            */
            if (Projects.Count != 0)
            {
                ProjectTitle.text = Projects[0].title;
                ProjectName.text = "Project: " + Projects[0].name;
                ProjectDescription.text = Projects[0].description;
                ProjectSize.text = "Model size: " + Projects[0].size;
                StartCoroutine(setProjectImage(Projects[0].photourl, ProjectImage, projectIndex));
                LoadingPanal.SetActive(false);
                ProjectsPanal.SetActive(true);
                DashBoardPanel.SetActive(false);
                if (Projects.Count == 1)
                {
                    nextbtn.SetActive(false);
                }
                else {
                    nextbtn.SetActive(true);
                }
                prevbtn.SetActive(false);
                chekDownloadStatus();
            }
            else {
                ProjectTitle.text = "Sample Project";
                ProjectName.text = "Project: Sample Project";
                ProjectImage.texture = SampleTexture;
                ProjectDescription.text = "This is Sample Project Proveided By Studio One eleven";
                ProjectSize.text = "Model size:0 MB";
               // StartCoroutine(setProjectImage(Projects[0].photourl, ProjectImage, projectIndex));
                LoadingPanal.SetActive(false);
                ProjectsPanal.SetActive(true);
                DashBoardPanel.SetActive(false);
                prevbtn.SetActive(false);
                chekDownloadStatus();
                prevbtn.SetActive(false);
                nextbtn.SetActive(false);
                LoadingPanal.SetActive(false);
                Debug.Log("No Projects");
            }
           
            
            // Print Headers
            Debug.Log(responseText);
           

        }

    }

    
    void chekDownloadStatus() {
        if (Projects.Count != 0)
        {
            if (PlayerPrefs.HasKey(Projects[projectIndex].name))
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
        else {
            DownloadBtn.SetActive(false);
            OpenBtn.SetActive(true);

        }

    }

    public Text ProjectDescription;

    public Text ProjectSize;
    public GameObject nextbtn;
    public GameObject prevbtn;
    public void NextProject() {

        if (projectIndex + 1 < Projects.Count)
        {

            projectIndex++;

            if (Projects[projectIndex].imgTexture != null)
            {
                ProjectImage.texture = Projects[projectIndex].imgTexture;
            }
            else if (!Projects[projectIndex].photourl.Equals(""))
            {

                StartCoroutine(setProjectImage(Projects[projectIndex].photourl, ProjectImage,projectIndex));
            }
            else
            {
                ProjectImage.texture = DefaultProjectTexture;
            }
            ProjectTitle.text = Projects[projectIndex].title;
            ProjectName.text = "Project: " + Projects[projectIndex].name;
            ProjectDescription.text = Projects[projectIndex].description;
            ProjectSize.text = "Model size: " + Projects[projectIndex].size ;
            chekDownloadStatus();
            if (projectIndex == Projects.Count - 1)
            {
                nextbtn.SetActive(false);
            }
            else {
                nextbtn.SetActive(true);
            }
            prevbtn.SetActive(true);
        }

    }
    public void PreviousProject()
    {
        if (projectIndex - 1 >=0)
        {

            projectIndex--;
            if (Projects[projectIndex].imgTexture != null)
            {
                ProjectImage.texture = Projects[projectIndex].imgTexture;
            }
            else if (!Projects[projectIndex].photourl.Equals(""))
            {

                StartCoroutine(setProjectImage(Projects[projectIndex].photourl, ProjectImage,projectIndex));
            }
            else
            {
                ProjectImage.texture = DefaultProjectTexture;
            }
            ProjectTitle.text = Projects[projectIndex].title;
            ProjectName.text ="Project: "+ Projects[projectIndex].name;
            ProjectDescription.text = Projects[projectIndex].description;
            ProjectSize.text = "Model size: " + Projects[projectIndex].size;
            chekDownloadStatus();
            if (projectIndex == 0)
            {
               prevbtn.SetActive(false);
            }
            else
            {
                prevbtn.SetActive(true);
            }
            nextbtn.SetActive(true);
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
            //LoadObj();
            LoadingPanal.SetActive(true);
            StartCoroutine(downloadAsset(Projects[projectIndex].url.Replace("\\", "")));
           
        }
        else
        {
            PlaceOnPlane.AssatObj = Projects[projectIndex].LoadedObj;

        }

        chekDownloadStatus();
    }

    IEnumerator downloadAsset(string url)
    {
        
        UnityWebRequest www = UnityWebRequest.Get(url);
        DownloadHandler handle = www.downloadHandler;
       
        //Send Request and wait
        yield return www.Send();

        if (www.isNetworkError)
        {

            UnityEngine.Debug.Log("Error while Downloading Data: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Success");

            //handle.data

            //Construct path to save it
            string dataFileName = Projects[projectIndex].name;
            string tempPath = Path.Combine(Application.persistentDataPath, "AssetData");
            tempPath = Path.Combine(tempPath, dataFileName + ".unity3d");
           
            //Save
            save(handle.data, tempPath);
        }
    }

    void save(byte[] data, string path)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        try
        {
            File.WriteAllBytes(path, data);
            Projects[projectIndex].isloaded = true;
            PlayerPrefs.SetString(Projects[projectIndex].name, path);
            Debug.Log("Saved Data to: " + path.Replace("/", "\\"));
            LoadingPanal.SetActive(false);
            chekDownloadStatus();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    IEnumerator LoadObject(string path,string viewer)
    {
       
        AssetBundleCreateRequest bundle = AssetBundle.LoadFromFileAsync(path);
        yield return bundle;

        AssetBundle myLoadedAssetBundle = bundle.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        var names =myLoadedAssetBundle.GetAllAssetNames();
        //  AssetBundleRequest request = myLoadedAssetBundle.LoadAsset();
        //yield return request;

        AssetBundleRequest objreq = myLoadedAssetBundle.LoadAssetAsync(names[0]);
        yield return objreq;
        //obj.transform.position = new Vector3(0.08f, -2.345f, 297.54f);
        // obj.transform.Rotate(350.41f, 400f, 20f);
        // obj.transform.localScale = new Vector3(1.0518f, 0.998f, 1.1793f);
        if (viewer.Equals("model"))
        {

            LoadedModel = Instantiate((GameObject)objreq.asset, Model.transform);
            LoadedModel.transform.parent = Model.transform;
            LoadingPanal.SetActive(false);
            ProjectsPanal.SetActive(false);
            ModelPanel.SetActive(true);
            ModelView.SetActive(true);
        }
        else {
            if (PlaceOnPlane.AssatObj != null)
            {
                PlaceOnPlane.AssatObj = null;
            }
            PlaceOnPlane.AssatObj = (GameObject)objreq.asset;
            Projects[projectIndex].LoadedObj = (GameObject)objreq.asset;
            Projects[projectIndex].isloaded = true;
            LoadingPanal.SetActive(false);
            // ARCanvas.SetActive(true);
            ArSession.SetActive(true);
            ArModePanel.SetActive(true);
            ProjectsPanal.SetActive(false);
            ArExitpanel.SetActive(true);
        }
        //Instantiate(obj);
        myLoadedAssetBundle.Unload(false);
       
    }
    void LoadInSeen() {
        string path = PlayerPrefs.GetString(Projects[projectIndex].name);
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        var names = bundle.GetAllAssetNames();

        GameObject cube = (GameObject)bundle.LoadAsset(names[0]);
        // spawnedObject = Instantiate(cube);
        Projects[projectIndex].LoadedObj = cube;
        PlaceOnPlane.AssatObj = cube;

        LoadingPanal.SetActive(false);
        // ARCanvas.SetActive(true);
        ArSession.SetActive(true);
        ArModePanel.SetActive(true);
        ProjectsPanal.SetActive(false);
        ArExitpanel.SetActive(true);
        //  LoadObject(path);
    }

    public GameObject ArSession;
    public GameObject ArModePanel;
    public GameObject DefaultObject;
    public Texture2D SampleTexture;
    public void GoToAr()
    {
        ArSession.SetActive(true);
        if (Projects.Count != 0)
        {
            Input.text = Projects[projectIndex].url.Replace("\\", "");
            LoadingPanal.SetActive(true);
            if (Projects[projectIndex].LoadedObj == null)
            {
               // LoadInSeen();
                StartCoroutine( LoadObject(PlayerPrefs.GetString(Projects[projectIndex].name),"ar"));

               
            }
            else
            {
                PlaceOnPlane.AssatObj =Projects[projectIndex].LoadedObj ;
                LoadingPanal.SetActive(false);
                //  ARCanvas.SetActive(true);
                ProjectsPanal.SetActive(false);
                ArSession.SetActive(true);
                ArModePanel.SetActive(true);
                ArExitpanel.SetActive(true);


            }
        }
        else {
            PlaceOnPlane.AssatObj =DefaultObject;
            LoadingPanal.SetActive(false);
            //  ARCanvas.SetActive(true);
            ProjectsPanal.SetActive(false);
            ArSession.SetActive(true);
            ArModePanel.SetActive(true);
            ArExitpanel.SetActive(true);

        }
       


    }
    public GameObject ModelView;
    public GameObject Model;
    public GameObject ModelPanel;
    public GameObject LoadedModel;
    public void GoTo3dModel()
    {
    //    ArSession.SetActive(true);
        if (Projects.Count != 0)
        {
            Input.text = Projects[projectIndex].url.Replace("\\", "");
            LoadingPanal.SetActive(true);
            if (Projects[projectIndex].LoadedObj == null)
            {
                // LoadInSeen();
                StartCoroutine(LoadObject(PlayerPrefs.GetString(Projects[projectIndex].name),"model"));


            }
            else
            {
              
               LoadedModel= Instantiate(Projects[projectIndex].LoadedObj, Model.transform);
                LoadedModel.transform.parent = Model.transform;
                LoadingPanal.SetActive(false);
                ProjectsPanal.SetActive(false);
                ModelPanel.SetActive(true);
                ModelView.SetActive(true);


            }
        }
        else
        {
            
           LoadedModel= Instantiate(DefaultObject, Model.transform);
            LoadedModel.transform.parent = Model.transform;

            LoadingPanal.SetActive(false);
            ProjectsPanal.SetActive(false);
            ModelPanel.SetActive(true);
            ModelView.SetActive(true);

        }



    }

    public void ExitModelView() {
        
        Destroy(LoadedModel);
        Model.transform.position = new Vector3(0, 0, 0);
        Model.transform.localScale = new Vector3(2, 2, 2);
        ModelPanel.SetActive(false);
        ModelView.SetActive(false);
        ProjectsPanal.SetActive(true);
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



   
    public void BackToDashBoard()
    {

        LoginPanal.SetActive(false);
        ProjectsPanal.SetActive(false);

        DashBoardPanel.SetActive(true);
    }
    public GameObject ArExitpanel;
    public void BackToProjects()
    {
        StartCoroutine(waitfor1sec());
        ArSession.SetActive(false);
        ArExitpanel.SetActive(false);
        LoginPanal.SetActive(false);
        ProjectsPanal.SetActive(true);
      //  ARCanvas.SetActive(false);
       // ArSession.SetActive(false);
        ArModePanel.SetActive(false);
        DashBoardPanel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
