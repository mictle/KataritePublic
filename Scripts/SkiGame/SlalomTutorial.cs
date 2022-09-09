using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlalomMode{
    calibRelax,
    calibMeasure,
    calibEnd,
    direction,
    speedUp,
    gimmick,
    gameReady,
    countDown
}

public class SlalomTutorial : MonoBehaviour
{
    [SerilaizeField] Text tutorialText;

    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] GameObject calibObj;
    [SerializeField] GameObject directionObj;
    [SerializeField] GameObject gimmickObj;
    [SerializeField] GameObject gameReadyObj;

    [SerializeField] GameObject calibRelax;
    [SerializeField] GameObject calibMeasure;
    [SerializeField] GameObject calibEnd;



    private Calibration calib;
    private SlalomMode tutorialMode = SlalomMode.calibRelax;
    private float timer;
    private float timerLimit;

    [SerializeField] float calibrationTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        calib = tutorialCanvas.GetComponent<Calibration>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(tutorialMode){
            case SlalomMode.calibRelax:
                if(calib.relax){
                    EndMode(SlalomMode.calibRelax);
                    StartMode(SlalomMode.calibMeasure);
                }
                break;

            case SlalomMode.calibMeasure:
                if(cal.calend){
                    EndMode(SlalomMode.calibMeasure);
                    StartMode(SlalomMode.calibEnd);
                }
                break;

            case SlalomMode.calibEnd:
                if(TimerUpdate()){
                    EndMode(SlalomMode.calibEnd);
                    StartMode(SlalomMode.direciton);
                }
                break;
            case SlalomMode.direction:
                break;
        }
    }

    public void StartMode(SlalomMode mode){
        switch(mode){
            case SlalomMode.calibRelax:
                tutorialText.text = "準備運動";
                calibObj.SetActive(true);
                calibRelax.SetActive(true);
                calibMeasure.SetActive(false);
                calibEnd.SetActive(false);
                break;

            case SlalomMode.calibMeasure:
                calibMeasure.SetActive(true);
                break;

            case SlalomMode.calibEnd:
                calibEnd.SetActive(true);
                SetTimer(calibrationTime);
                break;
        }
        tutorialMode = mode;
    }

    public void EndMode(SlalomMode mode){
        switch(mode){
            case SlalomMode.calibMeasure:
                calibRelax.SetActive(false);
                break;
        }
    }

    private void SetTimer(float lim){
        timer = 0;
        timerLimit = lim;
    }

    private bool TimerUpdate(){
        timer += Time.deltaTime;
        return (timer>=timerLimit);
    }
}
