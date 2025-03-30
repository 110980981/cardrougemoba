using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class 游戏所有类型的管理器
{
    public enum 敌人类型
    {
        木桩
    }
    public enum 技能类型
    {
        强化射击
    }
    public enum 敌人状态
    {
        移动,
        攻击,
        死亡,
        受击,
        待机
    }
    public enum 人物状态
    {
        移动,
        攻击,
        死亡,
        受击,
        冲撞,
        待机
    }
}
