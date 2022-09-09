using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotate : MonoBehaviour
{
    [SerializeField] private float wheelRadius = 1.0f; // タイヤの半径
    [SerializeField] private Transform carTransform; // 車本体のTransform
    private Vector3 beforePos; // 前のポジションを格納
    private float rotationSign; // 回転方向の符号

    void Start()
    {
        // タイヤの順回転・逆回転を決める
        // タイヤのX軸が車本体のX軸に向くよう取り付けられていれば1、逆向きなら-1とする
        rotationSign = Mathf.Sign(Vector3.Dot(carTransform.right, transform.right));

        // beforePosの初期値を設定しておく
        beforePos = transform.position;
    }

    void Update()
    {
        wheelRotate();
    }

    /// <summary>
    /// AIタイヤを回転
    /// </summary>
    private void wheelRotate()
    {
        // ワールド空間における位置変化を求め...
        var worldDeltaPos = transform.position - beforePos;

        // 車本体のローカル空間に直し...
        // （車本体のスケールは無視するべきかと思い、回転のみ考慮しました）
        var localDeltaPos = carTransform.InverseTransformDirection(worldDeltaPos);
        
        // そのZ成分から回転量を求める
        transform.Rotate(rotationSign * Mathf.Rad2Deg * localDeltaPos.z / wheelRadius, 0, 0);

        // beforePosを更新
        beforePos = transform.position;
    }
}
