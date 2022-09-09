using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SelectMode : MonoBehaviour
{
    private int scene;
    public GameObject keyguid;
    JoyconD joyD;
    
    //Scene1
    public GameObject scene1;
    public InputField name;
    private int userNumber;
    bool test;
    public GameObject discon;
    public GameObject namecheck;
    public Text username;
    public GameObject movetext;
    ImgMove imgmove;
    public Text kettei;
    public Text cancel;
    private int enter;
    private bool ok;
    private float poseTime;
    private bool poseFlag;
    public float changeTime;

    //Scene2
    public GameObject scene2;
    public Image Drive;
    public RawImage Drivemv;
    public VideoPlayer drive;
    public Image Ski;
    public RawImage Skimv;    
    public VideoPlayer ski;
    private bool shoulderDrive;
    
    
    //Scene3
    public GameObject scene3;
    private int level;
    public GameObject Drive_level;
    public Image Drive_easy;
    public Image Drive_normal;
    public Image Drive_hard;
    public GameObject Ski_mode;
    public Image Ski_slalom;
    public Image Ski_aerial;
    public Image Ski_free;


    // Start is called before the first frame update
    void Start()
    {
        scene = 1;
        enter = 0;
        level = 0;
        ok = true;
        shoulderDrive = true;
        poseTime = 0;
        name.ActivateInputField();
        // name.onEndEdit.AddListener(
        //     delegate{
        //         name.ActivateInputField();
        //     }
        // );
        userNumber =  PlayerPrefs.GetInt("guestNum",1);
        name.text  = "Guest" + userNumber;
        imgmove = movetext.GetComponent<ImgMove>();
        joyD = GetComponent<JoyconD>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("en" + poseTime);
        if(Mathf.Abs(scene) == 1){
            keyguid.SetActive(false);
        }else{
            keyguid.SetActive(true);
        }

        
        if(scene == 1){
            if(Input.anyKeyDown){
                poseTime = 0;
                poseFlag = false;
            }else{
                poseTime += Time.deltaTime;
            }
            if(poseTime >= changeTime && !poseFlag){
                poseFlag = true;
            }
            if(poseFlag){
                FadeManager.Instance.LoadScene("PV", 0.3f);
            }else{
                if(Input.GetKeyDown(KeyCode.RightAlt)){
                    test = true;
                    name.text  = "TEST";
                }
                if(Input.GetKeyDown(KeyCode.LeftAlt)){
                    name.text  = "Guest" + userNumber;
                    test = false;
                }
                if(Input.GetKeyDown(KeyCode.Return)){
                    if(joyD.joyConnect){
                        imgmove.movespeed = 0;
                        namecheck.SetActive(true);
                        username.text = name.text;
                        scene = -1;  
                    }else{
                        discon.SetActive(true);
                        if(Input.GetKeyDown(KeyCode.Return)){
                            enter++;
                            if(enter >= 2){
                                Debug.Log("RS");
                                FadeManager.Instance.LoadScene("StartScene", 0.3f);
                            }
                        }
                    }  
                }
            }
            
        }


        if(scene == -1){
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    Debug.Log("ok true");
                    scene1.SetActive(false);
                    namecheck.SetActive(false);
                    scene2.SetActive(true);
                    scene = 2;
                    enter = 0;
                    PlayerPrefs.SetString("UserName",name.text);
                    if(!test){
                        PlayerPrefs.SetInt("guestNum", userNumber + 1);
                    }
                    PlayerPrefs.Save(); 
                    Debug.Log(PlayerPrefs.GetString("UserName","0")); 
                }     
            }

            if(Input.GetKeyDown(KeyCode.Backspace)){
                Debug.Log("ok false");
                imgmove.movespeed = 70;
                namecheck.SetActive(false);
                scene = 1;
                ok = true;
                enter = 0;
                name.ActivateInputField(); 
            }
        }


        if(scene == 2){
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                shoulderDrive = false;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                shoulderDrive = true;
            }

            if(shoulderDrive){
                Drive.color = new Color(81f/255f, 255f/255f, 239f/255f);
                Drivemv.color = new Color(255f/255f, 255f/255f, 255f/255f);
                Ski.color = new Color(50f/255f, 50f/255f, 50f/255f); 
                Skimv.color = new Color(100f/255f, 100f/255f, 100f/255f);
                drive.Play();
                ski.Stop();
            }else{
                Drive.color = new Color(40f/255f, 150f/255f, 150f/255f);
                Drivemv.color = new Color(100f/255f, 100f/255f, 100f/255f);
                Ski.color = new Color(255f/255f, 255f/255f, 255f/255f); 
                Skimv.color = new Color(255f/255f, 255f/255f, 255f/255f);
                drive.Stop();
                ski.Play();
            }
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    scene = 3;
                    enter = 0;
                    scene2.SetActive(false);
                    scene3.SetActive(true);
                    if(shoulderDrive){
                        Drive_level.SetActive(true);
                    }else{
                        Ski_mode.SetActive(true);
                    } 
                } 
            }
            if(Input.GetKeyDown(KeyCode.Backspace)){
                scene = 1;
                ok = true;
                shoulderDrive = true;
                enter = 0;
                scene2.SetActive(false);
                scene1.SetActive(true);
                name.ActivateInputField(); 
            }
        }


        if(scene == 3){
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                if(level < 2){
                    level++;
                } 
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                if(level > 0){
                    level--;
                } 
            }

            if(level == 0){
                if(shoulderDrive){
                    Drive_easy.color = new Color(100f/255f, 255f/255f, 255f/255f);
                    Drive_normal.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Drive_hard.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }else{
                    Ski_slalom.color = new Color(100f/255f, 255f/255f, 255f/255f);
                    Ski_aerial.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Ski_free.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }
                
            }
            if(level == 1){
                if(shoulderDrive){
                    Drive_easy.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Drive_normal.color = new Color(255f/255f, 255f/255f, 100f/255f);
                    Drive_hard.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }else{
                    Ski_slalom.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Ski_aerial.color = new Color(255f/255f, 255f/255f, 100f/255f);
                    Ski_free.color = new Color(130f/255f, 60f/255f, 60f/255f);
                }
                
            }
            if(level == 2){
                if(shoulderDrive){
                    Drive_easy.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Drive_normal.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Drive_hard.color = new Color(255f/255f, 130f/255f, 130f/255f);
                }else{
                    Ski_slalom.color = new Color(50f/255f, 100f/255f, 100f/255f);
                    Ski_aerial.color = new Color(100f/255f, 100f/255f, 50f/255f);
                    Ski_free.color = new Color(255f/255f, 130f/255f, 130f/255f);
                }
            }
            if(Input.GetKeyDown(KeyCode.Return)){
                enter++;
                if(enter >= 2){
                    if(shoulderDrive){
                        PlayerPrefs.SetInt("Drive_level", level);
                        PlayerPrefs.Save();
                        FadeManager.Instance.LoadScene("GameScene", 0.3f);
                    }else{
                        if(level == 0){
                            FadeManager.Instance.LoadScene("Ski_Slalom", 0.3f);
                        }else if(level == 1){
                            FadeManager.Instance.LoadScene("Ski_Aerial", 0.3f);
                        }else{
                            FadeManager.Instance.LoadScene("Ski_Free", 0.3f);
                        }
                    }
                } 
            }
            if(Input.GetKeyDown(KeyCode.Backspace)){
                scene = 2;
                level = 0;
                enter = 1;
                scene3.SetActive(false);
                Drive_level.SetActive(false);
                Ski_mode.SetActive(false);
                scene2.SetActive(true);
            }
        }

    }
}
