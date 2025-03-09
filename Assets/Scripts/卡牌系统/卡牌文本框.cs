using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class 卡牌文本框 : MonoBehaviour
{
    private static 卡牌文本框 _instance;
    public static 卡牌文本框 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<卡牌文本框>();
            }
            return _instance;
        }
    }
    public GameObject 文本框;
    void 跟随鼠标()
    {
        Vector3 鼠标位置 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        鼠标位置.z = 0;
        transform.position = 鼠标位置;
    }
    public void 显示文本(string 文本)
    {
        transform.GetComponent<CanvasGroup>().alpha = 1;
        文本框.GetComponent<TextMeshProUGUI>().text = 文本;
        MonoMgr.GetInstance().AddUpdateListener(跟随鼠标);
    }
    public void 隐藏文本()
    {
        transform.GetComponent<CanvasGroup>().alpha = 0;
        MonoMgr.GetInstance().RemoveUpdateListener(跟随鼠标);
        transform.position = Vector3.one*3000;
    }
}