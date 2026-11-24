# PKHeX MAUI 简体中文汉化版（非官方）

本项目基于上游 PKHeX MAUI 项目整理，主要界面与常见运行时提示已翻译为简体中文。

## 安卓 APK 云端编译

1. 将本目录中的项目内容上传到你自己的 GitHub 仓库。
2. 打开仓库的 **Actions**。
3. 选择 **Build PKHeX MAUI Simplified Chinese APK**，点击 **Run workflow**。
4. 工作流成功后，在该次运行的 **Artifacts** 中下载 `PKHeXMAUI-Simplified-Chinese-APK`。
5. 解压后安装 `PKHeXMAUI-Simplified-Chinese.apk`。

详细签名说明见 [BUILD-ANDROID-APK.md](BUILD-ANDROID-APK.md)。

## 翻译范围与限制

- 已翻译主导航、常见操作、宝可梦编辑器、训练家编辑器、各世代存档编辑页、实时修改、批量编辑及常见提示。
- PKHeX.Core 依赖库提供的物种、招式、道具、特性、游戏版本等数据名称可能受其数据语言或所选游戏语言影响，仍可能显示英文。
- 少量由第三方控件或运行时数据动态生成的内容可能仍未汉化。
- 当前环境未安装 .NET MAUI Android 构建工具，尚未完成本地编译及实体设备测试。

保留原项目 LICENSE.txt 与第三方版权声明。本项目不是 PKHeX 官方版本。
