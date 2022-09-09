using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgMove : MonoBehaviour
{
    public float maxPos = -100;
    public float minPos = -112;
    public float movespeed = 6.0f;
    bool max = false;
    bool min = false;
    int num = 1;

    private Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.localPosition.y);

        pos = transform.localPosition;

        transform.Translate(new Vector3(0,movespeed*num,0) * Time.deltaTime);

        if(pos.y > maxPos)
        {
            num = -1;
        }
        if(pos.y < minPos)
        {
            num = 1;
        }
    }
}
