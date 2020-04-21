using Dummiesman;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine.UI;
using System;

public class ObjFromFile : MonoBehaviour
{
    string objPath = @"E:\project database\StudioOneEleven\Objects\obj\light\Livingroom\livingMTR.obj";
    string error = string.Empty;
    GameObject loadedObject;
    public Text snakBar;
    public Text ErrorBar;
    public void OnBTNClick() {
       
        if (loadedObject != null)
            Destroy(loadedObject);
        snakBar.text = objPath;
        loadedObject = new OBJLoader().Load(objPath);
        error = string.Empty;
       
        if (!string.IsNullOrWhiteSpace(error))
        {
            ErrorBar.text = error;
        }
    }
    public void OnSelectBTNClick() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Object", ".obj"));
        FileBrowser.SetDefaultFilter(".obj");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        // FileBrowser.ShowSaveDialog(null, null, false, "C:\\", "Save As", "Sav");
        StartCoroutine(ShowLoadDialogCoroutine());

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
            
            objPath = FileBrowser.Result;
            snakBar.text = objPath;
           // byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
           // string bitString = BitConverter.ToString(bytes);
          //  ErrorBar.text = bitString;

        }
    }
}
