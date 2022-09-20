using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScript : MonoBehaviour
{
    [SerializeField] float duration = 10.0f;

    float savedTime = 0;

    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - savedTime >= duration)
        {
            savedTime = Time.time;
        }

        material.SetFloat("_Offset", (Time.time - savedTime) / duration);
    }
}
