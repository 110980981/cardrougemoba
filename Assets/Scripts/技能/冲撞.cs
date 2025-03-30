using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GifImporter;
using UnityEngine;
using UnityEngine.U2D;

public class 冲撞 : MonoBehaviour
{
    SpriteAtlas 冲撞动画;
    int 冲撞伤害;
    Vector3 冲撞距离;
    public void _冲撞(Vector3 距离,int 伤害)
    {
        transform.Find("贴图").GetComponent<Animator>().enabled = false;
        冲撞伤害 = 伤害;
        冲撞距离 = 距离;
        if(冲撞动画 == null)冲撞动画 = Resources.Load<SpriteAtlas>("猎人/猎人冲撞");
        if(transform.Find("贴图").GetComponent<播放序列帧_图集版本>()==null)transform.Find("贴图").gameObject.AddComponent<播放序列帧_图集版本>();
        transform.Find("贴图").GetComponent<播放序列帧_图集版本>().播放序列帧(冲撞动画,0,10,"猎人冲撞_",()=>预备冲撞动画之后的回调(冲撞距离));
    }
    public void 预备冲撞动画之后的回调(Vector3 冲撞距离)
    {
        transform.Find("贴图").GetComponent<播放序列帧_图集版本>().播放序列帧(冲撞动画,11,11,"猎人冲撞_",()=>{
            transform.DOMove(transform.position+冲撞距离,0.5f).OnComplete(()=>{
                冲撞结束动画();
            });
        });
    }
    void 冲撞结束动画()
    {
        transform.Find("贴图").GetComponent<播放序列帧_图集版本>().播放序列帧(冲撞动画,12,15,"猎人冲撞_",()=>{
            transform.Find("贴图").GetComponent<Animator>().enabled = true;
            GetComponent<人物控制器>().人物当前状态 = 游戏所有类型的管理器.人物状态.待机;
        });
    }
    bool 只能冲撞一次 = false;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(只能冲撞一次)return;
        if(collision.gameObject.tag == "敌人")
        {
            只能冲撞一次 = true;
            collision.gameObject.GetComponent<敌人受击>().受击(冲撞伤害,冲撞距离);
        }
    }
}