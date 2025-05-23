using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using DG.Tweening;
using JetBrains.Annotations;

public class 人物技能 : MonoBehaviour
{
    public 人物属性 人物属性;
    public 人物控制器 人物控制器;
    public 人物受击 人物受击;
    public event EventHandler 释放技能事件;
    Type t;
    void Start()
    {
        t = this.GetType();
    }
    struct 组合技
    {
        public string 技能1, 技能2;
    }
    Dictionary<组合技, string> 组合技字典 = new Dictionary<组合技, string>()
    {
        {new 组合技(){技能1="冲撞",技能2="翻滚"}, "冲撞后跳"}
    };
    public string 获取组合技(string 技能1, string 技能2)
    {
        组合技 组合技1 = new 组合技()
        {
            技能1 = 技能1,
            技能2 = 技能2
        };
        组合技 组合技2 = new 组合技()
        {
            技能1 = 技能2,
            技能2 = 技能1
        };
        if (组合技字典.ContainsKey(组合技1)) return 组合技字典[组合技1];
        else if (组合技字典.ContainsKey(组合技2)) return 组合技字典[组合技2];
        else return "没有这个组合技";
    }
    public void 释放技能(string 技能名, params object[] 参数)
    {
        MethodInfo mi = t.GetMethod(技能名);
        if (mi == null) return;
        if (释放技能事件 != null) 释放技能事件(this, EventArgs.Empty);
        ParameterInfo[] 参数信息 = mi.GetParameters();
        // 检查参数数量是否匹配
        if (参数信息.Length == 0) mi.Invoke(this, null);
        else if (参数.Length == 参数信息.Length) mi.Invoke(this, 参数);
    }
    /// <summary>
    /// 往最近的怪物上发射一颗子弹
    /// </summary>
    public void 强化射击()
    {
        float 弹道速度 = 10; float 飞行时间 = 3; float 检测范围 = 100;
        Collider2D[] 检测到的敌人 = Physics2D.OverlapCircleAll(transform.position, 检测范围, LayerMask.GetMask("敌人"));
        if (检测到的敌人.Length == 0) return;
        Transform 最近的敌人 = 检测到的敌人[0].transform;
        for (int i = 0; i < 检测到的敌人.Length; i++)
        {
            if (Vector2.Distance(transform.position, 检测到的敌人[i].transform.position) < Vector2.Distance(transform.position, 最近的敌人.position))
            {
                最近的敌人 = 检测到的敌人[i].transform;
            }
        }
        GameObject 强化射击 = 对象池.实例.获取("强化射击", "技能");
        强化射击.GetComponent<强化射击>().Init(人物属性.伤害, 飞行时间, (最近的敌人.position - transform.position), 弹道速度, transform.position);
    }
    /// <summary>
    /// 每隔一段时间执行强化射击
    /// </summary>
    public void 射击()
    {
        IEnumerator 每隔一段时间执行强化射击()
        {
            float 持续时间 = 3; float 时间间隔 = 0.4f;
            float timer = 0;
            while (timer < 持续时间)
            {
                timer += 时间间隔;
                强化射击();
                yield return new WaitForSeconds(时间间隔);
            }
        }
        StartCoroutine(每隔一段时间执行强化射击());
    }
    public void 翻滚()
    {
        float 翻滚距离 = 4f;
        if (GetComponent<翻滚>() == null) gameObject.AddComponent<翻滚>();
        if (GetComponent<Rigidbody2D>().velocity.normalized.magnitude == 0) GetComponent<翻滚>()._翻滚(transform.right * 翻滚距离 * (transform.Find("贴图").GetComponent<SpriteRenderer>().flipX ? 1 : -1));
        else GetComponent<翻滚>()._翻滚(翻滚距离 * GetComponent<Rigidbody2D>().velocity.normalized);
    }
    public void 冲撞()
    {
        float 冲撞距离 = 6f;
        人物属性.冲撞伤害 = 人物属性.伤害 * 2;
        人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.冲撞, 人物控制器.人物当前状态);
        if (GetComponent<冲撞>() == null) gameObject.AddComponent<冲撞>();
        if (GetComponent<Rigidbody2D>().velocity.normalized.magnitude == 0) GetComponent<冲撞>()._冲撞(transform.right * 冲撞距离 * (transform.Find("贴图").GetComponent<SpriteRenderer>().flipX ? 1 : -1), 人物属性.冲撞伤害);
        else GetComponent<冲撞>()._冲撞(冲撞距离 * GetComponent<Rigidbody2D>().velocity.normalized, 人物属性.冲撞伤害);
    }
    public void 防守()
    {
        人物属性.移动速度 -= 2f;
        人物属性.固定格挡伤害 += 2;
        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1, 2).OnComplete(() =>
        {
            人物属性.固定格挡伤害 -= 2;
            人物属性.移动速度 += 2f;
        });
    }
    public void 冲撞后跳()//未完成,只有效果
    {
        float 冲撞距离 = 4f; float 冲撞伤害 = 3;
        人物属性.冲撞伤害 = 冲撞伤害;
        Vector3 原本位置 = transform.position;
        人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.冲撞后跳, 人物控制器.人物当前状态);
        transform.DOMove(transform.position + transform.right * 冲撞距离 * (transform.Find("贴图").GetComponent<SpriteRenderer>().flipX ? 1 : -1), 0.5f).OnComplete(() =>
        {
            transform.DOMove(原本位置, 0.1f).OnComplete(() =>
            {
                人物控制器.人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.待机, 人物控制器.人物当前状态);
            });
        });
    }
    public void 蓄力()
    {
        bool 蓄力被打断 = false;
        释放技能事件 += 释放技能时打断蓄力;
        void 释放技能时打断蓄力(object sender, EventArgs e) { 蓄力被打断 = true; }
        StartCoroutine(每隔一点二秒伤害加成提高());
        IEnumerator 每隔一点二秒伤害加成提高()
        {
            for (int i = 0; i < 5; i++)
            {
                if (蓄力被打断) break;
                人物属性.临时伤害加成 += 0.1f;
                yield return new WaitForSeconds(1.2f);
            }
        }
    }
    public void 隐藏()
    {
        人物控制器.A星寻路点.transform.localPosition = new Vector3(114514, 1919810, 0);
        Color tmp = 人物控制器.人物贴图.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        人物控制器.人物贴图.GetComponent<SpriteRenderer>().color = tmp;
        bool 隐藏被打断 = false;
        释放技能事件 += 释放技能时移除隐藏效果;
        void 释放技能时移除隐藏效果(object sender, EventArgs e)
        {
            隐藏被打断 = true;
            释放技能事件 -= 释放技能时移除隐藏效果;
        }
        StartCoroutine(每隔一秒恢复透明度());
        IEnumerator 每隔一秒恢复透明度()
        {
            for (int i = 0; i < 10; i++)
            {
                if (隐藏被打断) break;
                tmp.a = 0.5f + 0.05f * i;
                人物控制器.人物贴图.GetComponent<SpriteRenderer>().color = tmp;
                yield return new WaitForSeconds(1f);
            }
            tmp.a = 1;
            人物控制器.A星寻路点.transform.localPosition = new Vector3(0, 0, 0);
            人物控制器.人物贴图.GetComponent<SpriteRenderer>().color = tmp;
        }
    }
    public void 手刀()
    {
        float 控制时间 = 2;
        GameObject 手刀 = 对象池.实例.获取("手刀", "技能");
        手刀.GetComponent<手刀>().Init(人物属性.伤害 * 3, 控制时间);
        手刀.transform.SetParent(transform);
        手刀.transform.localPosition = new Vector3(0, 0, 0);
    }
    public void 瞭望()
    {
        Vector3 相机原本位置 = 人物控制器.跟随主角摄像机.transform.localPosition;
        人物控制器.跟随主角摄像机.transform.DOLocalMove(new Vector3(相机原本位置.x, 相机原本位置.y, -16), 1f);
        人物控制器.人物状态改变事件 += 改变状态时移除瞭望效果;
        void 改变状态时移除瞭望效果(object sender, EventArgs e)
        {
            人物控制器.跟随主角摄像机.transform.DOLocalMove(相机原本位置, 1f);
            人物控制器.人物状态改变事件 -= 改变状态时移除瞭望效果;
        }
    }
    public void 圆木盾_加入手牌()
    {
        人物属性.固定格挡伤害++;
    }
    public void 圆木盾()
    {
        人物属性.固定格挡伤害--;
    }
    public void 古老的咒文_加入手牌()
    {
        人物属性.固定伤害加成 += 1;
    }
    public void 古老的咒文()
    {
        人物属性.固定伤害加成 -= 1;
    }
    public void 守护戒指_加入手牌(GameObject 牌)
    {
        对象池.实例.获取("守护戒指", "技能").GetComponent<守护戒指>().初始化(牌);
    }
    public void 守护戒指()
    {
    }
    public void 誓言契约()
    {
        对象池.实例.获取("誓言契约", "技能");
    }
    public void 生命果()
    {
        人物属性.血量 += 3;
    }
}