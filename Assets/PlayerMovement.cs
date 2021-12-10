using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Timers;


public class PlayerMovement : MonoBehaviour
{
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
    private float jump;
    private int lock1;

    //private Timer timer = new Timer(10);
    

    void Start()
    {
        controller = GetComponent<CharacterController>();

        /*m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
        lock1 = 0;
       */
    }

    void Update()
    {
        /*m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        var stick = m_joyconL.GetStick();

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
        */

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


        // PCã‚Ì“®ìŠm”F—p
        if (Application.isEditor)
        {
            moveH = Input.GetAxis("Horizontal");
            moveV = Input.GetAxis("Vertical");
            jump = Input.GetAxis("Jump");         
                    
            //moveH = stick[0];
            //moveV = stick[1];
            /*var accel = m_joyconL.GetAccel();
            if (m_pressedButtonL != null)
            {
               
            }
            else
            {
            }

            float tmp = 0;
            int cnt = 0;

            if (accel.sqrMagnitude > 5)
            {
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
           /* }
            else
            {
                jump = 0;
                tmp = 0;
                //lock1 = 1;
                cnt = 0;
            }*/

           

        }
        // Oculus Quest2‚Å“®‚©‚·
        else
        {
            //moveH = stick[0];
            //moveV = stick[1];
            //moveH = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            //moveV = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
            //var accel = m_joyconL.GetAccel();

        }

        movement = new Vector3(moveH, jump, moveV);

        Vector3 desiredMove = cameraC.transform.forward * movement.z + cameraC.transform.up * movement.y + cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;

        
        if (jump > 0)
        {
            moveDir.y = desiredMove.y * 3f;
        }
        else
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
        

        controller.Move(moveDir * Time.deltaTime * speed);
    }
    /*private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;
        GUILayout.BeginHorizontal(GUILayout.Width(960));
        GUILayout.BeginVertical(GUILayout.Width(480));
        GUILayout.Label("time"+cnt);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }*/
}
