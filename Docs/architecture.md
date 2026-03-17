# Unity 3D HMI 配置器 - 架构文档

## 1. 项目概述

**项目名称**: unity-hmi-configurator  
**项目类型**: 3D产品配置器Demo  
**核心功能**: 3D模型展示、材质切换、360度旋转、配置预览  
**目标用户**: 车企HMI产品展示、电商3D产品预览

---

## 2. 技术架构

### 2.1 技术栈
- **引擎**: Unity 2022.3+
- **语言**: C#
- **渲染管线**: URP (Universal Render Pipeline)
- **输入系统**: New Input System
- **版本管理**: Git

### 2.2 模块划分

```
┌─────────────────────────────────────────┐
│              UI 层                       │
│  (ConfiguratorUI - 按钮、面板)           │
├─────────────────────────────────────────┤
│              业务层                       │
│  (ProductConfigurator - 配置逻辑)        │
├─────────────────────────────────────────┤
│              控制层                       │
│  (CameraController - 相机控制)           │
├─────────────────────────────────────────┤
│              引擎层                       │
│  (Unity Core - 渲染、输入、物理)          │
└─────────────────────────────────────────┘
```

---

## 3. 核心模块

### 3.1 ProductConfigurator
| 属性/方法 | 说明 |
|-----------|------|
| productModel | 产品模型Transform |
| materials | 材质数组 |
| rotateSpeed | 旋转速度 |
| NextMaterial() | 切换下一材质 |
| PreviousMaterial() | 切换上一材质 |
| ResetView() | 重置视角 |
| GetCurrentConfig() | 获取当前配置 |

### 3.2 ConfiguratorUI
| 属性/方法 | 说明 |
|-----------|------|
| prevButton/nextButton | 切换材质按钮 |
| resetButton | 重置视角按钮 |
| confirmButton | 确认按钮 |
| materialLabel | 当前材质显示 |
| ShowConfig() | 显示配置信息 |

### 3.3 CameraController
| 属性/方法 | 说明 |
|-----------|------|
| target | 观察目标 |
| distance | 距离 |
| minDistance/maxDistance | 缩放范围 |
| FocusOnTarget() | 聚焦目标 |
| ResetCamera() | 重置相机 |
| ToggleAutoRotate() | 切换自动旋转 |

---

## 4. 交互流程

```
用户输入 → Update() 
        → HandleInput() 
        → 旋转/缩放/材质切换
        → 渲染更新
```

### 4.1 旋转控制
- 鼠标左键拖动 / 手指滑动 → 旋转模型
- 鼠标右键拖动 → 旋转相机

### 4.2 缩放控制
- 鼠标滚轮 → 缩放
- 双指捏合 → 缩放（触屏）

### 4.3 材质切换
- 点击按钮 / 键盘左右键 → 切换材质
- 自动遍历所有预设材质

---

## 5. 材质系统

预置5种材质：
1. 默认白色 - 基础款
2. 炫酷黑色 - 运动款
3. 科技蓝色 - 智能款
4. 活力橙色 - 青春款
5. 奢华金色 - 旗舰款

---

## 6. 扩展计划

- [ ] 模型切换功能
- [ ] 配件选配
- [ ] 截图保存
- [ ] 配置导出JSON
- [ ] AR预览模式
- [ ] VR模式支持

---

## 7. 项目结构

```
Assets/
├── Scripts/
│   ├── ProductConfigurator.cs   # 核心配置器
│   ├── ConfiguratorUI.cs        # UI控制器
│   └── CameraController.cs      # 相机控制
├── Scenes/
│   └── Main.unity                # 主场景
├── Prefabs/
│   ├── Product.prefab           # 产品预制体
│   └── UI/
│       └── ConfigPanel.prefab   # 配置面板
├── Materials/                    # 材质
├── Models/                       # 3D模型
└── Shaders/                      # 着色器
```

---

*文档版本: v1.0.0*  
*最后更新: 2026-03-17*
