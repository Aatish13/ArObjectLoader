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

    public GameObject ARCanvas;
    public GameObject LoginCanvas;

    public GameObject HomePanel;
    public GameObject LoginPanal;

    public GameObject DashBoardPanel;
    public GameObject ProjectsPanal;

    public GameObject LoadingPanal;

    public Text UserName;
    public RawImage ProfileImage;
    public Text ErrorText;

    public GameObject DownloadBtn;
    public GameObject OpenBtn;
    // Start is called before the first frame update
    void Start()
    {
        FileBrowser.CheckPermission();
        if (PlayerPrefs.HasKey("username"))
        {
            StartCoroutine(setImage(PlayerPrefs.GetString("profileimg")));
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
    public void LogOut() {
        PlayerPrefs.DeleteAll();
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
                PlayerPrefs.SetString("username",username);
                PlayerPrefs.SetString("displayname", json.GetObject("user").GetString("displayname"));
                Debug.Log("Profile image link :::" + json.GetObject("user").GetString("avatar").Replace("\\", ""));
                PlayerPrefs.SetString("profileimg", json.GetObject("user").GetString("avatar").Replace("\\", ""));
                StartCoroutine(setImage(PlayerPrefs.GetString("profileimg")));

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
    IEnumerator setImage(string url)
    {
      //  Texture2D texture = ProfileImage.canvasRenderer.GetMaterial().mainTexture as Texture2D;

        WWW www = new WWW(url);
        yield return www;

        // calling this function with StartCoroutine solves the problem
        Debug.Log("Why on earh is this never called?");
        ProfileImage.color = Color.white;
        ProfileImage.texture = www.texture;
      // www.LoadImageIntoTexture((Texture2D)ProfileImage.texture);
        www.Dispose();
        www = null;
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
            chekDownloadStatus();
            // Print Headers
            Debug.Log(responseText);
           

        }

    }

    
    void chekDownloadStatus() {

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

    IEnumerable LoadObject(string path)
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

        GameObject obj = (GameObject)myLoadedAssetBundle.LoadAsset(names[0]);
        //obj.transform.position = new Vector3(0.08f, -2.345f, 297.54f);
        // obj.transform.Rotate(350.41f, 400f, 20f);
        // obj.transform.localScale = new Vector3(1.0518f, 0.998f, 1.1793f);

        //Instantiate(obj);
        if (PlaceOnPlane.AssatObj != null) {
            PlaceOnPlane.AssatObj = null;
        }
        PlaceOnPlane.AssatObj = obj;
         Projects[projectIndex].LoadedObj=obj;
        ARCanvas.SetActive(true);
        ProjectsPanal.SetActive(false);
        LoadingPanal.SetActive(false);
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
        ARCanvas.SetActive(true);
        ProjectsPanal.SetActive(false);
        //  LoadObject(path);
    }
    public void GoToAr()
    {

        Input.text = Projects[projectIndex].url.Replace("\\", "");
        LoadingPanal.SetActive(true);
        if (Projects[projectIndex].LoadedObj==null)
        {
            LoadInSeen();
            // LoadObj();
            
            Projects[projectIndex].isloaded = true;
        }
        else {
            PlaceOnPlane.AssatObj = Projects[projectIndex].LoadedObj;
            LoadingPanal.SetActive(false);
            ARCanvas.SetActive(true);
            ProjectsPanal.SetActive(false);
        }
        
      
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
