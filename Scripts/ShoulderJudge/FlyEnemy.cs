 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyEnemy : MonoBehaviour
{
    public GameObject player;
    ShoulderDriveSimple sds;
    public Vector3 startPos;
    public Vector3 goalPos;
    public float bodyRot;
    public GameObject colorBall;
    ColorBallMove cbm;
    private Sequence sequence;
    ScoreRecorder sr;

    

    // Start is called before the first frame update
    void Start()
    {
        sds = player.GetComponent<ShoulderDriveSimple>();
        cbm = colorBall.GetComponent<ColorBallMove>();
        sr = player.GetComponent<ScoreRecorder>();
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity:200, sequencesCapacity:50);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FlyEnemyDown(){
        transform.DOLocalMove( goalPos, 1f).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(bodyRot, 0, 0), 1f );
        });
    }

    public void FlyEnemyUp(){
        transform.DOLocalMove(
            startPos, // 移動終了地点
            1f                    // 演出時間
        );
        transform.DOLocalRotate(
            new Vector3(0, 0, 0), // 終了時のRotation
            1f                     // 演出時間
        ).OnComplete(()=> {
            sequence.Kill();
        });
    }

    public void FlyEnemyShootAction(){
        transform.DOLocalMove(
            new Vector3(goalPos.x,goalPos.y + 1f, goalPos.z + 1f), 1f).SetEase(Ease.InCubic)                    // 演出時間
        .OnComplete(() =>
        {
            cbm.throwStart();
            transform.DOLocalMove(goalPos, .5f).SetEase(Ease.OutBounce)
            .OnComplete(() => 
            {
                FlyEnemyUp();
            });
        });
    }

    public void ShotAction(){
        sequence = DOTween.Sequence()
        .OnStart(()=>
            {FlyEnemyDown();}
        )
        .PrependCallback(() => 
        {
            FlyEnemyShootAction();
        })
        .PrependInterval(2f);
        sequence.Play();
    }

    public void Enemy_Destroy(){
        Destroy(this.gameObject);
    }
}