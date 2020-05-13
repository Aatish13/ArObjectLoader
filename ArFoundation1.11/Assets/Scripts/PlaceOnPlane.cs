using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using SimpleFileBrowser;
using UnityEngine.UI;
using System;
using System.IO;
/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager), typeof(ARAnchorManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    public Text LogText;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; set; }

    public static GameObject AssatObj=null;

    public Text SnakBar;
    public string path;
    string url = "https://github.com/Aatish13/3DObjects/blob/master/anchor?raw=true";

    public InputField Url;

    public Slider YRotation;
    bool MoveXZFlag = false;
    public void MoveXZ()
    {
        MoveXZFlag = true;
        VisualizePlanes(true);
        VisualizePoints(true);

    }
    public void CloseMoveXZ()
    {
        MoveXZFlag = false;
        VisualizePlanes(false);
        VisualizePoints(false);

    }
    
    public void MoveUp() {
        var pos = spawnedObject.transform.position;
        pos.y += 0.2f;
        spawnedObject.transform.position = pos;
    }
    public void MoveDown()
    {
        var pos = spawnedObject.transform.position;
        pos.y -= 0.2f;
        spawnedObject.transform.position = pos;
    }
    public Text sizeLable;
    float[] size = { 1f,0.5f, 0.2f, 0.1f, 0.05f, 0.02f ,0.01f,0.005f};
    string[] lable = { "1:1", "1:2", "1:5", "1:10", "1:20", "1:50","1:100","1:200" };
    int index = 0;
    public void SizeUp()
    {
        if (spawnedObject != null)
        {
            if (index > 0)
            {
                index--;
            }
            Vector3 dif = new Vector3(size[index], size[index], size[index]);
            spawnedObject.transform.localScale = dif;
            sizeLable.text = lable[index];
        }
   }
    public void SizeDown()
    {
        if (spawnedObject != null)
        {
            if (index < size.Length - 1)
            {
                index++;
            }
            Vector3 dif = new Vector3(size[index], size[index], size[index]);
            spawnedObject.transform.localScale = dif;
            sizeLable.text = lable[index];

        }
        
    }
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }
    int draw = -1;
    public void changeDrowStatus() {
        draw *= -1;
    }
    
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {


        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;

           /* if (spawnedObject != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitt;
                if (Physics.Raycast(ray, out hitt))
                {
                    var pos = ARcamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.2f));
                    // var x = anchorManager.AddAnchor(new Pose(pos, ARcamera.transform.rotation));
                    //var tes = Instantiate(Note,hitt.transform.gameObject.transform);
                   string id= hitt.transform.gameObject.name;
                  //  hitt.transform.gameObject.GetComponent<MeshRenderer>().material.mainTexture = Texture2D.whiteTexture;
                    LogText.text = id.ToString();
                    //tes.transform.parent = hitt.transform.gameObject.transform;
                    var outline = hitt.transform.gameObject.AddComponent<Outline>();

                    outline.OutlineMode = Outline.Mode.OutlineAll;
                    outline.OutlineColor = Color.yellow;
                    outline.OutlineWidth = 5f;

                }
            }
      
            if (draw==1){
                var pos = ARcamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0.2f));
                var x = anchorManager.AddAnchor(new Pose(pos, ARcamera.transform.rotation));
            }
       */
            return true;
            
        }


        touchPosition = default;
        return false;
    }
    public GameObject Note;
    public Camera ARcamera;
    public List<GameObject> line=new List<GameObject>();
    public GameObject Pen;
    ARAnchorManager anchorManager; 
    void Update()
    {
       
        //  url = Url.textComponent.text;
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
    
        if (spawnedObject != null) {
            spawnedObject.transform.rotation = Quaternion.Euler(new Vector3(0,YRotation.value+90,0));
            
        }
        sizeLable.text = lable[index];


        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                     //var loadOb = SnapLoader.SnapLoadOBJ(@"file:\\storage\emulated\0\GameObject\o1.obj", hitPose.position);
                    if (AssatObj != null)
                    {
                        spawnedObject = Instantiate(AssatObj, hitPose.position, hitPose.rotation);
                        
                        VisualizePlanes(false);
                        VisualizePoints(false);
                    }
                  // else {
                    //  spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    //}

                }
                else if(MoveXZFlag)
                {
                    spawnedObject.transform.position = hitPose.position;
                }
            }
      
    }
    public void LoadObj() {
     
            SnakBar.text = "Loading...............";
        WWW www = new WWW(Url.text);
        StartCoroutine(WaitForReq(www));
    }

    GameObject loadedGameObject;
    IEnumerator WaitForReq(WWW www) {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == "")
        {
            var names = bundle.GetAllAssetNames();
            loadedGameObject = (GameObject)bundle.LoadAsset(names[0]);
             var obj = Instantiate(loadedGameObject);
            SnakBar.text = "Loaded";

        }
        else {
            Debug.Log(www.error);
        }
    }

    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;
    void VisualizePlanes(bool active)
    {
        planeManager.enabled = active;
        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }

    void VisualizePoints(bool active)
    {
        pointCloudManager.enabled = active;
        foreach (ARPointCloud pointCLoud in pointCloudManager.trackables)
        {
            pointCLoud.gameObject.SetActive(active);
        }
    }

    public void reset()
    {
        index = 0;
        YRotation.value = 360;
       // spawnedObject.SetActive(false);
        Destroy(spawnedObject);
        VisualizePlanes(true);
        VisualizePoints(true);
       // spawnedObject = null;

    }
   


   
    void Start()
    {

        Url.text = url;

    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {
            // If a file was chosen, read its bytes via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            SnakBar.text = FileBrowser.Result;
            path = FileBrowser.Result;
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
           
        }
    }


    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
