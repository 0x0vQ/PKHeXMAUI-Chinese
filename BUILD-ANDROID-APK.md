# Android APK 云端编译（已配置签名）

仓库包含 `.github/workflows/android-apk.yml`，可使用 GitHub Actions 编译并签名可安装的 APK。

## 首次构建（无需额外密钥配置）

1. 将项目内容上传到你自己的 GitHub 仓库。
2. 打开仓库的 **Actions**。
3. 选择 **Build PKHeX MAUI Simplified Chinese APK**，点击 **Run workflow**。
4. 工作流成功后，打开运行记录，在 **Artifacts** 下载 `PKHeXMAUI-Simplified-Chinese-APK`。
5. 解压下载的文件，在安卓设备上安装 `PKHeXMAUI-Simplified-Chinese.apk`。

首次构建会在 GitHub 临时运行环境中生成一次性签名密钥，APK 可侧载安装。为了安全，不会把该临时私钥作为构建产物发布。**如果下一次构建又生成了新密钥，安卓系统不允许直接覆盖安装；需要先卸载旧版再安装新版。**

## 保留更新安装能力（推荐）

如果希望后续版本可以直接覆盖安装，需要自己生成并长期保管 Android keystore，并在 GitHub 仓库的 Settings → Secrets and variables → Actions 中添加以下 Repository secrets：

- `ANDROID_KEYSTORE_BASE64`：keystore 文件的 Base64 内容
- `ANDROID_KEYSTORE_PASSWORD`：keystore 存储密码
- `ANDROID_KEY_ALIAS`：密钥别名
- `ANDROID_KEY_PASSWORD`：密钥密码

四项都设置后，工作流会优先使用该长期密钥签名。请务必备份 keystore 和密码；丢失后无法用相同签名身份发布可覆盖安装的更新。

## 注意

- 工作流目标为 `net10.0-android36.0`，使用 .NET 10、Java 21 和 Android SDK 36。
- 构建成功只代表 APK 已生成并签名，不等于已在实体手机上完成运行测试。
- 若安装时提示与已安装版本签名不一致，卸载原版本后再安装汉化版。卸载前请先备份应用数据和存档文件。
- 原项目许可证与版权声明必须保留。本汉化版为非官方修改。
