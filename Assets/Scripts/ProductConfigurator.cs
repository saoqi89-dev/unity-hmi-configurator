using UnityEngine;
using System.Collections;

/// <summary>
/// 产品配置器核心控制器
/// 功能：材质切换、模型旋转、配置管理
/// </summary>
public class ProductConfigurator : MonoBehaviour
{
    [Header("产品模型")]
    public Transform productModel;

    [Header("材质配置")]
    public Material[] materials;
    public string[] materialNames;

    [Header("旋转设置")]
    public float rotateSpeed = 50f;
    public bool autoRotate = true;

    private int currentMaterialIndex = 0;
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    private MeshRenderer[] modelRenderers;

    void Start()
    {
        InitializeModel();
        ApplyMaterial(0);
        
        if (autoRotate)
        {
            StartCoroutine(AutoRotateRoutine());
        }
    }

    void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// 初始化模型渲染器
    /// </summary>
    private void InitializeModel()
    {
        if (productModel == null)
        {
            Debug.LogWarning("[ProductConfigurator] 未设置产品模型");
            return;
        }

        modelRenderers = productModel.GetComponentsInChildren<MeshRenderer>();
        Debug.Log($"[ProductConfigurator] 找到 {modelRenderers.Length} 个渲染器");
    }

    /// <summary>
    /// 处理输入
    /// </summary>
    private void HandleInput()
    {
        // 鼠标/触摸开始
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
            autoRotate = false;
        }

        // 鼠标/触摸结束
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            // 3秒后恢复自动旋转
            Invoke(nameof(ResumeAutoRotate), 3f);
        }

        // 拖动旋转
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            productModel.Rotate(Vector3.up, -delta.x * rotateSpeed * Time.deltaTime);
            lastMousePosition = Input.mousePosition;
        }

        // 键盘控制
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousMaterial();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextMaterial();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetView();
        }

        // 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Camera.main.transform.Translate(Vector3.forward * scroll * 2f);
        }
    }

    /// <summary>
    /// 切换到下一材质
    /// </summary>
    public void NextMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex + 1) % materials.Length;
        ApplyMaterial(currentMaterialIndex);
    }

    /// <summary>
    /// 切换到上一材质
    /// </summary>
    public void PreviousMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex - 1 + materials.Length) % materials.Length;
        ApplyMaterial(currentMaterialIndex);
    }

    /// <summary>
    /// 应用材质
    /// </summary>
    private void ApplyMaterial(int index)
    {
        if (modelRenderers == null || materials.Length == 0) return;

        for (int i = 0; i < modelRenderers.Length; i++)
        {
            if (materials[index] != null)
            {
                modelRenderers[i].material = materials[index];
            }
        }

        string matName = materialNames != null && index < materialNames.Length 
            ? materialNames[index] 
            : $"Material_{index}";
            
        Debug.Log($"[ProductConfigurator] 切换材质: {matName}");
        
        // 通知UI更新
        if (ConfiguratorUI.Instance != null)
        {
            ConfiguratorUI.Instance.UpdateMaterialLabel(matName);
        }
    }

    /// <summary>
    /// 重置视角
    /// </summary>
    public void ResetView()
    {
        StopAllCoroutines();
        StartCoroutine(ResetViewRoutine());
    }

    private IEnumerator ResetViewRoutine()
    {
        Quaternion startRot = productModel.rotation;
        Quaternion targetRot = Quaternion.identity;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            productModel.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        autoRotate = true;
    }

    /// <summary>
    /// 恢复自动旋转
    /// </summary>
    private void ResumeAutoRotate()
    {
        autoRotate = true;
    }

    /// <summary>
    /// 自动旋转协程
    /// </summary>
    private IEnumerator AutoRotateRoutine()
    {
        while (autoRotate)
        {
            productModel.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// 获取当前配置
    /// </summary>
    public ProductConfig GetCurrentConfig()
    {
        return new ProductConfig
        {
            materialIndex = currentMaterialIndex,
            materialName = materialNames != null && currentMaterialIndex < materialNames.Length 
                ? materialNames[currentMaterialIndex] 
                : "Unknown",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}

/// <summary>
/// 产品配置数据
/// </summary>
[System.Serializable]
public class ProductConfig
{
    public int materialIndex;
    public string materialName;
    public string timestamp;
}
