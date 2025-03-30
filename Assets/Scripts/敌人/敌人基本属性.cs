using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 敌人基本属性 : MonoBehaviour
{
    public 游戏所有类型的管理器.敌人类型 敌人类型;
    public 游戏所有类型的管理器.敌人状态 敌人状态;
    public int 血量;
    public int 移动速度;
    public float 攻击速度;
}