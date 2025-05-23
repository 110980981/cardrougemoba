using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GifImporter;
using Pathfinding;

public class 敌人控制器 : MonoBehaviour
{
    public string 敌人类型;
    private 游戏所有类型的管理器.敌人状态 _敌人状态;
    public 游戏所有类型的管理器.敌人状态 敌人状态
    {
        get
        {
            return _敌人状态;
        }
        set
        {
            _敌人状态 = value;
            transform.GetComponent<AIPath>().maxSpeed = 0;
            if (value == 游戏所有类型的管理器.敌人状态.受击)
            {
                受击计时器 = 0;
            }
            else if (value == 游戏所有类型的管理器.敌人状态.移动)
            {
                transform.GetComponent<AIPath>().maxSpeed = 敌人基本属性.移动速度;
            }
        }
    }
    public 敌人基本属性 敌人基本属性;
    public 敌人受击 敌人受击;
    public GameObject 贴图;
    public Sprite 原始贴图;
    public Gif 受击动画;
    public Gif 死亡动画;
    public Gif 攻击动画;
    public float 解除控制时间;
    public bool 正在被控制;
    public float 受击计时器;
    bool 是否播放过受击动画前半段, 是否播放过受击动画后半段, 是否播放过死亡动画, 是否正在播放攻击动画;
    public float 受击僵直时间 = 0.5f;
    public Vector2 攻击方向;
    public Dictionary<GameObject, bool> 攻击距离检测 = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> 追踪距离检测 = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> 攻击碰撞盒内有没有人 = new Dictionary<GameObject, bool>();
    int[] 攻击帧列表 = new int[] { 3, 4 };
    void OnEnable()
    {
        Init();
    }
    void Update()
    {
        switch (敌人状态)
        {
            case 游戏所有类型的管理器.敌人状态.移动:
                移动状态();
                break;
            case 游戏所有类型的管理器.敌人状态.攻击:
                攻击状态();
                break;
            case 游戏所有类型的管理器.敌人状态.死亡:
                死亡状态();
                break;
            case 游戏所有类型的管理器.敌人状态.受击:
                受击状态();
                break;
            case 游戏所有类型的管理器.敌人状态.待机:
                待机状态();
                break;
            case 游戏所有类型的管理器.敌人状态.被控制:
                被控制状态();
                break;
        }
    }
    public void Init()
    {
        transform.tag = "敌人";
        transform.gameObject.layer = 6;
        敌人基本属性.GetComponent<敌人基本属性>().血量 = 敌人基本属性.GetComponent<敌人基本属性>().最大血量;
        追踪或攻击玩家();
    }
    public void 追踪或攻击玩家()
    {
        if (攻击距离检测.Count != 0)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.攻击, 敌人状态);
            return;
        }
        if (追踪距离检测.Count == 0)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.待机, 敌人状态);
        }
        else
        {
            float 距离 = float.MaxValue;
            GameObject 最近的玩家 = null;
            foreach (GameObject 玩家 in 追踪距离检测.Keys)
            {
                if (Vector2.Distance(transform.position, 玩家.transform.position) < 距离)
                {
                    最近的玩家 = 玩家;
                    距离 = Vector2.Distance(transform.position, 玩家.transform.position);
                }
            }
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.移动, 敌人状态);
            if (最近的玩家.transform.GetComponent<人物控制器>() != null)
                transform.GetComponent<AIDestinationSetter>().target = 最近的玩家.transform.GetComponent<人物控制器>().A星寻路点.transform;
            else transform.GetComponent<AIDestinationSetter>().target = 最近的玩家.transform;
        }
    }
    public void 移动状态()
    {
        贴图.GetComponent<SpriteRenderer>().sprite = 原始贴图;
    }
    public void 受击状态()
    {
        if (!是否播放过受击动画前半段)
        {
            是否播放过受击动画前半段 = true;
            if (贴图.GetComponent<播放一次帧动画>() != null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(受击动画, 1, 受击动画.Frames.Count / 2,受击中断回调);
            else 贴图.AddComponent<播放一次帧动画>().播放序列帧(受击动画, 1, 受击动画.Frames.Count / 2,受击中断回调);
        }
        受击计时器 += Time.deltaTime;
        if (受击计时器 > 受击僵直时间)
        {
            if (是否播放过受击动画后半段) return;
            是否播放过受击动画后半段 = true;
            贴图.GetComponent<播放一次帧动画>().播放序列帧(受击动画, 受击动画.Frames.Count / 2 + 1, 受击动画.Frames.Count - 1,受击中断回调, 受击结束回调);
        }
    }
    public void 受击中断回调()
    {
        是否播放过受击动画前半段 = false;
        是否播放过受击动画后半段 = false;
    }
    public void 受击结束回调()
    {
        是否播放过受击动画前半段 = false;
        是否播放过受击动画后半段 = false;
        if (敌人状态 == 游戏所有类型的管理器.敌人状态.受击)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
            追踪或攻击玩家();
        }
    }
    public void 死亡状态()
    {
        if (是否播放过死亡动画) return;
        是否播放过死亡动画 = true;
        transform.GetComponent<AIPath>().maxSpeed = 0;
        if (贴图.GetComponent<播放一次帧动画>() != null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(死亡动画, 1, 死亡动画.Frames.Count - 1, 死亡中断回调,死亡结束回调);
        else 贴图.AddComponent<播放一次帧动画>().播放序列帧(死亡动画, 1, 死亡动画.Frames.Count - 1,死亡中断回调, 死亡结束回调);
    }
    public void 死亡中断回调()
    {
        是否播放过死亡动画 = false;
        transform.GetComponent<AIPath>().maxSpeed = 敌人基本属性.移动速度;
    }
    public void 死亡结束回调()
    {
        是否播放过死亡动画 = false;
        敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
        transform.GetComponent<AIPath>().maxSpeed = 敌人基本属性.移动速度;
        攻击距离检测.Clear();
        追踪距离检测.Clear();
        攻击碰撞盒内有没有人.Clear();
        对象池.实例.回收(gameObject, 敌人类型);
    }
    int 记录目前攻击到第几帧;
    public void 攻击状态()
    {
        if (是否正在播放攻击动画) return;
        是否正在播放攻击动画 = true;
        记录目前攻击到第几帧 = 0;
        if (贴图.GetComponent<播放一次帧动画>() != null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(攻击动画, 1, 攻击动画.Frames.Count - 1,攻击中断回调, 攻击结束回调, 每帧判断是否进行攻击, 敌人基本属性.攻击速度);
        else 贴图.AddComponent<播放一次帧动画>().播放序列帧(攻击动画, 1, 攻击动画.Frames.Count - 1,攻击中断回调, 攻击结束回调, 每帧判断是否进行攻击, 敌人基本属性.攻击速度);
    }
    public void 每帧判断是否进行攻击()
    {
        for (int i = 0; i < 攻击帧列表.Length; i++)
        {
            if (记录目前攻击到第几帧 == 攻击帧列表[i])
            {
                if (攻击碰撞盒内有没有人.Count > 0)
                {
                    foreach (GameObject 玩家 in 攻击碰撞盒内有没有人.Keys)
                    {
                        玩家.GetComponent<受击接口>().受击((玩家.transform.position - transform.position).normalized*3, GetComponent<敌人基本属性>().攻击力);
                    }
                }
            }
        }
        记录目前攻击到第几帧++;
    }
    public void 攻击中断回调()
    {
        是否正在播放攻击动画 = false;
    }
    public void 攻击结束回调()
    {
        是否正在播放攻击动画 = false;
        敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.移动, 敌人状态,true);
        追踪或攻击玩家();
    }
    public void 待机状态()
    {
        贴图.GetComponent<SpriteRenderer>().sprite = 原始贴图;
    }
    public void 被控制状态()
    {
        if (正在被控制 == false)
        {
            GetComponent<AIPath>().maxSpeed = 0;
            正在被控制 = true;
        }
        else if (Time.time > 解除控制时间)
        {
            GetComponent<AIPath>().maxSpeed = 敌人基本属性.移动速度;
            正在被控制 = false;
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.移动, 敌人状态, true);
            追踪或攻击玩家();
        }
    }
    public void 被控制(float 控制时间)
    {
        解除控制时间 = Time.time + 控制时间;
        敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.被控制, 敌人状态);
    }
}