using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

public class 翻滚 : MonoBehaviour
{
    Vector3 翻滚距离;
    public void _翻滚(Vector3 距离)
    {
        transform.Find("贴图").GetComponent<Animator>().enabled = false;
        翻滚距离 = 距离;
        transform.Find("贴图").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("猎人/猎人翻滚");
        if(transform.Find("贴图").GetComponent<SpriteRenderer>().flipX==true)
        {
            transform.Find("贴图").DOLocalRotate(new Vector3(-45,0,-360),0.5f,RotateMode.FastBeyond360).OnUpdate(()=>{
                transform.Find("贴图").localRotation = Quaternion.Euler(-45,0,transform.Find("贴图").localRotation.eulerAngles.z);
            });
        }
        else
        {
            transform.Find("贴图").DOLocalRotate(new Vector3(-45,0,360),0.5f,RotateMode.FastBeyond360).OnUpdate(()=>{
                transform.Find("贴图").localRotation = Quaternion.Euler(-45,0,transform.Find("贴图").localRotation.eulerAngles.z);
            });
        }
        transform.DOMove(transform.position+翻滚距离,0.5f).OnComplete(()=>{
            transform.Find("贴图").GetComponent<Animator>().enabled = true;
            GetComponent<人物控制器>().人物当前状态 = 游戏所有类型的管理器.人物状态.待机;
        });
    }
}