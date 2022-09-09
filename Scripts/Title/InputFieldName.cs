using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputFieldName : MonoBehaviour
{

    private InputField name;
    private int userNumber;
    bool test;

    // Start is called before the first frame update
    void Start()
    {
        name = this.gameObject.GetComponent<InputField>();
        name.ActivateInputField();
        name.onEndEdit.AddListener(
            delegate{
                name.ActivateInputField();
            }
        );
        userNumber =  PlayerPrefs.GetInt("guestNum",1);
        name.text  = "Guest" + userNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightAlt)){
            test = true;
            name.text  = "TEST";
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            name.text  = "Guest" + userNumber;
            test = false;
        }
    }

    // void OnDestroy() {
    //     PlayerPrefs.SetString("UserName",name.text);
    //     if(!test){
    //         PlayerPrefs.SetInt("guestNum", userNumber + 1);
    //     }
    //     PlayerPrefs.Save();
        
    // }


                // if(Input.GetKeyDown(KeyCode.RightArrow)){
            //     ok = false;
            // }
            // if(Input.GetKeyDown(KeyCode.LeftArrow)){
            //     ok = true;
            // }
            // if(ok){
            //     kettei.color = new Color(255f/255f, 0f/255f, 0f/255f);
            //     cancel.color = new Color(50f/255f, 50f/255f, 50f/255f); 
            // }else{
            //     kettei.color = new Color(50f/255f, 50f/255f, 50f/255f);
            //     cancel.color = new Color(255f/255f, 0f/255f, 0f/255f);
            // }

            // if(Input.GetKeyDown(KeyCode.Return)){
            //     enter++;
            //     if(ok && enter >= 2){
            //         Debug.Log("ok true");
            //         scene1.SetActive(false);
            //         namecheck.SetActive(false);
            //         scene2.SetActive(true);
            //         scene = 2;
            //         enter = 0;
            //         PlayerPrefs.SetString("UserName",name.text);
            //         if(!test){
            //             PlayerPrefs.SetInt("guestNum", userNumber + 1);
            //         }
            //         PlayerPrefs.Save(); 
            //         Debug.Log(PlayerPrefs.GetString("UserName","0"));
            //     }else if(!ok){
            //         Debug.Log("ok false");
            //         imgmove.movespeed = 70;
            //         namecheck.SetActive(false);
            //         scene = 1;
            //         ok = true;
            //         enter = 0;
            //         name.ActivateInputField(); 
            //     }       
            // }
}
