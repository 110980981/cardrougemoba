using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class 敌人控制器 : MonoBehaviour
{
    public string 敌人类型;
    public 游戏所有类型的管理器.敌人状态 敌人状态;
    public 敌人基本属性 敌人基本属性;
    public 敌人受击 敌人受击;
    public GameObject 图层;
    private bool 正在晃动;
    void Update()
    {
        switch (敌人状态)
        {
            case 游戏所有类型的管理器.敌人状态.移动:
                break;
            case 游戏所有类型的管理器.敌人状态.攻击:
                break;
            case 游戏所有类型的管理器.敌人状态.死亡:
                死亡状态();
                break;
            case 游戏所有类型的管理器.敌人状态.受击:
                受击状态();
                break;
        }
    }
    void 受击状态()
    {
        if (!正在晃动)
        {
            正在晃动 = true;
            图层.transform.DOShakePosition(0.3f, 0.3f).OnComplete(() => {
                正在晃动 = false;
                敌人状态 = 游戏所有类型的管理器.敌人状态.移动;
            });
        }
    }
    void 死亡状态()
    {
        对象池.实例.回收(gameObject, 敌人类型);
    }
}
