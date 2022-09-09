using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PvScene : MonoBehaviour
{
    public VideoPlayer pv;
    // Start is called before the first frame update
    void Start()
    {
        pv.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.LeftControl)){
            FadeManager.Instance.LoadScene("StartScene", 0.3f);
        }
    }
}
