# Unity 3D HMI 配置器

基于 Unity 引擎的3D产品配置器Demo，复刻类似视频中的HMI配置器效果。

## 功能特性

- ✅ 3D模型展示
- ✅ 材质/颜色切换
- ✅ 360度旋转查看
- ✅ 鼠标/触摸交互
- ✅ 产品配置预览
- ✅ UI交互界面

## 技术栈

- Unity 2022.3+
- C#
- URP (Universal Render Pipeline)

## 项目结构

```
Assets/
├── Scripts/          # C#脚本
│   ├── ProductConfigurator.cs
│   ├── ConfiguratorUI.cs
│   ├── CameraController.cs
│   └── MaterialChanger.cs
├── Scenes/           # 场景文件
├── Prefabs/          # 预制体
├── Materials/        # 材质
├── Models/           # 3D模型
└── Shaders/          # 自定义着色器
```

## 快速开始

1. 安装 Unity Hub 和 Unity 2022.3+
2. 克隆项目
3. 用 Unity 打开项目目录
4. 打开 `Scenes/Main` 场景
5. 点击运行

## 核心脚本

### ProductConfigurator.cs
产品配置器核心逻辑，负责材质切换和模型控制。

### CameraController.cs
相机控制，实现360度旋转查看。

### MaterialChanger.cs
材质切换管理器。

## 操作说明

- **切换材质**: 点击左/右箭头按钮或按下键盘左右键
- **手动旋转**: 鼠标左键拖动 / 手指滑动
- **滚轮缩放**: 鼠标滚轮放大缩小
- **重置视角**: 点击重置按钮或按下R键

## 扩展开发

### 添加新材质
1. 在 `Materials` 文件夹创建新材质
2. 设置颜色/纹理
3. 在脚本中添加引用

### 添加新模型
1. 导入模型(.fbx/.obj)
2. 拖入 `Models` 文件夹
3. 创建预制体

## 文档

- [架构文档](#)
- [项目说明](#)

---

*创建时间: 2026-03-17*
