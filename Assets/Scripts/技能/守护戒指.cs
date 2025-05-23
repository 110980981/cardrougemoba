using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 守护戒指 : MonoBehaviour
{
    public GameObject 牌;
    public void 玩家免疫受到的伤害(object sender, 人物受击.受击前事件参数类 e)
    {
        e.伤害 = 0;
        e.击退方向 = Vector2.zero;
        牌.GetComponent<卡牌>().供外部调用的使用这张牌接口();
    }
    public void 初始化(GameObject 牌)
    {
        this.牌 = 牌;
        牌.GetComponent<卡牌>().这张牌使用事件 += 销毁;
        用于找全局变量的脚本.实例.玩家.GetComponent<人物受击>().受击前事件 += 玩家免疫受到的伤害;
    }
    public void 销毁(object sender, EventArgs e)
    {
        用于找全局变量的脚本.实例.玩家.GetComponent<人物受击>().受击前事件 -= 玩家免疫受到的伤害;
        对象池.实例.回收(gameObject, "守护戒指");
    }
}