using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyconD : MonoBehaviour
{

    public List<Joycon>    m_joycons;
    // public Joycon          m_joyconL;
    // public Joycon          m_joyconR;
    // public Joycon.Button?  m_pressedButtonL;
    // public Joycon.Button?  m_pressedButtonR;

    public Image controlerL;
    public Image controlerR;
    public GameObject disconnectL;
    public GameObject disconnectR;
    public bool joyConnect;


    // Start is called before the first frame update
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        // Debug.Log(m_joycons.Count);
        // controlerL.GetComponent<Image>().color = new Color(49f / 255f, 49f / 255f, 49f / 255f);

        if ( m_joycons == null || m_joycons.Count == 0){
            // Debug.Log("S");
            controlerL.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
            controlerR.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
            disconnectL.SetActive(true);
            disconnectR.SetActive(true);
            joyConnect = false;
            return;
        }else if(m_joycons.Count == 1 ){
            joyConnect = false;
            if(m_joycons[0].isLeft){
                // Debug.Log("B");
                controlerR.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
                disconnectR.SetActive(true);
                return;
            }else{
                // Debug.Log("A");
                controlerL.color = new Color(49f / 255f, 49f / 255f, 49f / 255f);
                disconnectL.SetActive(true);
                return;
            }
        }else{
            joyConnect = true;
        }

        // m_joyconL = m_joycons.Find( c =>  c.isLeft );
        // m_joyconR = m_joycons.Find( c => !c.isLeft );
    }
}
