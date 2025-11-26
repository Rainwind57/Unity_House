using UnityEngine;
using System.Collections; // 用于协程

public class DoorController : MonoBehaviour
{
    // 门是否是打开状态
    private bool isOpen = false;

    // 目标旋转
    private Quaternion targetRotation;

    // 门的关闭状态（初始）旋转
    private Quaternion closeRotation;

    // 设置门打开时的Y轴旋转角度
    [Header("旋转设置")]
    public float openAngle = 90.0f;

    // 旋转速度
    public float smoothSpeed = 2.0f;

    void Start()
    {
        // 保存初始（关闭）的旋转状态
        closeRotation = transform.rotation;
        // 设置目标旋转为关闭状态
        targetRotation = closeRotation;
    }

    void Update()
    {
        // 平滑地插值旋转到目标角度
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // 这是一个公共方法，将由玩家脚本调用
    public void Interact()
    {
        // 切换状态
        isOpen = !isOpen;

        if (isOpen)
        {
            // 如果是打开，计算并设置打开的旋转
            Quaternion openRot = closeRotation * Quaternion.Euler(0, openAngle, 0);
            targetRotation = openRot;
        }
        else
        {
            // 如果是关闭，设置回初始的关闭旋转
            targetRotation = closeRotation;
        }
    }
}