using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(JoyconData))]

public class skiing_movement : MonoBehaviour
{
    // public float speed = 6.0f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float gravity = 20.0f;
    private float x = 0.0f; //x方向の坂によるスピード
    private float z = 0.0f; //z方向の坂によるスピード
    [SerializeField] private float x_accele = 20.0f;
    [SerializeField] private float z_break = 20.0f;
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float direction_to = 0.0f;

    //joyCon使うかどうか
    [SerializeField]
    private bool joyConUse;

    
    //加速中の残り時間保存
    [SerializeField]
    private float accelTime;

    //2本のレイにより角度を計測するかどうか
    [SerializeField]
    private bool UseDualRay;

    //2本のレイの中央からのずれ
    [SerializeField]
    private Vector3 rayPosDistance;

    //raycast時地面判定に使うレイヤー
    [SerializeField]
    private LayerMask layerMask;

    //レイの中心座標
    [SerializeField]
    private Vector3 rayCenterPos;

    private float before_direction_to = 0.0f;
    private float normalvect = 0.0f;
    private float accele = 0.0f;
    private float z_speed = 0.0f;

    private Vector3 normalVector = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool raybool;
    public Transform myTransform;

    private JoyconData _joyconD;//joyconのデータ取得

    public Animator skiing_animation;

    private Example EXP;

    float tmp = 0.0f;

    private Example Hundleflag_switch;

    private ParticleSystem Jump_Action_Particle;

    public GameObject Jump_Action_Effect;

    public int point = 0;
    public GameObject flag_get_system;
    public AudioClip flag_get_sound_effect;
    private ParticleSystem flag_get_particle;
    private AudioSource get_audio;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _joyconD = GetComponent<JoyconData>();
        skiing_animation = GetComponent<Animator>();
        accelTime = 0.0f;
        EXP = GetComponent<Example>();
        Hundleflag_switch = GetComponent<Example>();
        EXP.Load();
        Jump_Action_Particle = Jump_Action_Effect.GetComponentInChildren<ParticleSystem>();
        flag_get_particle = flag_get_system.GetComponentInChildren<ParticleSystem>();
        get_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //肩の傾き
        if (joyConUse) {
            tmp = (float)(-_joyconD.readXR() + _joyconD.readXL());
            if (tmp > 1) direction_to = 1;
            else if (tmp < -1) direction_to = -1;
            else direction_to = tmp;
        }

        if(UseDualRay){

            Ray ray1 = new Ray(transform.TransformPoint(rayCenterPos + rayPosDistance), -transform.up);
            Ray ray2 = new Ray(transform.TransformPoint(rayCenterPos - rayPosDistance), -transform.up);
            RaycastHit hit1, hit2;
            raybool = Physics.Raycast(ray1, out hit1,2.0f, layerMask);
            raybool = raybool & Physics.Raycast(ray2, out hit2,2.0f, layerMask);
            Vector3 slopeVec = hit1.point - hit2.point;
            
            Debug.DrawRay(ray1.origin, ray1.direction,Color.green,0);
            Debug.DrawRay(ray2.origin, ray2.direction,Color.red,0);
            if(raybool)normalVector = (new Vector3(0, slopeVec.z, -slopeVec.y)).normalized;
            else normalVector = Vector3.up;

            Debug.Log("normalVec" + normalVector);

        }else{
            Vector3 rayPosition = transform.position + new Vector3(0.0f, 0.0f, 0.0f);
            Ray ray = new Ray(rayPosition,Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction,Color.red,1.0f);
            RaycastHit hit;
            raybool = Physics.Raycast(ray, out hit,1.0f);
            Debug.Log(raybool);
            normalVector = hit.normal;
            Debug.Log(hit.normal);
            Debug.Log("法線座標"+normalVector.x);

            
        }


        before_direction_to = direction_to;
        //Debug.Log("before_direction_to" + before_direction_to);

        //方向制御（キー）
        // if(Input.GetKey(KeyCode.L)){direction_to += Time.deltaTime;
        //     if(direction_to > 1){direction_to = 1.0f;}}
        // else if(Input.GetKey(KeyCode.J) ){direction_to -= Time.deltaTime;
        //     if(direction_to < -1){direction_to = -1.0f;}}
        // else if(direction_to > 0){direction_to -= Time.deltaTime;}
        // else if(direction_to < 0){direction_to += Time.deltaTime;}

        if (direction_to == 1.0 && before_direction_to == 1.0 || direction_to == -1.0 && before_direction_to == -1.0) { before_direction_to = 0; }

        skiing_animation.SetFloat("Blend", (direction_to + 1.0f) / 2.0f);

        //速度制御
        // x =  Mathf.Sign(direction_to) * gravity *normalVector.x * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)))* Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)))
        //     +  Mathf.Sign(direction_to) * gravity *normalVector.z * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to))) * Mathf.Cos(Mathf.PI/2 *  (1 - Mathf.Abs(direction_to)));
        // Debug.Log("法線x"+ x);
        z = gravity * normalVector.z;
        // z = Mathf.Sign(direction_to) * gravity *normalVector.x * Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to))* Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to))
        //     +  Mathf.Sign(direction_to) * gravity *normalVector.z * Mathf.Cos(Mathf.PI/2 * Mathf.Abs(direction_to)) * Mathf.Cos(Mathf.PI/2 *  Mathf.Abs(direction_to));
        //gravity * normalVector.z;
        //     moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //     moveDirection = transform.TransformDirection(moveDirection);
        //     moveDirection = moveDirection * speed;
        // moveDirection.y = 0;

        //ジャンプ
        //if (Input.GetButton("Jump"))
        //{
        //    moveDirection.y = jumpSpeed;
        //}
        if (Input.GetKey(KeyCode.P))
        {
            moveDirection.y = moveDirection.z*0.5f;
        }
        //加速

        if (Input.GetKey(KeyCode.K)) AccelByTime(0.5f);

        if(accelTime>0){
            skiing_animation.SetBool("SpeedUp", true);
            accelTime -= Time.deltaTime;
           
            if (accele < 5.0f){
                accele += 0.1f;
            }
        }
        else if(accele > 0){
            skiing_animation.SetBool("SpeedUp", false);
            accele -= 0.1f;
        }


        Debug.Log("y" + moveDirection.y);
        Debug.Log("x" + moveDirection.x);
        Debug.Log("direction_to" + direction_to);
        Debug.Log("before_direction_to" + before_direction_to);
        if (raybool == false) {//接地
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime); 
            
        }

        moveDirection.x = x_accele * direction_to + accele * Mathf.Sin(Mathf.PI / 2 * direction_to);//横方向の移動
        if (accele > 0) {
            if (direction_to != 1.0 && direction_to != -1.0) {
                z_speed = moveDirection.z - accele * Mathf.Cos(Mathf.PI / 2 * before_direction_to);
            }
            else {
                z_speed = moveDirection.z - accele * Mathf.Cos(Mathf.PI / 2);
            }

            if (z_speed >= 0 && z_speed < 3) { z_speed = z_speed + (z * Time.deltaTime);
                if (z_speed < 0) { z_speed = 0; } }
            else if (z_speed >= 3) { z_speed = z_speed + (z * Time.deltaTime - z_break * Mathf.Abs(direction_to) + z_break * Mathf.Abs(before_direction_to));
                if (z_speed < 3) { z_speed = 3; } }
            if (z_speed > 20) { z_speed = 20; }
            moveDirection.z = z_speed + accele * Mathf.Cos(Mathf.PI / 2 * direction_to);
        }
        else {
            if (moveDirection.z >= 0 && moveDirection.z < 3) { moveDirection.z = moveDirection.z + (z * Time.deltaTime);
                if (moveDirection.z < 0) { moveDirection.z = 0; } }
            else if (moveDirection.z >= 3) { moveDirection.z = moveDirection.z + (z * Time.deltaTime - z_break * Mathf.Abs(direction_to) + z_break * Mathf.Abs(before_direction_to));
                if (moveDirection.z < 3) { moveDirection.z = 3; } }
            if (moveDirection.z > 20) { moveDirection.z = 20; } }
        Debug.Log("z" + moveDirection.z);
        controller.Move(moveDirection * Time.deltaTime);


        Vector3 localAngle = myTransform.localEulerAngles;//アバターを地面に垂直に立たせる
        if (raybool == true) {
            localAngle.x = Vector3.SignedAngle(Vector3.up, normalVector, Vector3.right);
            skiing_animation.SetBool("jump",false);
        }
        Debug.Log("回転" + localAngle.x + localAngle.y + localAngle.z);

        myTransform.localEulerAngles = localAngle;

        if(Input.GetKey(KeyCode.J))
        {
            Hundleflag_switch.hundleflag = true;
            gravity =20f;
            moveDirection.y -= 15f;
            //moveDirection.z += 0.1f;
            //Invoke("Gravity_change", 1.0f);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("jumpArea"))
        {
            skiing_animation.SetBool("jump", true);
            moveDirection.y = moveDirection.z * 0.5f;
            Jump_Action_Particle.Play();
        }
        if(other.CompareTag("snowman"))
        {
            moveDirection.z =0f;
        }

        if (other.CompareTag("flags"))
        {
            point += 10;
            flag_get_particle.Play();
            get_audio.PlayOneShot(flag_get_sound_effect);
            Debug.Log("ポイント" + point);

        }
    }

    public float direction_into_particle(){
        Debug.Log("direction_to:" + direction_to);    
        return direction_to;
        }
    public float speed_to_z()
    {
      return moveDirection.z;
    }

    void Gravity_change()
    {
        gravity = 20f;
    }

    public bool ground_judge()
    {
        return raybool;
    }

    public void AccelByTime(float time){
        accelTime = time;
    }

    public void SkiiJump(){

    }

    public void TurnRight(){

    }

    public void TurnLeft(){

    } 

}
