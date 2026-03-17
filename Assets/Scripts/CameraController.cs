using UnityEngine;

/// <summary>
/// 相机控制器
/// 实现轨道相机、缩放、聚焦等功能
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("目标")]
    public Transform target;

    [Header("距离设置")]
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float distance = 5f;

    [Header("旋转设置")]
    public float rotationSpeed = 5f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    [Header("缩放设置")]
    public float zoomSpeed = 2f;
    public float smoothTime = 0.1f;

    [Header("自动旋转")]
    public bool autoRotate = false;
    public float autoRotateSpeed = 10f;

    private float currentX = 0f;
    private float currentY = 20f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float currentDistance;

    void Start()
    {
        currentDistance = distance;
        
        // 设置初始位置
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            currentX = angles.y;
            currentY = angles.x;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 处理输入
        HandleInput();

        // 计算相机位置
        UpdateCameraPosition();
    }

    /// <summary>
    /// 处理输入
    /// </summary>
    private void HandleInput()
    {
        // 鼠标右键拖动旋转
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            currentX += mouseX;
            currentY -= mouseY;
            currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

            autoRotate = false;
        }

        // 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // 触摸旋转
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * rotationSpeed * 0.01f;
                currentY -= touch.deltaPosition.y * rotationSpeed * 0.01f;
                currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

                autoRotate = false;
            }
        }

        // 双指缩放
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            distance -= difference * 0.01f * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // 自动旋转
        if (autoRotate)
        {
            currentX += autoRotateSpeed * Time.deltaTime;
        }

        // 恢复自动旋转（3秒无操作后）
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Input.touchCount == 0)
        {
            // 可以添加计时器逻辑恢复自动旋转
        }
    }

    /// <summary>
    /// 更新相机位置
    /// </summary>
    private void UpdateCameraPosition()
    {
        // 平滑过渡
        currentDistance = Mathf.Lerp(currentDistance, distance, smoothTime);

        // 计算位置
        Vector3 direction = new Vector3(0, 0, -currentDistance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        transform.position = target.position + rotation * direction;
        transform.LookAt(target.position);
    }

    /// <summary>
    /// 聚焦到目标
    /// </summary>
    public void FocusOnTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// 设置距离
    /// </summary>
    public void SetDistance(float newDistance)
    {
        distance = Mathf.Clamp(newDistance, minDistance, maxDistance);
    }

    /// <summary>
    /// 重置相机位置
    /// </summary>
    public void ResetCamera()
    {
        currentX = 0f;
        currentY = 20f;
        distance = 5f;
        currentDistance = distance;
        autoRotate = true;
    }

    /// <summary>
    /// 切换自动旋转
    /// </summary>
    public void ToggleAutoRotate()
    {
        autoRotate = !autoRotate;
    }
}
