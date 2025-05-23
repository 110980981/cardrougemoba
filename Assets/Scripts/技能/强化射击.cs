using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class 强化射击 : MonoBehaviour
{
    public string 技能类型;
    public float 攻击力;
    public float 飞行时间;
    public void Init(float 攻击力,float 飞行时间,Vector3 方向,float 速度,Vector3 初始位置)
    {
        this.攻击力 = 攻击力;
        this.飞行时间 = 飞行时间;
        transform.position = 初始位置;
        GetComponent<Rigidbody2D>().velocity = 方向.normalized * 速度;
        transform.Find("贴图").Rotate(0,0,Mathf.Atan2(方向.y,方向.x)*Mathf.Rad2Deg);
        transform.Find("贴图").Rotate(0,0,-140);
        transform.DOShakePosition(0,3f).OnComplete(() => {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            对象池.实例.回收(gameObject,技能类型);
        });
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "敌人")
        {
            other.GetComponent<受击接口>().受击(GetComponent<Rigidbody2D>().velocity,攻击力);
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            对象池.实例.回收(gameObject,技能类型);
        }
    }
}
