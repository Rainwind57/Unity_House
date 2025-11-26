using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HouseFunction : MonoBehaviour
{

    private DigitalHuman digitalHuman;
    [Header("UI 绑定")]
    public Text environmentText;     // 用于显示环境指标
    public Text energyText;          // 用于显示能耗信息
    // Start is called before the first frame update
    void Start()
    {
        digitalHuman = GameObject.Find("PlayerArmature").GetComponent<DigitalHuman>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnvironment();
        CheckEnergyConsumption();
        CheckBehavior();
        CheckHealth();
        CheckSafety();
        UpdateUI();
    }

    #region 环境智控

    [Header("环境指标")]
    public float temperature = 25.0f;
    public float humidity = 50.0f;
    public float pm25 = 150.0f;
    public float pm10 = 200.0f;

    [Header("控制阈值")]
    public float pm25Threshold = 75.0f;
    public float targetTemperature = 26.0f;
    public float targetHumidity = 55.0f;

    [Header("设备状态")]
    public bool isAirFreshSystemOn = false;
    public bool isAirConditionerOn = false;

    void CheckEnvironment()
    {
        if (pm25 > pm25Threshold || pm10 > pm25Threshold * 1.5f)
        {
            if (!isAirFreshSystemOn)
            {
                ControlAirFreshSystem(true, "空气质量超标");
            }
        }
        else if (pm25 < pm25Threshold * 0.5f && pm10 < pm25Threshold)
        {
            if (isAirFreshSystemOn)
            {
                ControlAirFreshSystem(false, "空气质量恢复优良");
            }
        }

        if (temperature < targetTemperature - 1.0f || temperature > targetTemperature + 1.0f || 
            humidity < targetHumidity - 5.0f || humidity > targetHumidity + 5.0f)
        {
            if (!isAirConditionerOn)
            {
                ControlAirConditioner(true, "温湿度超限");
            }
            
            AdjustAirConditionerSettings();
        }
        else
        {
            if (isAirConditionerOn)
            {
                ControlAirConditioner(false, "温湿度稳定");
            }
        }
        
        SimulateEnvironmentChange();
    }

    void ControlAirFreshSystem(bool turnOn, string reason)
    {
        isAirFreshSystemOn = turnOn;
        Debug.Log($"新风系统：{(turnOn ? "开启" : "关闭")}。原因: {reason}");
    }

    void ControlAirConditioner(bool turnOn, string reason)
    {
        isAirConditionerOn = turnOn;
        Debug.Log($"空调：{(turnOn ? "开启" : "关闭")}。原因: {reason}");
    }

    void AdjustAirConditionerSettings()
    {
        if (!isAirConditionerOn) return;

        if (temperature > targetTemperature + 0.5f)
        {
            Debug.Log("空调模式：制冷");
        }
        else if (temperature < targetTemperature - 0.5f)
        {
            Debug.Log("空调模式：制热");
        }
    }

    void SimulateEnvironmentChange()
    {
        pm25 += 0.05f * Time.deltaTime; 
        pm10 += 0.06f * Time.deltaTime;
        
        if (isAirFreshSystemOn)
        {
            pm25 = Mathf.Lerp(pm25, 10f, Time.deltaTime * 0.1f);
            pm10 = Mathf.Lerp(pm10, 20f, Time.deltaTime * 0.1f);
        }
        
        if (isAirConditionerOn)
        {
            temperature = Mathf.Lerp(temperature, targetTemperature, Time.deltaTime * 0.05f);
            humidity = Mathf.Lerp(humidity, targetHumidity, Time.deltaTime * 0.05f);
        }
        
        pm25 = Mathf.Max(0, pm25);
        pm10 = Mathf.Max(0, pm10);
    }

    #endregion



    #region 能耗智控

    [Header("能耗智控")]
    public bool isEnergySavingMode = true;
    public float energyConsumption = 0.0f;
    public float maxEnergyConsumption = 100.0f;

    void CheckEnergyConsumption()
    {
        if (isEnergySavingMode)
        {
            if (isAirConditionerOn)
            {
                energyConsumption += 0.5f * Time.deltaTime;
                if (energyConsumption > maxEnergyConsumption)
                {
                    ControlAirConditioner(false, "能耗超标，关闭空调以节约能源");
                }
            }
        }
        else
        {
            if (isAirConditionerOn)
            {
                energyConsumption += 1.0f * Time.deltaTime;
            }
        }

        Debug.Log($"当前能耗: {energyConsumption:F2} / {maxEnergyConsumption}");
    }

    #endregion


    #region 智能监护

    [Header("智能监护")]
    public float sittingTime = 0.0f; 
    public float bathingTime = 0.0f; 
    public float maxSittingTime = 3600.0f; 
    public float maxBathingTime = 1800.0f; 

    void CheckBehavior()
    {
        if (digitalHuman.state == HumanState.Sitting)
        {
            sittingTime += Time.deltaTime;
            if (sittingTime > maxSittingTime)
            {
                TriggerAlarm("久坐时间过长，请注意活动！");
                sittingTime = 0.0f;
            }
        }
        else
        {
            sittingTime = 0.0f;
        }

        if (digitalHuman.state == HumanState.Bathing)
        {
            bathingTime += Time.deltaTime;
            if (bathingTime > maxBathingTime)
            {
                TriggerAlarm("久浴时间过长，请注意安全！");
                bathingTime = 0.0f;
            }
        }
        else
        {
            bathingTime = 0.0f;
        }
    }

    #endregion

    #region 健康监测


    [Header("健康预警阈值")]
    public float minHeartRate = 50.0f;
    public float maxHeartRate = 120.0f;
    public float minRespirationRate = 10.0f;
    public float maxRespirationRate = 24.0f;
    public float noMovementDuration = 300.0f; // 持续无体动报警阈值 (秒)

    private float noMovementTimer = 0.0f;

    void CheckHealth()
    {
        // 从 digitalHuman 组件读取实时数据
        float currentHeartRate = digitalHuman.heartRate;
        float currentRespirationRate = digitalHuman.respirationRate;
        float currentBodyMovement = digitalHuman.bodyMovement;

        if (currentHeartRate < minHeartRate || currentHeartRate > maxHeartRate)
        {
            TriggerAlarm($"心率异常！当前心率: {currentHeartRate:F0} 次/分钟");
        }

        if (currentRespirationRate < minRespirationRate || currentRespirationRate > maxRespirationRate)
        {
            TriggerAlarm($"呼吸异常！当前呼吸频率: {currentRespirationRate:F0} 次/分钟");
        }

        if (digitalHuman.state != HumanState.Sleeping)
        {
            if (currentBodyMovement < 0.1f)
            {
                noMovementTimer += Time.deltaTime;
                if (noMovementTimer > noMovementDuration)
                {
                    TriggerAlarm($"长时间无体动，可能发生意外！");
                    noMovementTimer = 0f;
                }
            }
            else
            {
                noMovementTimer = 0f;
            }
        }
        else
        {
            noMovementTimer = 0f;
        }
    }

    #endregion

    #region 安全防护

    [Header("安全防护")]
    public float smokeConcentration = 10.0f; // 烟雾浓度
    public float smokeThreshold = 100.0f; // 烟雾报警阈值
    public bool isSmokeAlarmOn = false;
    public bool areWindowsOpen = false;

    void CheckSafety()
    {
        // 模拟烟雾浓度变化，实际应用中应来自传感器
        SimulateSmokeChange();

        if (smokeConcentration > smokeThreshold)
        {
            if (!isSmokeAlarmOn)
            {
                ControlSmokeAlarm(true, "烟雾浓度过高");
            }
            if (!areWindowsOpen)
            {
                ControlWindows(true, "烟雾浓度过高，自动开窗通风");
            }
        }
        else if (smokeConcentration < smokeThreshold * 0.5f) // 浓度降到安全值以下
        {
            if (isSmokeAlarmOn)
            {
                ControlSmokeAlarm(false, "烟雾浓度恢复正常");
            }
            if (areWindowsOpen)
            {
                ControlWindows(false, "烟雾浓度恢复正常，关闭窗户");
            }
        }
    }

    void ControlSmokeAlarm(bool turnOn, string reason)
    {
        isSmokeAlarmOn = turnOn;
        Debug.Log($"[安全警报] 声光报警器：{(turnOn ? "开启" : "关闭")}。原因: {reason}");
    }

    void ControlWindows(bool open, string reason)
    {
        areWindowsOpen = open;
        Debug.Log($"窗户：{(open ? "自动打开" : "自动关闭")}。原因: {reason}");
    }

    void SimulateSmokeChange()
    {
        // 缓慢增加烟雾浓度来模拟潜在风险
        smokeConcentration += Time.deltaTime * 0.2f;

        // 如果窗户打开，烟雾浓度会快速下降
        if (areWindowsOpen)
        {
            smokeConcentration = Mathf.Lerp(smokeConcentration, 0f, Time.deltaTime * 0.5f);
        }
        smokeConcentration = Mathf.Max(0, smokeConcentration);
    }

    #endregion

    #region 一键呼叫

    void TriggerAlarm(string message)
    {
        Debug.Log($"[报警] {message}");
        
    }
    
    #endregion

    void UpdateUI()
    {

        if (environmentText != null)
        {
            environmentText.text = 
                $"环境指标\n" +
                $"温度: {temperature:F1}°C (目标: {targetTemperature:F1}°C)\n" +
                $"湿度: {humidity:F1}% (目标: {targetHumidity:F1}%)\n" +
                $"PM2.5: {pm25:F1} / PM10: {pm10:F1}\n" +
                $"新风系统: {(isAirFreshSystemOn ? "ON" : "OFF")}\n" +
                $"空调: {(isAirConditionerOn ? "ON" : "OFF")}";
        }
        

        if (energyText != null)
        {
            energyText.text = 
                $"能耗智控\n" +
                $"模式: {(isEnergySavingMode ? "节能模式" : "普通模式")}\n" +
                $"能耗: {energyConsumption:F2} / {maxEnergyConsumption:F2}";
        }

    }
}
