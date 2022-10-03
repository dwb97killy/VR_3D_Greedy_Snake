using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerControl_Body : MonoBehaviour
{
 private Vector3 m_camRot;
 private Transform m_camTransform;//摄像机Transform
 private Transform m_transform;//摄像机父物体Transform
 private Transform m_transform_pre;//记录运动前的摄像机父物体Transform
 public float m_movSpeed=5;//移动系数
 public float m_rotateSpeed=60;//旋转系数
 public Vector3 movement_dis;
 public float movement_ang;
 private Rigidbody rd;
 public int force = 30;
 private int flag = 0;
 private float times;//定义一个数值
 
 private void Start()
 {
  times = 0;
  m_camTransform = Camera.main.transform;
  m_transform = GetComponent<Transform>();
  rd = GetComponent<Rigidbody>();//给变量赋值变量
 }
 private void Update()
 {
  m_transform_pre = m_transform;
  Control();
  for(int i = 0;i < m_transform.childCount;i++)
  {
   times += Time.deltaTime;
   if (times > 0.1*(i+1))
   {
     times = 0;
     m_transform.GetChild(i).Rotate(Vector3.right, movement_ang);
     m_transform.GetChild(i).Translate(movement_dis,Space.Self);
   }
  }
  //m_transform.Rotate(Vector3.right, movement_ang);
  //m_transform.Translate(movement_dis,Space.Self);
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
  camrot.x = 0; camrot.z = 0;
  // m_transform.eulerAngles = camrot;
 
  // 定义4个值控制移动
  float xm = 0, ym = 0, zm = 0, xa = 0;

  if (Input.GetKey(KeyCode.Space)) //按空格加速
  {
   m_movSpeed = 15;
  }
  if (Input.GetKeyUp(KeyCode.Space)) //松开恢复
  {
   m_movSpeed = 5;
  }

  if (Input.GetKey(KeyCode.W)) //按键盘W向前移动
  {
   ym += m_movSpeed * Time.deltaTime;
   flag = 0;
  }
  else if (Input.GetKey(KeyCode.S))//按键盘S向后移动
  {
   ym -= m_movSpeed * Time.deltaTime;
   flag = 1;
  }
 
  if (Input.GetKey(KeyCode.A))//按键盘A向左移动，方向偏转
  {
   zm -= m_movSpeed * Time.deltaTime / 2;
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
   zm += m_movSpeed * Time.deltaTime / 2;
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
   xm += m_movSpeed * Time.deltaTime;
  }
  else if (Input.GetKey(KeyCode.P))//按键盘P向下移动，方向偏转
  {
   xm -= m_movSpeed * Time.deltaTime;
  }
 
  movement_dis = new Vector3(xm,ym,zm);
  movement_ang = xa;
 }

 private void OnTriggerEnter(Collider other)
 {
  if(other.tag=="Ball")
  {
   Destroy(other.gameObject);
  }
  if(other.tag=="Wall")
  {
   m_transform.Rotate(Vector3.up, -movement_ang);
   m_transform.Translate(-10 * movement_dis,Space.Self);
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
  }
 } 


 public static IEnumerator DelayFuc(Action action,float delaySeconds)
 {
        yield return new WaitForSeconds (delaySeconds);
        action ();
 }
}