using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 配置器UI控制器
/// 管理界面按钮和状态显示
/// </summary>
public class ConfiguratorUI : MonoBehaviour
{
    public static ConfiguratorUI Instance { get; private set; }

    [Header("按钮")]
    public Button prevButton;
    public Button nextButton;
    public Button resetButton;
    public Button confirmButton;

    [Header("文本显示")]
    public Text materialLabel;
    public Text configText;

    [Header("面板")]
    public GameObject confirmPanel;

    [Header("材质名称")]
    public string[] materialNames = new string[] 
    { 
        "默认白色", 
        "炫酷黑色", 
        "科技蓝色", 
        "活力橙色", 
        "奢华金色" 
    };

    private ProductConfigurator configurator;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeButtons();
        configurator = FindObjectOfType<ProductConfigurator>();
        
        if (materialLabel != null)
        {
            materialLabel.text = $"当前材质: {materialNames[0]}";
        }
    }

    /// <summary>
    /// 初始化按钮事件
    /// </summary>
    private void InitializeButtons()
    {
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPrevClick);
        }

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextClick);
        }

        if (resetButton != null)
        {
            resetButton.onClick.AddListener(OnResetClick);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmClick);
        }
    }

    /// <summary>
    /// 上一材质按钮点击
    /// </summary>
    public void OnPrevClick()
    {
        if (configurator != null)
        {
            // 获取当前索引并切换到上一个
            int currentIdx = GetCurrentMaterialIndex();
            int prevIdx = currentIdx > 0 ? currentIdx - 1 : materialNames.Length - 1;
            configurator.SetMaterial(prevIdx);
        }
        UpdateUI();
    }

    /// <summary>
    /// 下一材质按钮点击
    /// </summary>
    public void OnNextClick()
    {
        if (configurator != null)
        {
            configurator.NextMaterial();
        }
        UpdateUI();
    }

    /// <summary>
    /// 重置视角按钮点击
    /// </summary>
    public void OnResetClick()
    {
        if (configurator != null)
        {
            configurator.ResetView();
        }
    }

    /// <summary>
    /// 确认按钮点击
    /// </summary>
    public void OnConfirmClick()
    {
        if (configurator != null)
        {
            ProductConfig config = configurator.GetCurrentConfig();
            ShowConfig(config);
        }
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    private void UpdateUI()
    {
        if (materialLabel != null && configurator != null)
        {
            int idx = GetCurrentMaterialIndex();
            materialLabel.text = $"当前材质: {materialNames[idx]}";
        }
    }

    /// <summary>
    /// 更新材质标签
    /// </summary>
    public void UpdateMaterialLabel(string materialName)
    {
        if (materialLabel != null)
        {
            materialLabel.text = $"当前材质: {materialName}";
        }
    }

    /// <summary>
    /// 显示配置信息
    /// </summary>
    public void ShowConfig(ProductConfig config)
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
        }

        if (configText != null)
        {
            configText.text = $"材质: {config.materialName}\n" +
                            $"索引: {config.materialIndex}\n" +
                            $"时间: {config.timestamp}";
        }
    }

    /// <summary>
    /// 隐藏确认面板
    /// </summary>
    public void HideConfirmPanel()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 获取当前材质索引
    /// </summary>
    private int GetCurrentMaterialIndex()
    {
        // 通过反射或public字段获取
        if (configurator != null)
        {
            var field = typeof(ProductConfigurator).GetField("currentMaterialIndex", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                return (int)field.GetValue(configurator);
            }
        }
        return 0;
    }
}
