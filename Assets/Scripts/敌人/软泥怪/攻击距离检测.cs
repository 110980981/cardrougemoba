using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 攻击距离检测 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("玩家进入攻击范围");
            transform.parent.GetComponent<敌人控制器>().攻击距离检测[collision.gameObject] = true;
            transform.parent.transform.GetComponent<敌人控制器>().追踪或攻击玩家();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("玩家离开攻击范围");
            if(transform.parent.GetComponent<敌人控制器>().攻击距离检测.ContainsKey(collision.gameObject))
                transform.parent.GetComponent<敌人控制器>().攻击距离检测.Remove(collision.gameObject);
            transform.parent.transform.GetComponent<敌人控制器>().追踪或攻击玩家();
        }
    }
}
