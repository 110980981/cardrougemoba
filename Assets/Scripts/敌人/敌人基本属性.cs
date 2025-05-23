using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 敌人基本属性 : MonoBehaviour
{
    public 游戏所有类型的管理器.怪物类型 敌人类型;
    public 游戏所有类型的管理器.敌人状态 敌人状态;
    public float 最大血量;
    public float 血量;
    public int 移动速度;
    public float 攻击速度;
    public int 攻击力;
}