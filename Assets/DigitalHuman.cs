using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定义数字人的各种状态
/// </summary>
public enum HumanState
{
    Standing, // 站立
    Walking,  // 行走
    Sitting,  // 久坐
    Bathing,  // 洗浴
    Sleeping  // 睡眠
}

public class DigitalHuman : MonoBehaviour
{
    [Header("状态")]
    [Tooltip("当前状态")]
    public HumanState state = HumanState.Standing;

    [Header("健康指标")]
    public float heartRate = 75.0f;
    public float respirationRate = 18.0f;
    public float bodyMovement = 1.0f;

    private float stateChangeTimer = 0.0f;
    private float nextStateChangeTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化状态改变计时器
        nextStateChangeTime = Random.Range(20f, 40f);
    }

    // Update is called once per frame
    void Update()
    {
        // 模拟状态随时间变化
        stateChangeTimer += Time.deltaTime;
        if (stateChangeTimer > nextStateChangeTime)
        {
            ChangeState();
            stateChangeTimer = 0;
            nextStateChangeTime = Random.Range(30f, 60f); // 设置下一次状态改变的时间
        }

        // 根据当前状态模拟健康数据
        SimulateHealthData();
    }

    /// <summary>
    /// 随机改变数字人的状态
    /// </summary>
    void ChangeState()
    {
        // 从 HumanState 枚举中随机选择一个新状态
        var possibleStates = System.Enum.GetValues(typeof(HumanState));
        state = (HumanState)possibleStates.GetValue(Random.Range(0, possibleStates.Length));
        Debug.Log($"数字人状态变更为: {GetStateName(state)}");
    }

    /// <summary>
    /// 根据当前状态模拟健康数据
    /// </summary>
    void SimulateHealthData()
    {
        float heartRateBase = 75f;
        float respirationRateBase = 18f;
        float bodyMovementBase = 0.5f;

        switch (state)
        {
            case HumanState.Standing:
                heartRateBase = 75f;
                respirationRateBase = 18f;
                bodyMovementBase = 0.2f;
                break;
            case HumanState.Walking:
                heartRateBase = 95f;
                respirationRateBase = 20f;
                bodyMovementBase = 1.5f;
                break;
            case HumanState.Sitting:
                heartRateBase = 70f;
                respirationRateBase = 16f;
                bodyMovementBase = 0.1f;
                break;
            case HumanState.Bathing:
                heartRateBase = 80f;
                respirationRateBase = 19f;
                bodyMovementBase = 0.8f;
                break;
            case HumanState.Sleeping:
                heartRateBase = 60f;
                respirationRateBase = 14f;
                bodyMovementBase = 0.05f;
                break;
        }

        // 在基础值上增加一些随机波动，使其更真实
        heartRate = heartRateBase + Mathf.PingPong(Time.time * 0.5f, 5) - 2.5f;
        respirationRate = respirationRateBase + Mathf.PingPong(Time.time * 0.3f, 3) - 1.5f;
        bodyMovement = bodyMovementBase + Mathf.PingPong(Time.time * 1.5f, 0.1f) - 0.05f;
        bodyMovement = Mathf.Max(0, bodyMovement); // 确保体动不会是负数
    }

    /// <summary>
    /// 获取状态枚举对应的名称
    /// </summary>
    string GetStateName(HumanState stateEnum)
    {
        switch (stateEnum)
        {
            case HumanState.Standing: return "站立 (Standing)";
            case HumanState.Walking: return "行走 (Walking)";
            case HumanState.Sitting: return "久坐 (Sitting)";
            case HumanState.Bathing: return "洗浴 (Bathing)";
            case HumanState.Sleeping: return "睡眠 (Sleeping)";
            default: return "未知";
        }
    }
}