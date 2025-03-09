using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using System.Data;
using System.IO;
using System.Text;

public class 根据id获取卡牌信息 : MonoBehaviour
{
    private static 根据id获取卡牌信息 _实例;
    public static 根据id获取卡牌信息 实例
    {
        get
        {
            if (_实例 == null)
            {
                _实例 = FindFirstObjectByType<根据id获取卡牌信息>();
                if (_实例 == null)
                {
                    GameObject 新对象 = new GameObject("根据id获取卡牌信息");
                    _实例 = 新对象.AddComponent<根据id获取卡牌信息>();
                }
            }
            return _实例;
        } 
    }
    string[][] 卡牌信息;
    void Awake()
    {
        string filePath = Application.streamingAssetsPath + "/卡牌信息.csv";
        string[] fileLines = File.ReadAllLines(filePath, Encoding.UTF8);
        卡牌信息 = new string[fileLines.Length][];
        for (int i = 0; i < fileLines.Length; i++)
        {
            string[] 行信息 = fileLines[i].Split(',');
            卡牌信息[i] = new string[行信息.Length];
            for (int j = 0; j < 行信息.Length; j++)
            {
                卡牌信息[i][j] = 行信息[j];
            }
        }
    }
    public string[] 获取卡牌信息(int id)
    {
        return 卡牌信息[id];
    }
}