using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 手刀 : MonoBehaviour
{
    Dictionary<GameObject, bool> 记录被击中的敌人 = new Dictionary<GameObject, bool>();
    float 手刀伤害,控制时间;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "敌人")
        {
            if (!记录被击中的敌人.ContainsKey(collision.gameObject))
            {
                记录被击中的敌人.Add(collision.gameObject, true);
                Debug.Log(collision.gameObject.name);
                collision.gameObject.GetComponent<受击接口>().受击((collision.gameObject.transform.position - transform.position).normalized,手刀伤害);
                collision.gameObject.GetComponent<敌人控制器>().被控制(控制时间);
            }
        }
    }
    public void Init(float 手刀伤害, float 控制时间)
    {
        this.手刀伤害 = 手刀伤害;
        this.控制时间 = 控制时间;
    }
}