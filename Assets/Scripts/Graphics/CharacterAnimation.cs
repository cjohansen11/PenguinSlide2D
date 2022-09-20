using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Material material;

    [SerializeField] Texture idle;
    [SerializeField] Texture flyingUp;
    [SerializeField] Texture flyingDown;

    [SerializeField] float switchTime = 0.1f;

    bool up = true;
    float savedTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(bool flying)
    {
        if (flying)
        {
            if (Time.time - savedTime >= switchTime)
            {
                if (up)
                {
                    material.mainTexture = flyingUp;
                    up = false;
                }
                else
                {
                    material.mainTexture = flyingDown;
                    up = true;
                }

                savedTime = Time.time;
            }
        }
        else
        {
            material.mainTexture = idle;
        }
    }
}
