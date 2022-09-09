using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Calibration : MonoBehaviour
{
    // Joycon
    private List<Joycon> m_joycons; // 2つのJoyconのステータス
    private Joycon       m_joyconL; // 左のJoycon
    private Joycon       m_joyconR; // 右のJoycon

    // L
    private float[] joydataL; // リアルデータ
    [SerializeField]
    private float[] maxL; // 最大値
    [SerializeField]
    private float[] minL;  // 最小値
    [SerializeField]
    private float[] sumnormalL; // 安静時の合計

    // R
    
    private float[] joydataR; // リアルデータL
    [SerializeField]
    private float[] maxR; // 最大値
    [SerializeField]
    private float[] minR;  // 最小値
    [SerializeField]
    private float[] sumnormalR; // 安静時の合計

    private int normalCount = 0; // 安静時の平均データの分母
    public float relaxjudge = 0.01f; // リラックスのしきい値
    public bool relax = false; // リラックスしてるかどうか
    private float relaxtimer = 0; // リラックスしてる時間
    private int notrelax = 0; //リラックスしていない回数
    private bool calflag = false; // キャリブレーション中かどうか
    public bool calend = false; //キャリブレーション終了
    public bool Relax{ get{ return relax; } } //relaxの取得
    public bool Calend{ get{ return calend; } } //calendの取得

    [SerializeField]
    Image progressBar;   

    public GameObject SMG;
    JoyDis joydis; 

    // Start is called before the first frame update
    void Start()
    {
        joydis = SMG.GetComponent<JoyDis>();
        // Joyconの接続確認
        m_joycons = JoyconManager.Instance.j;
        if ( m_joycons == null || m_joycons.Count <= 0 ) return;
        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );

        

        // Joyconの加速度データの初期化
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(joydis.joyConnect){
            // リアルタイムデータ取得
            var accel_R = m_joyconR.GetAccel();
            var accel_L = m_joyconL.GetAccel();

            if(Input.GetKeyDown(KeyCode.R)) Initialize();

            if(!relax){
                progressBar.fillAmount = relaxtimer / 5.0f;
                // 両肩が安定しているかどうかの判定
                if( Mathf.Abs(joydataR[0] - accel_R.x) < relaxjudge && Mathf.Abs(joydataL[0] - accel_L.x) < relaxjudge ){
                    relaxtimer += Time.deltaTime;
                    // Debug.Log("relaxtimer" + relaxtimer);
                }else{
                    notrelax++;
                }

                if(notrelax > 100){
                    relaxtimer = 0;
                    notrelax = 0;
                }

                if(relaxtimer >= 3){
                    Sumnormal();
                    normalCount++;
                }

                if(relaxtimer >= 5){
                    progressBar.color = new Color(245f / 255f, 101f / 255f, 15f / 255f);
                    relax = true;
                    calflag = true;
                    relaxtimer = 0;
                }
            }

            if(relax){
                // Debug.Log("relax");
                if(calflag){
                    relaxtimer += Time.deltaTime;
                    // if(relaxtimer >= 1.0 && relaxtimer<=3.0){
                    //     if( Mathf.Abs(joydataR[0] - accel_R.x) < relaxjudge && Mathf.Abs(joydataL[0] - accel_L.x) < relaxjudge ){
                    //         // Debug.Log("normal");
                    //         Sumnormal();
                    //         normalCount++;
                    //     }else{
                    //         // Debug.Log("error");
                    //     }
                    // }else 
                    if(relaxtimer > 3.0 && relaxtimer <= 11.0){
                        // Debug.Log("compare");
                        Compare(); // 最大値、最小値更新
                    }else if(relaxtimer > 10.0){
                        Save();
                        calflag = false;
                        calend = true;
                    }
                }
            }
            joydataR[0] = accel_R.x;
            joydataR[1] = accel_R.y;
            joydataR[2] = accel_R.z;
            joydataL[0] = accel_L.x;
            joydataL[1] = accel_L.y;
            joydataL[2] = accel_L.z;
        }
    }

    // 値の初期化
    void Initialize(){
        relax = false;
        relaxtimer = 0;
        calflag = false;
        calend = false;
        progressBar.color = new Color(154f / 255f, 245f / 255f, 15f / 255f);
        joydataL = new float[] {0,0,0};
        maxL = new float[] { -100,-100,-100}; 
        minL = new float[] { 100,100,100}; 
        sumnormalL = new float[] { 0,0,0};
        joydataR = new float[] {0,0,0};
        maxR = new float[] { -100,-100,-100}; 
        minR = new float[] { 100,100,100}; 
        sumnormalR = new float[] { 0,0,0};
        //PlayerPrefs.DeleteAll();
    }

    // 最大値と最小値の比較
    void Compare(){
        for(int i=0; i<3; i++){
            // 右
            if(maxR[i] <= joydataR[i]){
                maxR[i] = joydataR[i];
            }else if(minR[i] >= joydataR[i]){
                minR[i] = joydataR[i];
            }
            // 左
            if(maxL[i] <= joydataL[i]){
                maxL[i] = joydataL[i];
            }else if(minL[i] >= joydataL[i]){
                minL[i] = joydataL[i];
            }
        }
    }


    void Sumnormal(){
        for(int i=0; i<3; i++){
            sumnormalR[i] += joydataR[i];
            sumnormalL[i] += joydataL[i];
        } 
    }

    public void Save(){
        Debug.Log("Save");
        PlayerPrefs.SetFloat("MaxR_x",maxR[0]);
        PlayerPrefs.SetFloat("MaxR_y",maxR[1]);
        PlayerPrefs.SetFloat("MaxR_z",maxR[2]);
        PlayerPrefs.SetFloat("MinR_x",minR[0]);
        PlayerPrefs.SetFloat("MinR_y",minR[1]);
        PlayerPrefs.SetFloat("MinR_z",minR[2]);
        PlayerPrefs.SetFloat("MaxL_x",maxL[0]);
        PlayerPrefs.SetFloat("MaxL_y",maxL[1]);
        PlayerPrefs.SetFloat("MaxL_z",maxL[2]);
        PlayerPrefs.SetFloat("MinL_x",minL[0]);
        PlayerPrefs.SetFloat("MinL_y",minL[1]);
        PlayerPrefs.SetFloat("MinL_z",minL[2]);
        PlayerPrefs.SetFloat("avenormalR_x", sumnormalR[0] / normalCount);
        PlayerPrefs.SetFloat("avenormalR_y", sumnormalR[1] / normalCount);
        PlayerPrefs.SetFloat("avenormalR_z", sumnormalR[2] / normalCount);
        PlayerPrefs.SetFloat("avenormalL_x", sumnormalL[0] / normalCount);
        PlayerPrefs.SetFloat("avenormalL_y", sumnormalL[1] / normalCount);
        PlayerPrefs.SetFloat("avenormalL_z", sumnormalL[2] / normalCount);
        PlayerPrefs.Save();
    }
}
