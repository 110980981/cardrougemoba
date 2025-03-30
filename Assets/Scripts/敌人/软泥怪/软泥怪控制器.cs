using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class 软泥怪控制器 : MonoBehaviour
{
    public Dictionary<GameObject,bool> 攻击距离检测=new Dictionary<GameObject, bool>();
    public Dictionary<GameObject,bool> 追踪距离检测=new Dictionary<GameObject, bool>();
    void Start()
    {
        追踪玩家();
    }
    public void 追踪玩家()
    {
        if(追踪距离检测.Count == 0)
        {
            transform.GetComponent<敌人控制器>().敌人状态 = 游戏所有类型的管理器.敌人状态.待机;
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
            transform.GetComponent<敌人控制器>().敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
            transform.GetComponent<AIDestinationSetter>().target = 最近的玩家.transform;
        }
    }
}