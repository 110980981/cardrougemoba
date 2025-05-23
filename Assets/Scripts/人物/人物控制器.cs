using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GifImporter;
using UnityEngine;

public class 人物控制器 : MonoBehaviour
{
    private 游戏所有类型的管理器.人物状态 _人物当前状态=游戏所有类型的管理器.人物状态.待机;
    public event EventHandler 人物状态改变事件;
    public 游戏所有类型的管理器.人物状态 人物当前状态
    {
        get { return _人物当前状态; }
        set
        {
            if (人物当前状态 != value)
            {
                if (人物状态改变事件 != null) 人物状态改变事件(this, EventArgs.Empty);
            }
            _人物当前状态 = value;
            switch (value)
            {
                case 游戏所有类型的管理器.人物状态.移动:
                    break;
                case 游戏所有类型的管理器.人物状态.攻击:
                    break;
                case 游戏所有类型的管理器.人物状态.死亡:
                    break;
                case 游戏所有类型的管理器.人物状态.受击:
                    break;
                case 游戏所有类型的管理器.人物状态.冲撞:
                    break;
                default:
                    break;
            }
        }
    }
    private Vector3 位移向量;
    public 人物移动 人物移动;
    public 人物键位 人物键位;
    public 人物属性 人物属性;
    public 人物技能 人物技能;
    public GameObject 人物贴图;
    public GameObject A星寻路点;
    public GameObject 跟随主角摄像机;
    public bool 是否为该客户端操作的角色=true;
    void Start()
    {
        血条管理.实例.初始化血量(人物属性.血量);
    }
    void Update()
    {
        移动();
        人物状态判断();
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
            case 游戏所有类型的管理器.人物状态.冲撞:
                break;
            case 游戏所有类型的管理器.人物状态.待机:
                待机状态();
                break;
            default:
                break;
        }
    }
    void 人物状态判断()
    {
        if(人物当前状态!=游戏所有类型的管理器.人物状态.移动&&人物当前状态!=游戏所有类型的管理器.人物状态.待机)return;
        if(位移向量==Vector3.zero)
        {
            人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.待机,人物当前状态);
        }
        else
        {
            人物当前状态 = 游戏所有类型的管理器.人物状态过度判断(游戏所有类型的管理器.人物状态.移动,人物当前状态);
        }
    }
    void 移动()
    {
        位移向量 = Vector3.zero;
        if (Input.GetKey(人物键位._人物键位["上"])) 位移向量 += Vector3.up;
        if (Input.GetKey(人物键位._人物键位["下"])) 位移向量 += Vector3.down;
        if (Input.GetKey(人物键位._人物键位["左"])) 位移向量 += Vector3.left;
        if (Input.GetKey(人物键位._人物键位["右"])) 位移向量 += Vector3.right;
        //反转人物朝向
        if(位移向量.x>0)
        {
            transform.Find("贴图").GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(位移向量.x<0)
        {
            transform.Find("贴图").GetComponent<SpriteRenderer>().flipX = false;
        }
        人物移动.移动(位移向量, 人物属性.移动速度);
    }
    void 移动状态()
    {
        if(transform.Find("贴图").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("移动动画"))
        {
            transform.Find("贴图").GetComponent<Animator>().SetInteger("状态参数",0);
            return;
        }
        transform.Find("贴图").GetComponent<Animator>().SetInteger("状态参数",1);
    }
    void 待机状态()
    {
        if(transform.Find("贴图").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("待机动画"))
        {
            transform.Find("贴图").GetComponent<Animator>().SetInteger("状态参数",0);
            return;
        }
        transform.Find("贴图").GetComponent<Animator>().SetInteger("状态参数",2);
    }
}