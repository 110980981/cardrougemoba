using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Collections;
using System;

public class 敌人受击 : MonoBehaviour,受击接口
{
    public 敌人控制器 敌人控制器;
    public 敌人基本属性 敌人基本属性;
    public void 受击(Vector2 击退方向,object 攻击力)
    {
        敌人基本属性.血量 -= Convert.ToSingle(攻击力);
        transform.DOJump(transform.position + (Vector3)击退方向.normalized*0.1f,0.1f,1, 0.1f);
        敌人控制器.敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.受击, 敌人控制器.敌人状态);
        判断死亡();
    }
    public void 判断死亡()
    {
        if (敌人基本属性.血量 <= 0.0000001f)
        {
            敌人控制器.敌人状态 = 游戏所有类型的管理器.敌人状态过度判断(游戏所有类型的管理器.敌人状态.死亡, 敌人控制器.敌人状态);
        }
    }
}