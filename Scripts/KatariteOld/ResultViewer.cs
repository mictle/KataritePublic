using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultViewer : MonoBehaviour
{
    int myTime;
    int penartyCount;
    int myScore;
    public string myName = "guest";
    private int countNum;

    private int myRank;

    public Text myTimeTex;
    public Text myPenartyTex;
    public Text myScoreTex;

    //ランキング内のランク外欄順位
    public Text[] myRankTex;

    private string rankingCSVPath;
    private List<string> rankingNames = new List<string>();
    private List<int> rankingScores = new List<int>();
    public bool setMyScore;

    public List<Text> rankNameTexts = new List<Text>();
    public List<Text> rankScoreTexts = new List<Text>();

    //ランキング赤ラベル
    public List<Image> rankRedLabel = new List<Image>();

    //ランクtext
    public List<Text> rankAlphabetText= new List<Text>();
    public Text rankAlphabetShadow;

    //ランクボーダー
    [SerializeField] private int SSborder;
    [SerializeField] private int Sborder;
    [SerializeField] private int Aborder;
    [SerializeField] private int Bborder;

    //表示するランクワードテキスト
    private Text nowRankText;

    public Text timeLimitText;

    private float time = 1000;
    public float timeLim = 5;
    //強制的に最初に戻る時間
    public float endTime;
    //時間保存
    [SerializeField] private float endTimeCounter = -1;
    
    //数字変化の時間
    [SerializeField] private float showTime;

    //リザルト開始からリザルトが動き出すまでの時間
    [SerializeField] private float UIFirstBreak;




    void Start(){
        
    }
    // Start is called before the first frame update
    public void ShowResult()
    {
        rankingCSVPath = Application.dataPath + "/ranking.csv";
        setMyScore = false;
        countNum = 0;
        myTime = (int)PlayerPrefs.GetFloat("time", 0);
        penartyCount = PlayerPrefs.GetInt("boomNum", 0);
        myScore = 400 + (360 - myTime) - 20 * penartyCount;
        if(myTime%60<10) myTimeTex.text = (myTime / 60) + " : 0" + (myTime % 60);
        else myTimeTex.text = (myTime / 60) + " : " + (myTime % 60);
        myPenartyTex.text = penartyCount +"回";
        myScoreTex.text = myScore + "";

        //名前取得
        myName = PlayerPrefs.GetString("UserName", "Guest");

        //ランク設定

        if(myScore >= SSborder){
            nowRankText = rankAlphabetText[0];
            rankAlphabetShadow.text = "SS";
        }else if(myScore >= Sborder){
            nowRankText = rankAlphabetText[1];
            rankAlphabetShadow.text = "S";
        }else if(myScore >= Aborder){
            nowRankText = rankAlphabetText[2];
            rankAlphabetShadow.text = "A";
        }else if(myScore >= Bborder){
            nowRankText = rankAlphabetText[3];
            rankAlphabetShadow.text = "B";
        }else{
            nowRankText = rankAlphabetText[4];
            rankAlphabetShadow.text = "C";
        }
        //rankAlphabetShadow.enabled = false;

        //ランキング
        StreamReader sr = new StreamReader(rankingCSVPath);
        while(sr.Peek() != -1){
            string[] st = sr.ReadLine().Split(',');
            int score = int.Parse(st[1]);
            if(!setMyScore && myScore > score){
                setMyScore = true;
                rankingNames.Add(myName);
                rankingScores.Add(myScore);
                myRank = countNum + 1;
                if(countNum<5)rankRedLabel[countNum].enabled = true;
            }
            rankingNames.Add(st[0]);
            rankingScores.Add(score);
            countNum++;
        }

        //ランキングの最後まで自分がいなかったら最後に追加
        if(!setMyScore){
            rankingNames.Add(myName);
            rankingScores.Add(myScore);
            if(countNum<5)rankRedLabel[countNum].enabled = true;
            myRank = countNum + 1;
        }

        //リストからランキングをUIに登録
        for(int i=0; i<5; i++){
            if(rankingNames.Count > i){
                rankNameTexts[i].text = rankingNames[i];
                rankScoreTexts[i].text = rankingScores[i] + "";
            }else{
                rankNameTexts[i].text = "None";
                rankScoreTexts[i].text = "0";
            }

        }

        rankNameTexts[5].text = myName + "";
        rankScoreTexts[5].text = myScore + "";
        myRankTex[0].text = myRank + ".";
        myRankTex[1].text = myRank + ".";


        sr.Close();
        
        endTimeCounter = 0;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(endTimeCounter >= 0){
            endTimeCounter += Time.deltaTime;
            timeLimitText.text = "あと"+((int)(endTime - endTimeCounter)) + "秒でタイトルに戻ります";

            nowRankText.enabled = true;
            rankAlphabetShadow.enabled = true;

            //終了時処理
            if(endTimeCounter>= endTime){
                EndResult();
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void EndResult(){
        if(myName == "TEST")return;
        //CSVファイルに書き込むときに使うEncoding
        System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");
        StreamWriter sw = new StreamWriter(rankingCSVPath, false, enc);
        for(int i=0; i< rankingNames.Count; i++){
            if(i+1 == myRank) sw.WriteLine(myName + ',' + rankingScores[i]);
            else sw.WriteLine(rankingNames[i] + ',' + rankingScores[i]);
        }
        sw.Close();
    }

    void ShowTimeResult(){
        
    }

}
