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
            if(value==游戏所有类型的管理器.敌人状态.受击)
            {
                受击计时器=0;
            }
            else if(value==游戏所有类型的管理器.敌人状态.移动)
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
    public float 受击计时器;
    bool 是否播放过受击动画前半段,是否播放过受击动画后半段,是否播放过死亡动画,是否正在播放攻击动画;
    public float 受击僵直时间=0.5f;
    public Vector2 攻击方向;
    public Dictionary<GameObject,bool> 攻击距离检测=new Dictionary<GameObject, bool>();
    public Dictionary<GameObject,bool> 追踪距离检测=new Dictionary<GameObject, bool>();
    void Start()
    {
        追踪或攻击玩家();
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
        }
    }
    public void 追踪或攻击玩家()
    {
        if(攻击距离检测.Count != 0)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.攻击,敌人状态);
            return;
        }
        if(追踪距离检测.Count == 0)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.待机,敌人状态);
        }
        else
        {
            float 距离 = float.MaxValue;
            GameObject 最近的玩家 = null;
            foreach (GameObject 玩家 in 追踪距离检测.Keys)
            {
                if(Vector2.Distance(transform.position,玩家.transform.position) < 距离)
                {
                    最近的玩家 = 玩家;
                    距离 = Vector2.Distance(transform.position,玩家.transform.position);
                }
            }
            敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.移动,敌人状态);
            transform.GetComponent<AIDestinationSetter>().target = 最近的玩家.transform;
        }
    }
    void 移动状态()
    {
        贴图.GetComponent<SpriteRenderer>().sprite = 原始贴图;
    }
    void 受击状态()
    {
        if(!是否播放过受击动画前半段)
        {
            是否播放过受击动画前半段 = true;
            if(贴图.GetComponent<播放一次帧动画>()!=null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(受击动画,1,受击动画.Frames.Count/2);
            else 贴图.AddComponent<播放一次帧动画>().播放序列帧(受击动画,1,受击动画.Frames.Count/2);
        }
        受击计时器 += Time.deltaTime;
        if(受击计时器>受击僵直时间)
        {
            if(是否播放过受击动画后半段)return;
            是否播放过受击动画后半段 = true;
            贴图.GetComponent<播放一次帧动画>().播放序列帧(受击动画,受击动画.Frames.Count/2+1,受击动画.Frames.Count-1,受击结束回调);
        }
    }
    public void 受击结束回调()
    {
        是否播放过受击动画前半段 = false;
        是否播放过受击动画后半段 = false;
        if(敌人状态==游戏所有类型的管理器.敌人状态.受击)
        {
            敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
            追踪或攻击玩家();
            
        }
    }
    void 死亡状态()
    {
        if(是否播放过死亡动画)return;
        是否播放过死亡动画 = true;
        transform.GetComponent<AIPath>().maxSpeed = 0;
        if(贴图.GetComponent<播放一次帧动画>()!=null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(死亡动画,1,死亡动画.Frames.Count-1,死亡结束回调);
        else 贴图.AddComponent<播放一次帧动画>().播放序列帧(死亡动画,1,死亡动画.Frames.Count-1,死亡结束回调);
    }
    void 死亡结束回调()
    {
        是否播放过死亡动画 = false;
        敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
        transform.GetComponent<AIPath>().maxSpeed = 敌人基本属性.移动速度;
        对象池.实例.回收(gameObject, 敌人类型);
    }
    void 攻击状态()
    {
        if(是否正在播放攻击动画)return;
        是否正在播放攻击动画 = true;
        if(贴图.GetComponent<播放一次帧动画>()!=null) 贴图.GetComponent<播放一次帧动画>().播放序列帧(攻击动画,1,攻击动画.Frames.Count-1,攻击结束回调,敌人基本属性.攻击速度);
        else 贴图.AddComponent<播放一次帧动画>().播放序列帧(攻击动画,1,攻击动画.Frames.Count-1,攻击结束回调);
    }
    void 攻击结束回调()
    {
        是否正在播放攻击动画 = false;
        敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
        追踪或攻击玩家();
    }
}
