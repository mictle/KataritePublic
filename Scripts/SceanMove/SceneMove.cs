using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];

    private List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;

    public bool StartScene;
    private int userNumber;

    private void Start()
    {
        // m_joycons = JoyconManager.Instance.j;

        // if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        // m_joyconL = m_joycons.Find( c =>  c.isLeft );
        // m_joyconR = m_joycons.Find( c => !c.isLeft );
        userNumber =  PlayerPrefs.GetInt("guestNum",1);
    }

    private void Update()
    {
        // m_pressedButtonL = null;
        // m_pressedButtonR = null;

        // if ( m_joycons == null || m_joycons.Count <= 1 ) return;

        // foreach ( var button in m_buttons )
        // {
        //     if ( m_joyconL.GetButton( button ) )
        //     {
        //         m_pressedButtonL = button;
        //     }
        //     if ( m_joyconR.GetButton( button ) )
        //     {
        //         m_pressedButtonR = button;
        //     }
        // }


        if(StartScene){
            if(Input.GetKeyDown(KeyCode.RightControl)){
                // SceneManager.LoadScene("GameScene");
                FadeManager.Instance.LoadScene("GameScene", 0.3f);
            }
        }else{
            if(/*m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN) || */Input.GetKeyDown(KeyCode.RightControl)){
                // PlayerPrefs.SetInt("guestNum", userNumber - 1);
                // SceneManager.LoadScene("StartScene");
                FadeManager.Instance.LoadScene("StartScene", 0.3f);
            }
            if(Input.GetKeyDown(KeyCode.RightShift)){
                FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 0.3f);
            }
        }
    }
}
