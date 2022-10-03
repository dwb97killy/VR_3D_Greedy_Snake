using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerControl : MonoBehaviour
{
 private Vector3 m_camRot;
 private Transform m_camTransform;//摄像机Transform
 private Transform m_transform;//摄像机父物体Transform
 private Transform m_transform_pre;//记录运动前的摄像机父物体Transform
 private float m_movSpeed;//移动系数
 private float m_rotateSpeed;//旋转系数
 public Vector3 movement_dis;
 public float movement_ang;
 private Rigidbody rd;
 private int flag;
 private int hit;
 private int hitwall;
 private Vector3 v;
 private Vector3 v1;
 private Vector3 v2;
 private Vector3[] FollowTransform_po;
 private Vector3[] FollowTransform_ro;
 public GameObject prefab;
 private GameObject camera0;
 private GameObject camera1;
 
 private int speed_count = 6;

 private void Start()
 {
    m_movSpeed=5;
    m_rotateSpeed=60;
    flag = 0;
    hit = 0;
    hitwall = 0;
    v = new Vector3(0, 180, 90);
    v1 = new Vector3(0, 0, -45);
    v2 = new Vector3(0, 0, 45);
    FollowTransform_po = new Vector3[15000];
    FollowTransform_ro = new Vector3[15000];
    //prefab = GameObject.Find("node");
    camera0 = GameObject.Find("Camera0");
    //camera1 = GameObject.Find("Camera");
    camera0.SetActive(false);
    m_camTransform = Camera.main.transform;
    m_transform = GetComponent<Transform>();
    rd = GetComponent<Rigidbody>();//给变量赋值变量
 }
 private void Update()
 {
  speed_count = 6;
  m_transform_pre = m_transform;
  Control();
  if (movement_dis != Vector3.zero || movement_ang != 0)
  {
    m_transform.GetChild(0).Rotate(Vector3.up, movement_ang, Space.Self);
    if (movement_dis != Vector3.zero) 
    {
      m_transform.GetChild(0).Translate(movement_dis,Space.Self);
    }
    else
    {
      m_transform.GetChild(0).position = m_transform_pre.GetChild(0).position;
    }
    Vector3 b = Vector3.zero;
    b.x = m_transform.GetChild(0).eulerAngles.x;
    b.y = m_transform.GetChild(0).eulerAngles.y;
    b.z = 0;
    m_transform.GetChild(0).eulerAngles = b;
  }

  
  /*if (movement_dis[1] > 0)
  {
    Vector3 b = Vector3.zero;
    b.x = m_transform.GetChild(0).eulerAngles.x;
    b.y = m_transform.GetChild(0).eulerAngles.y;
    b.z = 30;
    m_transform.GetChild(0).eulerAngles = b;
  }
  else if (movement_dis[1] < 0)
  {
    Vector3 b = Vector3.zero;
    b.x = m_transform.GetChild(0).eulerAngles.x;
    b.y = m_transform.GetChild(0).eulerAngles.y;
    b.z = -30;
    m_transform.GetChild(0).eulerAngles = b;
  }
  else 
  {
    Vector3 b = Vector3.zero;
    b.x = m_transform.GetChild(0).eulerAngles.x;
    b.y = m_transform.GetChild(0).eulerAngles.y;
    b.z = 0;
    m_transform.GetChild(0).eulerAngles = b;
  }*/

  if (hit == 1)
  {
   GameObject prefabInstance = Instantiate(prefab);
   prefabInstance.transform.parent = m_transform;
   m_transform.GetChild(m_transform.childCount - 2).position = m_transform.GetChild(m_transform.childCount - 1).position;
   m_transform.GetChild(m_transform.childCount - 2).eulerAngles = m_transform.GetChild(m_transform.childCount - 1).eulerAngles;
   hit = 0; 
  }

  if (movement_dis != Vector3.zero && hitwall == 0)
  {
   for (int i = 0 ; i < 14999 ; i++)
   {
    if (FollowTransform_po[14998 - i] != Vector3.zero)
    {
     FollowTransform_po[14999 - i] = FollowTransform_po[14998 - i];
    }
    if (FollowTransform_ro[14998 - i] != Vector3.zero)
    {
     FollowTransform_ro[14999 - i] = FollowTransform_ro[14998 - i];
    }
   }
   FollowTransform_po[0] = m_transform.GetChild(0).position;
   FollowTransform_ro[0] = m_transform.GetChild(0).eulerAngles + v;
   if (movement_dis.y > 0)
   {
     FollowTransform_ro[0] = FollowTransform_ro[0] + v1;
     if (movement_dis.x == 0 && movement_dis.z == 0)
     {
       FollowTransform_ro[0] = FollowTransform_ro[0] + v1;
     }
   }
   else if (movement_dis.y < 0)
   {
     FollowTransform_ro[0] = FollowTransform_ro[0] + v2;
     if (movement_dis.x == 0 && movement_dis.z == 0)
     {
       FollowTransform_ro[0] = FollowTransform_ro[0] + v2;
     }
   }
   for(int j = 1; j < m_transform.childCount; j++)
   {
      if (FollowTransform_po[speed_count * j] != Vector3.zero)
      {
       m_transform.GetChild(j).position = FollowTransform_po[speed_count * j];
       m_transform.GetChild(j).eulerAngles = FollowTransform_ro[speed_count * j];
      }
   } 
  }
  else if (hitwall == 1)
  {
    hitwall = 0;
  }
 }

 void Control()
 {
  if (Input.GetMouseButton(0))
  {
   //获取鼠标移动距离
   float rh = Input.GetAxis("Mouse X");
   float rv = Input.GetAxis("Mouse Y");
 
   // 旋转摄像机
   m_camRot.x -= rv * m_rotateSpeed;
   m_camRot.y += rh * m_rotateSpeed;
 
  }
 
  m_camTransform.eulerAngles = m_camRot;
 
  // 使主角的面向方向与摄像机一致
  Vector3 camrot = m_camTransform.eulerAngles;
  camrot.x = 200; camrot.z = 200;
  // m_transform.eulerAngles = camrot;
 
  // 定义4个值控制移动
  float xm = 0, ym = 0, zm = 0, xa = 0;

  if (Input.GetKey(KeyCode.Space)) //按空格加速
  {
   m_movSpeed = 15;
   speed_count = 3;
  }
  if (Input.GetKeyUp(KeyCode.Space)) //松开恢复
  {
   m_movSpeed = 5;
   speed_count = 6;
  }

  if (Input.GetKey(KeyCode.W)) //按键盘W向前移动
  {
   xm += m_movSpeed * Time.deltaTime;
   flag = 0;
  }
  else if (Input.GetKey(KeyCode.S))//按键盘S向后移动
  {
   xm -= m_movSpeed * Time.deltaTime;
   flag = 1;
  }
 
  if (Input.GetKey(KeyCode.A))//按键盘A向左移动，方向偏转
  {
   //xm += m_movSpeed * Time.deltaTime;
   //zm += m_movSpeed * Time.deltaTime / 2;
   if (flag == 0)
   {
    xa -= m_rotateSpeed * Time.deltaTime;
   } 
   else if (flag == 1)
   {
    xa += m_rotateSpeed * Time.deltaTime;
   }
  }
  else if (Input.GetKey(KeyCode.D))//按键盘D向右移动，方向偏转
  {
   //xm += m_movSpeed * Time.deltaTime;
   //zm -= m_movSpeed * Time.deltaTime / 2;
   if (flag == 0)
   {
    xa += m_rotateSpeed * Time.deltaTime;
   } 
   else if (flag == 1)
   {
    xa -= m_rotateSpeed * Time.deltaTime;
   }
  }
  else 
  {
    flag = 0;
  }

  if (Input.GetKey(KeyCode.O))//按键盘O向上移动，方向偏转
  {
   ym += m_movSpeed * Time.deltaTime / 2;
  }
  else if (Input.GetKey(KeyCode.P))//按键盘P向下移动，方向偏转
  {
   ym -= m_movSpeed * Time.deltaTime / 2;
  }
 
  movement_dis = new Vector3(xm,ym,zm);
  movement_ang = xa;
 }

 private void OnTriggerEnter(Collider other)
 {
  if(other.tag=="Ball")
  {
   gamecontroller.instance.IncreaseScore();
   Destroy(other.gameObject);
   //GameObject pfb = Resources.Load("Assets/Prefabs/snake") as GameObject;
   //GameObject prefabInstance = Instantiate(pfb);
   hit = 1;
  }
  if(other.tag=="Wall")
  {
   m_transform.GetChild(0).Rotate(Vector3.up, -movement_ang,Space.Self);
   m_transform.GetChild(0).Translate(-2 * movement_dis,Space.Self);
   //m_transform.GetChild(0).position = FollowTransform_po[0];
   //m_transform.GetChild(0).eulerAngles = FollowTransform_ro[0];
   hitwall = 1;
   //m_movSpeed = 0;
   //m_rotateSpeed = 0;
  }
 }

 private void OnTriggerStay(Collider other)
 {
  if(other.tag=="Wall")
  {
   //rd.AddForce(new Vector3(1, 0, 0) * force * 100 * Time.deltaTime);//对物体施加力
   //m_movSpeed = 0;
   //m_rotateSpeed = 0;
  }
 }

 private void OnTriggerExit(Collider other)
 {
  if(other.tag=="Wall")
  {
   m_movSpeed = 5;
   m_rotateSpeed = 60;
   hitwall = 1;
  }
 } 
}