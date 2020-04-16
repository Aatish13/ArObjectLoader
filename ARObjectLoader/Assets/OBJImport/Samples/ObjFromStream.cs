using Dummiesman;
using System.IO;
using System.Text;
using UnityEngine;

public class ObjFromStream : MonoBehaviour {
	void Start () {
        //make www
        var www = new WWW("https://raw.githubusercontent.com/Aatish13/AugmentedBook/master/AugmentedBook/table.obj");
        while (!www.isDone)
            System.Threading.Thread.Sleep(1);
        
        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        var loadedObj = new OBJLoader().Load(textStream);
        loadedObj.transform.position = new Vector3(0, 0, 15);
        loadedObj.transform.localRotation = Quaternion.Euler(0f,50f,0f);
        loadedObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
