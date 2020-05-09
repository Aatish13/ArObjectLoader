using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodingCircle : MonoBehaviour
{
    public GameObject icon;
    float rotateSpeed = -200f;

    private void Start()
    {
        
    }

    private void Update()
    {
        icon.transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
