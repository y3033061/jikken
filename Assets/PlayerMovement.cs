using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Timers;


public class PlayerMovement : MonoBehaviour
{
    Transform m_transform;
    public float speed;
    private Vector3 movement;
    private CharacterController controller;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    public GameObject cameraC;
    private Vector3 moveDir = Vector3.zero;
    private float gravity = 9.8f;
    private float moveH;
    private float moveV;
    private float cameray;
    private float jump;
    private int lock1;
    private Boolean key;
    private Boolean col;

    private AudioSource sound01;
    public float volume;

    readonly Quaternion _BASE_ROTATION = Quaternion.Euler(180, 0, 0);

    //private Timer timer = new Timer(10);


    void Start()
    {
        controller = GetComponent<CharacterController>();

        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
        //lock1 = 0;
        key = true;
        col = true;

        // サポートするかの確認
        if (!SystemInfo.supportsGyroscope)
        {
            //Destroy(this);
            //return;
        }

        m_transform = transform;

        sound01 = GetComponent<AudioSource>();

        

    }

    void Update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        var stick = m_joyconL.GetStick();

        Quaternion gyro = m_joyconR.GetVector();
        //var accel = m_joyconL.GetAccel();

        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }
            if (m_joyconR.GetButton(button))
            {
                m_pressedButtonR = button;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_joyconL.SetRumble(160, 320, 0.6f, 200);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_joyconR.SetRumble(160, 320, 0.6f, 200);
        }

        /*int cnt = 0;
        float tmp = 0;
        var accel = m_joyconL.GetAccel();
        timer.Elapsed += (sender, e) =>
        {
            if (cnt < 100000)
            {

                var a = (float)(accel.sqrMagnitude * 0.1);
                if (a > tmp)
                {
                    jump = a;
                    tmp = a;
                }
                else
                {
                    jump = tmp;
                }

                moveDir.y = jump * 3f;
                //controller.Move(moveDir * Time.deltaTime * speed);

                cnt++;
            }
            else
            {
                timer.Stop();
            }
        };*/


        // PC上の動作確認用
        if (Application.isEditor)
        {
            moveH = Input.GetAxis("Horizontal");
            moveV = Input.GetAxis("Vertical");
            jump = Input.GetAxis("Jump");

            //moveH = stick[0];
            //moveV = stick[1];
            cameray += stick[0] * 5;
            var accel = m_joyconL.GetAccel(); // joycon の加速度を取得

            //x += Time.deltaTime * 10;
            //transform.rotation = Quaternion.Euler(x, 0, 0);

            // ボタンを押すとジャンプ出来るように
            if (m_pressedButtonL != null)
            {
                key = true;
                col = true;
            }
            else
            {
            }

            // ジャンプする
            if (accel.sqrMagnitude > 2)
            {
                StartCoroutine("jumping");
                /*
                while (true)
                {
                    
                    var a = (float)(accel.sqrMagnitude * 0.1);
                    if (a > tmp)
                    {
                        jump = a;
                        tmp = a;
                    }
                    else
                    {
                        jump = tmp;
                    }
                    moveDir.y = jump * 3f;
                    //controller.Move(moveDir * Time.deltaTime * speed);
                    cnt+=1;
                    if (cnt == 100000000)
                    {
                        break;
                    }
                }
                //jump = (float)(accel.sqrMagnitude * 0.1);

                /*timer.Elapsed += (sender, e) =>
                {
                    if (cnt < 100000)
                    {

                        var a = (float)(accel.sqrMagnitude * 0.1);
                        if (a > tmp)
                        {
                            jump = a;
                            tmp = a;
                        }
                        else
                        {
                            jump = tmp;
                        }
                        
                        moveDir.y = jump * 3f;
                        //controller.Move(moveDir * Time.deltaTime * speed);

                        cnt++;
                    }
                    else
                    {
                        timer.Stop();
                    }
                };
                timer.Start();*/

            }
            /*else
            {
                jump = 0;
                tmp = 0;
                //lock1 = 1;
                cnt = 0;
            }*/



        }
        // Oculus Quest2で動かす
        else
        {
            //moveH = stick[0];
            //moveV = stick[1];
            //moveH = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            //moveV = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

            cameray += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                // Aボタンが押された時の処理
                key = true;
            }

            var accel = m_joyconL.GetAccel();

            if (accel.sqrMagnitude > 2)
            {
                StartCoroutine("jumping");
            }
        }
        /*movement = new Vector3(moveH, jump, moveV);

        Vector3 desiredMove = cameraC.transform.forward * movement.z + cameraC.transform.up * movement.y + cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;

        
        if (jump > 0)
        {
            moveDir.y = desiredMove.y * 3f;
        }
        else
        {*/
        
        moveDir.y -= gravity * Time.deltaTime; // 重力を与える
        //}

        if (this.m_transform.position.y <= 2)
        {
            moveDir.x = 0;
            moveDir.z = 0;
        }

        // joyconの傾きに合わせて体を傾ける
        m_transform.localRotation = _BASE_ROTATION * (new Quaternion(-gyro.x, gyro.z, gyro.y, gyro.w));
        // コントローラーで
        this.transform.Rotate(0.0f, cameray, 0.0f);

        // 移動
        controller.Move(moveDir * Time.deltaTime * speed);

        //
        volume = (float)(this.transform.position.y * 0.01);
        sound01.volume = volume;

    }


    IEnumerator jumping()
    {
        float tmp = 0;
        int cnt = 0;

        
        //sound01.PlayOneShot(sound01.clip);

        Quaternion gyro = m_joyconR.GetVector();
        var qua = _BASE_ROTATION * (new Quaternion(-gyro.x, gyro.z, gyro.y, gyro.w));
        Vector3 vec3 = qua.eulerAngles;

        while (key == true)
        {
            yield return new WaitForSeconds(0.01f);
            var accel = m_joyconL.GetAccel();
            var a = (float)(accel.sqrMagnitude );
            if (a > tmp)
            {
                jump = a;
                tmp = a;
            }
            else
            {
                jump = tmp;
            }
            //jump = (float)(jump * 0.5);
            movement = new Vector3(0, jump, 0);
            Vector3 desiredMove = this.transform.forward * movement.z  + this.transform.up * movement.y + this.transform.right * movement.x;
            moveDir.x = desiredMove.x * 3f;
            moveDir.z = desiredMove.z * 3f;
            moveDir.y = desiredMove.y; //* 3f;
            controller.Move(moveDir * Time.deltaTime * speed);

            cnt++;

            if (cnt >= 100)
            {
                key = false;
                break;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {     
        moveDir.x = 0;
        moveDir.y = 0;
        moveDir.z = 0;
        controller.Move(moveDir * Time.deltaTime * speed);
    }
}
