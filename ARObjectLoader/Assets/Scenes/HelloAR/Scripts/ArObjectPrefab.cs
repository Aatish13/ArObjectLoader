using System.Collections;
using System.Collections.Generic;
using Dummiesman;
using System.IO;
using UnityEngine;
using System.Text;

public class ArObjectPrefab : MonoBehaviour
{
    string objPath = @"file:///storage/emulated/0/GameObjects/o1.obj";
    string error = string.Empty;
    public GameObject loadedObject;
    public GameObject CenterPoint;
    // Start is called before the first frame update
    void Start()
    {

        var www = new WWW("https://raw.githubusercontent.com/Aatish13/AugmentedBook/master/AugmentedBook/table.obj");
        while (!www.isDone)
            System.Threading.Thread.Sleep(1);

        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        loadedObject =new OBJLoader().Load(textStream);// GameObject.CreatePrimitive(PrimitiveType.Cube);//
        loadedObject.transform.position = CenterPoint.transform.position;

        loadedObject.transform.localRotation = Quaternion.Euler(0f, 50f, 0f);
        loadedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        loadedObject.SetActive(true);
    }
}
