using UnityEngine;
using Cinemachine; // 导入 Cinemachine
using UnityEngine.Rendering; // 导入 Rendering

public class PerspectiveSwitcher : MonoBehaviour
{
    [Header("摄像机")]
    [SerializeField] private CinemachineVirtualCamera followCamera; // 拖入 PlayerFollowCamera
    [SerializeField] private float thirdPersonDistance = 6.0f; // 第三人称距离
    [SerializeField] private float firstPersonDistance = 0.1f; // 第一人称距离

    [Header("玩家模型 (指定模型对象)")]
    [SerializeField] private GameObject playerHeadObject; // 拖入 Head_Geo
    [SerializeField] private GameObject playerBodyObject; // 拖入 Body_Geo

    private Cinemachine3rdPersonFollow cameraBody;
    private bool isFirstPerson = false;

    // 存储模型的渲染器
    private SkinnedMeshRenderer headRenderer;
    private SkinnedMeshRenderer bodyRenderer;

    void Start()
    {
        // 从虚拟摄像机获取 "Body" 组件
        if (followCamera != null)
        {
            cameraBody = followCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        // 从指定的GameObject获取渲染器
        if (playerHeadObject != null) headRenderer = playerHeadObject.GetComponent<SkinnedMeshRenderer>();
        if (playerBodyObject != null) bodyRenderer = playerBodyObject.GetComponent<SkinnedMeshRenderer>();

        // 默认设置为第三人称
        SwitchToThirdPerson();
    }

    void Update()
    {
        // 为了简单，我们这里使用旧的输入方式来检测 "V" 键
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            SwitchView();
        }
    }

    void SwitchView()
    {
        if (cameraBody == null) return;

        if (isFirstPerson)
        {
            // 切换到第一人称
            cameraBody.CameraDistance = firstPersonDistance;
            SetModelVisibility(ShadowCastingMode.ShadowsOnly); // 隐藏模型（但保留阴影）
        }
        else
        {
            // 切换到第三人称
            SwitchToThirdPerson();
        }
    }

    void SwitchToThirdPerson()
    {
        cameraBody.CameraDistance = thirdPersonDistance;
        SetModelVisibility(ShadowCastingMode.On); // 显示模型
    }

    // 设置模型可见性
    void SetModelVisibility(ShadowCastingMode mode)
    {
        if (headRenderer != null) headRenderer.shadowCastingMode = mode;
        if (bodyRenderer != null) bodyRenderer.shadowCastingMode = mode;
        // 如果你还有其他部分（如头发、配件），也需要在这里添加
    }
}