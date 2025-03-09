using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using DG.Tweening;

public class 人物技能 : MonoBehaviour
{
    public 人物属性 人物属性;
    public void 释放技能(string 技能名)
    {
        Type t = this.GetType();
        MethodInfo mi = t.GetMethod(技能名);
        mi.Invoke(this, null);
    }
    /// <summary>
    /// 往最近的怪物上发射一颗子弹
    /// </summary>
    public void 强化射击()
    {
        int 攻击力=1;float 弹道速度=10;float 飞行时间=3;float 检测范围=100;
        Collider2D[] 检测到的敌人 = Physics2D.OverlapCircleAll(transform.position, 检测范围, LayerMask.GetMask("敌人"));
        if(检测到的敌人.Length == 0)return;
        Transform 最近的敌人 = 检测到的敌人[0].transform;
        for(int i = 0; i < 检测到的敌人.Length; i++)
        {
            if(Vector2.Distance(transform.position,检测到的敌人[i].transform.position) < Vector2.Distance(transform.position,最近的敌人.position))
            {
                最近的敌人 = 检测到的敌人[i].transform;
            }
        }
        GameObject 强化射击 = 对象池.实例.获取("强化射击","技能");
        强化射击.GetComponent<强化射击>().Init(攻击力,飞行时间,(最近的敌人.position - transform.position),弹道速度,transform.position);
    }
    /// <summary>
    /// 每隔一段时间执行强化射击
    /// </summary>
    public void 射击()
    {
        StartCoroutine(每隔一段时间执行强化射击());
    }
    IEnumerator 每隔一段时间执行强化射击()
    {
        float 持续时间=3;float 时间间隔=0.4f;
        float timer = 0;
        while(timer<持续时间)
        {
            timer+=时间间隔;
            强化射击();
            yield return new WaitForSeconds(时间间隔);
        }
    }
    public void 翻滚()
    {
        float 翻滚距离=4f;
        transform.DOMove(transform.position + (Vector3)transform.GetComponent<Rigidbody2D>().velocity.normalized*翻滚距离,0.2f);
    }
    public void 冲撞()
    {
        float 冲撞距离=6f;
        transform.DOMove(transform.position + (Vector3)transform.GetComponent<Rigidbody2D>().velocity.normalized*冲撞距离,0.2f);
    }
    public void 防守()
    {
        人物属性.移动速度 -= 2f;
        float timer = 0;
        DOTween.To(()=>timer, x=>timer=x, 1, 2).OnComplete(()=>{
            人物属性.移动速度 += 2f;
        });
    }
}