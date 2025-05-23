using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;

public class 牌库手牌墓地管理 : MonoBehaviour
{
    private static 牌库手牌墓地管理 _实例;
    public static 牌库手牌墓地管理 实例
    {
        get
        {
            if (_实例 == null)
            {
                _实例 = FindFirstObjectByType<牌库手牌墓地管理>();
                if (_实例 == null)
                {
                    GameObject 新对象 = new GameObject("牌库手牌墓地管理");
                    _实例 = 新对象.AddComponent<牌库手牌墓地管理>();
                }
            }
            return _实例;
        }
    }
    public List<GameObject> 牌库 = new List<GameObject>();
    public List<GameObject> 手牌 = new List<GameObject>();
    public List<GameObject> 墓地 = new List<GameObject>();
    public Transform 卡牌位置,卡牌间距,卡牌飞入的位置;
    public Canvas 卡牌界面;
    public float 抽卡间隔=5;
    float 抽卡计时器;
    public void 弃牌(GameObject 牌)
    {
        手牌.Remove(牌);
        墓地.Add(牌);
        展示手牌();
        牌.transform.position = Vector2.one * 3000;
    }
    public void 连续弃牌(List<GameObject> 牌)
    {
        for (int i = 0; i < 牌.Count; i++)
        {
            牌[i].transform.position = Vector2.one * 3000;
            手牌.Remove(牌[i]);
            墓地.Add(牌[i]);
        }
        展示手牌();
    }
    public void 加入手牌(GameObject 牌)
    {
        手牌.Add(牌);
        牌.GetComponent<卡牌>().Init_加入手牌的时候();
        展示手牌(true);
    }
    public void 加入牌库(int id)
    {
        GameObject 牌 = 对象池.实例.获取("卡牌","其他",卡牌界面.gameObject);
        牌.GetComponent<卡牌>().Init(id);
        牌库.Add(牌);
    }
    public void 抽卡()
    {
        if (牌库.Count == 0)
        {
            if(墓地.Count == 0)return;
            牌库 = 墓地;
            墓地 = new List<GameObject>();
            牌库洗牌();
        }
        加入手牌(牌库[0]);
        牌库.Remove(牌库[0]);
    }
    public void 牌库洗牌()
    {
        for (int i = 0; i < 牌库.Count; i++)
        {
            int 随机数 = Random.Range(0, 牌库.Count);
            GameObject 牌 = 牌库[i];
            牌库[i] = 牌库[随机数];
            牌库[随机数] = 牌;
        } 
    }
    public void 展示手牌(bool 是否有新牌加入手牌 = false)
    {
        for (int i = 0; i < 手牌.Count; i++)
        {
            Vector3 位置 = 卡牌位置.position;
            位置.x += (卡牌间距.position.x - 卡牌位置.position.x) * (i - 手牌.Count / 2);
            位置.y += (卡牌间距.position.y - 卡牌位置.position.y) * Mathf.Abs(i - 手牌.Count / 2);
            Vector3 旋转 = 卡牌位置.eulerAngles;
            旋转 += new Vector3(0, 0, -10) * (i - 手牌.Count / 2);
            if (是否有新牌加入手牌 && i + 1 == 手牌.Count)
            {
                手牌[i].transform.position = 卡牌飞入的位置.position;
                手牌[i].transform.DOJump(位置, 0.5f, 1, 0.5f);
                手牌[i].transform.DORotate(旋转, 0.5f);
            }
            else
            {
                手牌[i].transform.DOMove(位置, 0.2f);
                手牌[i].transform.DORotate(旋转, 0.2f);
            }
            手牌[i].GetComponent<卡牌>().初始位置 = 位置;
        }
    }
    void Update()
    {
        // 展示手牌();
        每隔一段时间自动抽卡();
    }
    void 每隔一段时间自动抽卡()
    {
        if(手牌.Count < 5)
        {
            抽卡计时器 += Time.deltaTime;
            if (抽卡计时器 >= 抽卡间隔)
            {
                抽卡();
                抽卡计时器 = 0;
            }
        }
        else 抽卡计时器 = 0;
    }
}