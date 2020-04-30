using Dummiesman;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class ObjFromStream : MonoBehaviour {

    public InputField Input;
   
    public GameObject LoadingPanal;

    public void LoadObj() {
        if (PlaceOnPlane.AssatObj != null) {
            //PlaceOnPlane.AssatObj.active = false;
            PlaceOnPlane.AssatObj = null;
        }
        LoadingPanal.SetActive(true);
        WWW www2 = new WWW(Input.text);
        StartCoroutine(WaitForReq(www2));
    }
    void Start () {
        Input.text = "https://github.com/Aatish13/3DObjects/blob/master/anchor?raw=true";
        LoadingPanal.SetActive(false);
    }

    IEnumerator WaitForReq(WWW www)
    {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            var names= bundle.GetAllAssetNames();

            GameObject cube = (GameObject)bundle.LoadAsset(names[0]);
             // spawnedObject = Instantiate(cube);
            PlaceOnPlane.AssatObj = cube;
           
            LoadingPanal.SetActive(false);
        }
        else
        {
            LoadingPanal.SetActive(false);
            Debug.Log(www.error);
        }
    }

}
