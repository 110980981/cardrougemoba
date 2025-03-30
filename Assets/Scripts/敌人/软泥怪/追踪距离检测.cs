using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 追踪距离检测 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("玩家进入追踪范围");
            transform.parent.transform.GetComponent<软泥怪控制器>().追踪距离检测[collision.gameObject] = true;
            transform.parent.transform.GetComponent<软泥怪控制器>().追踪玩家();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("玩家离开追踪范围");
            if(transform.parent.transform.GetComponent<软泥怪控制器>().追踪距离检测.ContainsKey(collision.gameObject))
                transform.parent.transform.GetComponent<软泥怪控制器>().追踪距离检测.Remove(collision.gameObject);
            transform.parent.transform.GetComponent<软泥怪控制器>().追踪玩家();
        }
    }
}
