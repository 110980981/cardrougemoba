using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class 攻击碰撞盒检测 : MonoBehaviour
{
    // 配置参数
    [SerializeField] private float _syncInterval = 0.2f; // 同步检测间隔

    // 运行时数据
    private 敌人控制器 _controller;
    private readonly HashSet<GameObject> _currentTargets = new HashSet<GameObject>();
    private Collider2D[] _overlapBuffer = new Collider2D[10];
    private float _lastSyncTime;
    private ContactFilter2D _targetLayers;

    private void Awake()
    {
        // 初始化验证
        if (transform.parent == null)
        {
            Debug.LogError("追踪检测器必须挂载在有父物体的对象上", this);
            enabled = false;
            return;
        }

        // 缓存控制器引用
        if (!transform.parent.TryGetComponent(out _controller))
        {
            Debug.LogError("父物体上未找到敌人控制器", this);
            enabled = false;
            return;
        }
        _controller = transform.parent.GetComponent<敌人控制器>();
        // 确保字典初始化
        _controller.攻击碰撞盒内有没有人 ??= new Dictionary<GameObject, bool>();
    }

    private void Update()
    {
        // 按间隔执行物理同步检测
        if (Time.time - _lastSyncTime >= _syncInterval)
        {
            SyncPhysicsDetection();
            _lastSyncTime = Time.time;
        }
    }

    private void SyncPhysicsDetection()
    {
        if (transform.parent.tag == "敌人") _targetLayers.SetLayerMask(LayerMask.GetMask("Player"));
        else if (transform.parent.tag == "Player") _targetLayers.SetLayerMask(LayerMask.GetMask("敌人"));
        // 1. 物理检测周围目标
        int count = Physics2D.OverlapCollider(
            GetComponent<Collider2D>(),
            _targetLayers,
            _overlapBuffer
        );

        // 2. 记录新检测到的目标
        var newTargets = new HashSet<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject target = _overlapBuffer[i].gameObject;
            if (IsValidTarget(target))
            {
                newTargets.Add(target);
                if (!_currentTargets.Contains(target))
                {
                    OnTargetEnter(target);
                }
            }
        }

        // 3. 处理离开的目标
        foreach (var oldTarget in _currentTargets)
        {
            if (oldTarget != null && !newTargets.Contains(oldTarget))
            {
                OnTargetExit(oldTarget);
            }
        }

        _currentTargets.Clear();
        _currentTargets.UnionWith(newTargets);
    }

    private bool IsValidTarget(GameObject target)
    {
        bool isPlayer = target.CompareTag("Player") && transform.parent.CompareTag("敌人");
        bool isEnemy = target.CompareTag("敌人") && transform.parent.CompareTag("Player");
        return isPlayer || isEnemy;
    }

    private void OnTargetEnter(GameObject target)
    {
        Debug.Log($"进入追踪范围: {target.name}", this);
        _controller.攻击碰撞盒内有没有人[target] = true;
        _controller.追踪或攻击玩家();
    }

    private void OnTargetExit(GameObject target)
    {
        Debug.Log($"离开追踪范围: {target.name}", this);
        if (_controller.攻击碰撞盒内有没有人.ContainsKey(target))
        {
            _controller.攻击碰撞盒内有没有人.Remove(target);
        }
        _controller.追踪或攻击玩家();
    }

    private void OnDisable()
    {
        // 禁用时清理所有目标
        foreach (var target in _currentTargets)
        {
            if (target != null && _controller.攻击碰撞盒内有没有人.ContainsKey(target))
            {
                _controller.攻击碰撞盒内有没有人.Remove(target);
            }
        }
        _currentTargets.Clear();
        _controller.追踪或攻击玩家();
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 可视化检测范围
        if (TryGetComponent(out Collider2D collider))
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
            if (collider is BoxCollider2D box)
            {
                Gizmos.DrawCube(transform.position + (Vector3)box.offset, box.size);
            }
            else if (collider is CircleCollider2D circle)
            {
                Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);
            }
        }
    }
    #endif
}