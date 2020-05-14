using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectcustomizer : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject SelectedObject=null;

    IEnumerator SelectObject(RaycastHit hitt) {
        Outline outl;
        if (SelectedObject != null)
        {
            outl = SelectedObject.GetComponent<Outline>();
            Destroy(outl);

        }
        else {
            outl = null;
        }
      
        string id = hitt.transform.gameObject.name;

        var outline = hitt.transform.gameObject.AddComponent<Outline>();
        SelectedObject = hitt.transform.gameObject;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 10f;
        yield return new WaitUntil(()=> { if (outl != null) return true; else return false; });
    }
    // Update is called once per frame
    void Update()
    {

     if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitt;
            if (Physics.Raycast(ray, out hitt))
            {

                StartCoroutine(SelectObject(hitt));

            }
        }
    }
}
