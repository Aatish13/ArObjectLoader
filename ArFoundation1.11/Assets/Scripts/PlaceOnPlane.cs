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
[RequireComponent(typeof(ARRaycastManager))]
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

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    public Text SnakBar;
    public string path;
    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
          
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                var loadOb = SnapLoader.SnapLoadOBJ(@"file:\\storage\emulated\o\GameObject\o1.obj", hitPose.position);
          
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    void Start()
    {

   
        // Set filters (optional)
        // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
        // if all the dialogs will be using the same filters
      //   FileBrowser.SetFilters(true, new FileBrowser.Filter("Object", ".obj"));

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        // FileBrowser.SetDefaultFilter(".obj");

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        //FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Name: Users
        // Path: C:\Users
        // Icon: default (folder icon)
      //  FileBrowser.AddQuickLink("Users", @"file:\\storage\emulated\o", null);

        // Show a save file dialog 
        // onSuccess event: not registered (which means this dialog is pretty useless)
        // onCancel event: not registered
        // Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
        // FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
         //FileBrowser.ShowLoadDialog( (pat) => { Debug.Log( "Selected: " + pat ); }, 
             //                          () => { Debug.Log( "Canceled" ); }, 
           //                             true, null, "Select File", "Select" );

        // Coroutine example
          //StartCoroutine(ShowLoadDialogCoroutine());

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
    /* void Start()
    {
        string url = "your url";
        StartCoroutine(loadWWWCaching(url));
    }
    IEnumerator loadWWWCaching(string url)
    {
        while (!Caching.ready)
            yield return null;
        using (WWW www = WWW.LoadFromCacheOrDownload(url, 1))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                AssetBundle assetBundle = www.assetBundle;
            }
            else
            {
                Debug.Log("WWW error: " + www.error);
            }
        }
    }*/

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
