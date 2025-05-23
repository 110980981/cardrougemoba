using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

public class 地牢生成器 : MonoBehaviour
{
    // 房间信息结构体
    public struct 房间信息
    {
        public BoundsInt 边界;  // 房间边界
        public string 类型;     // 房间类型（交互/战斗/特殊）

        public 房间信息(BoundsInt 边界, string 类型)
        {
            this.边界 = 边界;
            this.类型 = 类型;
        }
    }

    [Header("基本设置")]
    public Tilemap 瓦片地图;
    public TileBase 交互房间地砖;
    public TileBase 战斗房间地砖;
    public TileBase 特殊房间地砖;
    public GameObject 墙壁砖块;
    public TileBase 走廊;
    public TileBase 墙壁瓦片;
    public int 地牢宽度 = 50;
    public int 地牢高度 = 50;

    [Header("房间设置")]
    public int 最小房间尺寸 = 5;
    public int 最大房间尺寸 = 10;
    public int 房间数量 = 10;
    public int 最小房间间距 = 2;

    [Header("走廊设置")]
    public int 走廊宽度 = 1;
    public float 转弯概率 = 0.3f;
    private List<房间信息> 房间列表 = new List<房间信息>();
    private string[,] 地图数组;

    void Start()
    {
        瓦片地图.ClearAllTiles();
        生成地牢();
        StartCoroutine(过一帧等碰撞体刷出来再扫描());
    }
    IEnumerator 过一帧等碰撞体刷出来再扫描()
    {
        // 等待一帧确保物理数据更新
        yield return null;
        Physics2D.SyncTransforms();  // 强制同步物理变换
        AstarPath.active.Scan();
    }
    void 生成地牢()
    {
        地图数组 = new string[地牢宽度, 地牢高度];
        for (int x = 0; x < 地牢宽度; x++)
        {
            for (int y = 0; y < 地牢高度; y++)
            {
                地图数组[x, y] = "墙"; // 初始化为墙
            }
        }

        // 1. 生成随机房间
        生成房间();

        // 2. 连接所有房间
        连接房间();

        // 3. 将地图数组渲染到瓦片地图
        生成墙壁();
    }

    void 生成房间()
    {
        for (int i = 0; i < 房间数量; i++)
        {
            int 房间宽 = Random.Range(最小房间尺寸, 最大房间尺寸 + 1);
            int 房间高 = Random.Range(最小房间尺寸, 最大房间尺寸 + 1);

            // 尝试放置房间最多100次
            for (int 尝试次数 = 0; 尝试次数 < 100; 尝试次数++)
            {
                int 房间X = Random.Range(1, 地牢宽度 - 房间宽 - 1);
                int 房间Y = Random.Range(1, 地牢高度 - 房间高 - 1);

                BoundsInt 新房间边界 = new BoundsInt(房间X, 房间Y, 0, 房间宽, 房间高, 0);

                // 检查新房间是否与其他房间重叠或太近
                bool 有效位置 = true;
                foreach (var 现有房间 in 房间列表)
                {
                    if (是否房间重叠或过近(新房间边界, 现有房间.边界))
                    {
                        有效位置 = false;
                        break;
                    }
                }

                if (有效位置)
                {
                    // 创建带类型的房间
                    string 房间类型 = 获取随机房间类型();
                    房间列表.Add(new 房间信息(新房间边界, 房间类型));

                    // 填充房间区域
                    for (int x = 新房间边界.x; x < 新房间边界.x + 新房间边界.size.x; x++)
                    {
                        for (int y = 新房间边界.y; y < 新房间边界.y + 新房间边界.size.y; y++)
                        {
                            地图数组[x, y] = "地板";
                            switch (房间类型)
                            {
                                case "交互":
                                    瓦片地图.SetTile(new Vector3Int(x, y, 0), 交互房间地砖);
                                    break;
                                case "战斗":
                                    瓦片地图.SetTile(new Vector3Int(x, y, 0), 战斗房间地砖);
                                    break;
                                case "特殊":
                                    瓦片地图.SetTile(new Vector3Int(x, y, 0), 特殊房间地砖);
                                    break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }

    // 随机分配房间类型
    string 获取随机房间类型()
    {
        float 随机值 = Random.value;
        if (随机值 < 0.35f) return "交互";  // 35%概率
        if (随机值 < 0.75f) return "战斗";  // 40%概率
        return "特殊";                    // 25%概率
    }

    bool 是否房间重叠或过近(BoundsInt 房间1, BoundsInt 房间2)
    {
        // 扩展房间边界以包含最小距离
        BoundsInt 扩展边界 = new BoundsInt(
            房间1.x - 最小房间间距,
            房间1.y - 最小房间间距,
            0,
            房间1.size.x + 最小房间间距 * 2,
            房间1.size.y + 最小房间间距 * 2,
            0);

        // 手动实现重叠检测
        bool X重叠 = 扩展边界.x < 房间2.x + 房间2.size.x &&
                   扩展边界.x + 扩展边界.size.x > 房间2.x;

        bool Y重叠 = 扩展边界.y < 房间2.y + 房间2.size.y &&
                   扩展边界.y + 扩展边界.size.y > 房间2.y;

        return X重叠 && Y重叠;
    }

    void 连接房间()
    {
        if (房间列表.Count < 2) return;

        // 使用队列连接所有房间
        Queue<房间信息> 待连接房间 = new Queue<房间信息>(房间列表);
        房间信息 当前房间 = 待连接房间.Dequeue();

        while (待连接房间.Count > 0)
        {
            房间信息 下一个房间 = 待连接房间.Dequeue();

            // 获取两个房间的中心点
            Vector2Int 当前中心 = new Vector2Int(
                当前房间.边界.x + 当前房间.边界.size.x / 2,
                当前房间.边界.y + 当前房间.边界.size.y / 2);

            Vector2Int 下一个中心 = new Vector2Int(
                下一个房间.边界.x + 下一个房间.边界.size.x / 2,
                下一个房间.边界.y + 下一个房间.边界.size.y / 2);

            // 创建走廊
            创建走廊(当前中心, 下一个中心);

            当前房间 = 下一个房间;
        }
    }

    void 创建走廊(Vector2Int 起点, Vector2Int 终点)
    {
        Vector2Int 当前位置 = 起点;

        // 随机决定先走水平还是垂直
        bool 水平优先 = Random.value > 0.5f;

        while (当前位置 != 终点)
        {
            // 随机决定是否转弯（如果不在直线上）
            if (当前位置.x != 终点.x && 当前位置.y != 终点.y && Random.value < 转弯概率)
            {
                水平优先 = !水平优先;
            }

            // 移动一步
            if (水平优先 && 当前位置.x != 终点.x)
            {
                当前位置.x += (终点.x > 当前位置.x) ? 1 : -1;
            }
            else if (当前位置.y != 终点.y)
            {
                当前位置.y += (终点.y > 当前位置.y) ? 1 : -1;
            }
            else if (当前位置.x != 终点.x)
            {
                当前位置.x += (终点.x > 当前位置.x) ? 1 : -1;
            }

            // 创建走廊（考虑宽度）
            for (int 偏移 = -走廊宽度 / 2; 偏移 <= 走廊宽度 / 2; 偏移++)
            {
                int x坐标 = 当前位置.x + (走廊宽度 % 2 == 0 ? 偏移 : 偏移);
                int y坐标 = 当前位置.y;

                if (x坐标 >= 0 && x坐标 < 地牢宽度 && y坐标 >= 0 && y坐标 < 地牢高度 && (地图数组[x坐标, y坐标] == "墙" || 地图数组[x坐标, y坐标] == null))
                {
                    地图数组[x坐标, y坐标] = "走廊";
                    瓦片地图.SetTile(new Vector3Int(x坐标, y坐标, 0), 走廊);
                }

                // 宽走廊垂直填充
                if (走廊宽度 > 1)
                {
                    x坐标 = 当前位置.x;
                    y坐标 = 当前位置.y + (走廊宽度 % 2 == 0 ? 偏移 : 偏移);

                    if (x坐标 >= 0 && x坐标 < 地牢宽度 && y坐标 >= 0 && y坐标 < 地牢高度)
                    {
                        地图数组[x坐标, y坐标] = "走廊";
                        瓦片地图.SetTile(new Vector3Int(x坐标, y坐标, 0), 走廊);
                    }
                }
            }
        }
    }

    void 生成墙壁()
    {
        // 绘制地板和墙壁
        for (int x = 0; x < 地牢宽度; x++)
        {
            for (int y = 0; y < 地牢高度; y++)
            {
                if (地图数组[x, y] == "地板" || 地图数组[x, y] == "走廊") // 如果是地板
                {
                    // 自动添加周围墙壁
                    for (int 相邻X = x - 1; 相邻X <= x + 1; 相邻X++)
                    {
                        for (int 相邻Y = y - 1; 相邻Y <= y + 1; 相邻Y++)
                        {
                            if (相邻X >= 0 && 相邻X < 地牢宽度 &&
                                相邻Y >= 0 && 相邻Y < 地牢高度 &&
                                地图数组[相邻X, 相邻Y] == "墙")
                            {
                                瓦片地图.SetTile(new Vector3Int(相邻X, 相邻Y, 0), 墙壁瓦片);
                            }
                        }
                    }
                }
            }
        }
    }
}