using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class 敌人受击 : MonoBehaviour
{
    public 敌人控制器 敌人控制器;
    public 敌人基本属性 敌人基本属性;
    public void 受击(int 攻击力,Vector2 击退方向)
    {
        敌人基本属性.血量 -= 攻击力;
        transform.DOJump(transform.position + (Vector3)击退方向.normalized*0.1f,0.1f,1, 0.1f);
        敌人控制器.敌人状态 = 游戏所有类型的管理器.敌人状态.受击;
        判断死亡();
    }
    public void 判断死亡()
    {
        if (敌人基本属性.血量 <= 0)
        {
            敌人控制器.敌人状态 = 游戏所有类型的管理器.敌人状态.死亡;
        }
    }
}
