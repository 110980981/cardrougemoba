using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using DG.Tweening;

public class 人物技能 : MonoBehaviour
{
    public 人物属性 人物属性;
    public 人物控制器 人物控制器;
    Type t;
    void Start()
    {
        t = this.GetType();
    }
    struct 组合技
    {
        public string 技能1,技能2;
    }
    Dictionary<组合技,string> 组合技字典 = new Dictionary<组合技, string>()
    {
        {new 组合技(){技能1="冲撞",技能2="翻滚"}, "冲撞后跳"}
    };
    public string 获取组合技(string 技能1,string 技能2)
    {
        组合技 组合技1 = new 组合技()
        {
            技能1=技能1,
            技能2=技能2
        };
        组合技 组合技2 = new 组合技()
        {
            技能1=技能2,
            技能2=技能1
        };
        if(组合技字典.ContainsKey(组合技1))return 组合技字典[组合技1];
        else if(组合技字典.ContainsKey(组合技2))return 组合技字典[组合技2];
        else return "没有这个组合技";
    }
    public void 释放技能(string 技能名)
    {
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
        if(GetComponent<翻滚>()==null)gameObject.AddComponent<翻滚>();
        if(GetComponent<Rigidbody2D>().velocity.normalized.magnitude == 0)GetComponent<翻滚>()._翻滚(transform.right*翻滚距离*(transform.Find("贴图").GetComponent<SpriteRenderer>().flipX?1:-1));
        else GetComponent<翻滚>()._翻滚(翻滚距离*GetComponent<Rigidbody2D>().velocity.normalized);
    }
    public void 冲撞()
    {
        float 冲撞距离=6f;int 冲撞伤害=3;
        人物属性.冲撞伤害 = 冲撞伤害;
        人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.冲撞,人物控制器.人物当前状态);
        if(GetComponent<冲撞>()==null)gameObject.AddComponent<冲撞>();
        if(GetComponent<Rigidbody2D>().velocity.normalized.magnitude == 0) GetComponent<冲撞>()._冲撞(transform.right*冲撞距离*(transform.Find("贴图").GetComponent<SpriteRenderer>().flipX?1:-1),冲撞伤害);
        else GetComponent<冲撞>()._冲撞(冲撞距离*GetComponent<Rigidbody2D>().velocity.normalized,冲撞伤害);
    }
    public void 防守()
    {
        人物属性.移动速度 -= 2f;
        float timer = 0;
        DOTween.To(()=>timer, x=>timer=x, 1, 2).OnComplete(()=>{
            人物属性.移动速度 += 2f;
        });
    }
    public void 冲撞后跳()
    {
        float 冲撞距离=4f;int 冲撞伤害=3;
        人物属性.冲撞伤害 = 冲撞伤害;
        Vector3 原本位置 = transform.position;
        人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.冲撞后跳,人物控制器.人物当前状态);
        transform.DOMove(transform.position+transform.right*冲撞距离*(transform.Find("贴图").GetComponent<SpriteRenderer>().flipX?1:-1), 0.5f).OnComplete(()=>{
            transform.DOMove(原本位置, 0.1f).OnComplete(()=>{
                人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.待机,人物控制器.人物当前状态);
            });
        });
    }
}