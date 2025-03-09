using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 人物控制器 : MonoBehaviour
{
    public 游戏所有类型的管理器.人物状态 人物当前状态=游戏所有类型的管理器.人物状态.移动;
    private Vector3 位移向量;
    public 人物移动 人物移动;
    public 人物键位 人物键位;
    public 人物属性 人物属性;
    public 人物技能 人物技能;
    void Update()
    {
        移动状态();
        switch (人物当前状态)
        {
            case 游戏所有类型的管理器.人物状态.移动:
                移动状态();
                break;
            case 游戏所有类型的管理器.人物状态.攻击:
                break;
            case 游戏所有类型的管理器.人物状态.死亡:
                break;
            case 游戏所有类型的管理器.人物状态.受击:
                break;
        }
    }
    void 移动状态()
    {
        位移向量 = Vector3.zero;
        if (Input.GetKey(人物键位._人物键位["上"])) 位移向量 += Vector3.up;
        if (Input.GetKey(人物键位._人物键位["下"])) 位移向量 += Vector3.down;
        if (Input.GetKey(人物键位._人物键位["左"])) 位移向量 += Vector3.left;
        if (Input.GetKey(人物键位._人物键位["右"])) 位移向量 += Vector3.right;
        人物移动.移动(位移向量, 人物属性.移动速度);
    }
}