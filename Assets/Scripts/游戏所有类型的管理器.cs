using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class 游戏所有类型的管理器
{
    public enum 怪物类型
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
        待机,
        被控制
    }
    public enum 人物状态
    {
        移动,
        攻击,
        死亡,
        受击,
        冲撞,
        待机,
        冲撞后跳
    }
    public static 游戏所有类型的管理器.人物状态 人物状态过度判断(游戏所有类型的管理器.人物状态 即将过渡到的状态, 游戏所有类型的管理器.人物状态 当前状态)
    {
        if(当前状态 == 游戏所有类型的管理器.人物状态.死亡)return 游戏所有类型的管理器.人物状态.死亡;
        return 即将过渡到的状态;
    }
    public static 游戏所有类型的管理器.敌人状态 敌人状态过度判断(游戏所有类型的管理器.敌人状态 即将过渡到的状态, 游戏所有类型的管理器.敌人状态 当前状态,bool 强制转换 = false)
    {
        if(即将过渡到的状态 == 游戏所有类型的管理器.敌人状态.死亡||当前状态==游戏所有类型的管理器.敌人状态.死亡)return 游戏所有类型的管理器.敌人状态.死亡;
        if(强制转换)return 即将过渡到的状态;
        if (当前状态 == 游戏所有类型的管理器.敌人状态.被控制 || 即将过渡到的状态 == 游戏所有类型的管理器.敌人状态.被控制) return 游戏所有类型的管理器.敌人状态.被控制;
        if (当前状态 == 游戏所有类型的管理器.敌人状态.受击) return 游戏所有类型的管理器.敌人状态.受击;
        if(当前状态 == 游戏所有类型的管理器.敌人状态.攻击)return 游戏所有类型的管理器.敌人状态.攻击;
        return 即将过渡到的状态;
    }
}
