using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackGroundHeightColor : MonoBehaviour
{
    Material material;

    [SerializeField] Vector4 lowColor = Vector4.zero;
    [SerializeField] Vector4 highColor = Vector4.zero;

    [SerializeField] float maxHeight = 5;

    [SerializeField] FunctionReading functionReading;

    // Start is called before the first frame update
    void Start()
    {
        material = material = GetComponent<Renderer>().material;

        transform.localScale = new Vector3(functionReading.nbCubeToGenerate, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("color", Vector4.Lerp(lowColor, highColor, transform.position.y / maxHeight));
    }
}
