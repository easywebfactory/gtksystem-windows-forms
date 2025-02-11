# GTKSystem.Windows.Forms

### 介绍
**Visual Studio原生开发，无需学习，一次编译，跨平台运行**.
C#桌面应用程序跨平台（windows、linux、macos）界面开发组件，基于GTK组件开发，该组件的核心优势是使用C#的原生winform表单控件窗体设计器，相同的属性方法，C#原生开发即可，无需学习。一次编译，跨平台运行。
便于开发跨平台winform软件，便于将C#升级为跨平台软件。

项目官网：[https://www.gtkapp.com](https://www.gtkapp.com)   

目前功能持续更新中，将优先完善常用功能。

### 软件架构

使用DotNet Csharp为开发语言，使用GTK3.24.24.95作为表单UI，重写C#的System.Windows.Forms组件，在应用时，兼容原生C#程序组件。

### 安装教程
默认的情况下，visual studio从Nuget引用GtkSharp编译时，就会自动下载Gtk.zip运行时安装包，并自动解压安装。本开源项目下载包也包含Gtk.zip包，可手动安装。以下是三种环境安装方法：

1、安装GtkSharp后，编译你的工程项目，自动安装（此库不是最新的，有些功能可能有Bug）  
安装GtkSharp后，编译你的工程项目时，会自动下载gtk.zip解压到目录$(LOCALAPPDATA)\Gtk\3.24.24配置Gtk环境，目前国内网络限制，可能会出现无法下载的错误。
如果无法自动下载，本项目提供下载 [https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies](https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies)。
也可以下载https://github.com/GtkSharp/Dependencies，把文件解压后放到$(LOCALAPPDATA)\Gtk\3.24.24目录即可。
ps: $(LOCALAPPDATA)为电脑的AppData\Local文件夹,如：C:\Users\chj\AppData\Local\Gtk\3.24.24

2、下载exe安装包安装（建议使用此方法下载安装，获取最新库，本项目下载包已经包含此安装包） 
本项目提供下载 [https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies](https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies)。
方法1在国内可能会有网络障碍，并且是比较旧的运行时库，可能有Bug，建议用此方法获取最新版本安装：下载[https://github.com/tschoonj/GTK-for-Windows-Runtime-Environment-Installer](https://github.com/tschoonj/GTK-for-Windows-Runtime-Environment-Installer)，安装后配置电脑变量环境：
```
你可以打开电脑属性配置，或者执行以下.bat命令：
@set GTK3R_PREFIX=C:\Program Files\GTK3-Runtime Win64
@echo set PATH=%GTK3R_PREFIX%;%%PATH%%
@set PATH=%GTK3R_PREFIX%;%PATH%
```
3、使用MSYS软件平台安装，具体操作请网上查询（可以获取最新库）

windows安装DotNet环境：
```
  从微软官网下载安装包https://dotnet.microsoft.com/zh-cn/download
```

**桌面版linux操作系统通常已经预装GTK环境，不需要再安装GTK，只需安装DotNet SDK即可运行本框架。**

对于没有安装GTK环境的linux系统，可用以下命令安装：
```
#Debian/Ubuntu环境
    sudo apt install libgtk-3-0  //Binary package
    sudo apt install libgtk-3-dev //开发环境 package
#Arch环境
    sudo apt install gtk3
#Fedora	环境
    sudo apt install gtk3    //Binary package
    sudo apt install gtk3-devel  //开发环境 package

*或指定库名安装
    sudo apt-get install libgtk3*

#从MSYS2安装：
    pacman -S mingw-w64-ucrt-x86_64-gtk3

*检查环境情况（需要安装pkg-config）：
    pkg-config --cflags --libs gtk+-3.0
*查找gtk的安装包目录：
    ldconfig -p | grep gtk
```
linux安装DotNet环境：
```
  安装方法可以查看微软官网教程：https://learn.microsoft.com/zh-cn/dotnet/core/install/linux-scripted-manual
```

### 开发教程
1.  项目工程框架选择“window应用程序”改配置UseWindowsForms为false或“控制台应用程序”，.net6及以上版本
2.  NulGet安装GtkSharp(3.24.24.95)、GTKSystem.Windows.Forms、GTKSystem.Windows.FormsDesigner
3.  检查form表单是否有使用图像资源，如使用需新建System.Resources.ResourceManager和System.ComponentModel.ComponentResourceManager，具体请看下面内容。
4.  编译工程，执行本项目的开发插件菜单“修复窗体设计器”，或者手动在obj目录下创建.designer.runtimeconfig.json，请看下面第5点。

### 如何运行软件
1. windows下：直接编译发布运行，Debug目录的demo_app.exe文件或demo_app.dll文件都可以直接运行。
2. linux和macos上：执行命令运行dotnet demo_app.dll。
3. 使用本框架的工程项目也可以在linux系统上编译发布，可以生成linux系统专用文件（无后缀名的文件），此文件可以直接双击启动应用

### VisualStudio插件安装

工具一、从NuGet上安装GTKSystem.Windows.FormsDesigner类库，此类库可以在编译工程时修正窗体设计器。

工具二、下载本插件工具，关闭visual studio，直接双击GTKWinformVSIXProject.vsix文件安装（本框架下的工程，Studio没有添加Form模板项，需要安装此插件）

插件会安装两个功能：

1、新建项的Form窗体模板、用户控件模板。

2、工程右键菜单。

![输入图片说明](pic/vs_vsix.jpeg)

### 开发教程及说明

以下配置在你的项目工程里操作：

1、新建System.Resources.ResourceManager类<br/>
在项目下新建System.Resources.ResourceManager类，继承GTKSystem.Resources.ResourceManager，用于覆盖原生System.Resources.ResourceManager类。
GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
如果项目里没有使用资源图像文件，可以不用新建此文件。

2、新建System.ComponentModel.ComponentResourceManager类<br/>
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。<br/>
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
如果项目里没有使用资源图像文件，可以不用新建此文件。

3、GTKWinFormsApp.csproj<br/>
配置UseWindowsForms为false，目标OS设置为“(空)”，或者使用控制台应用程序(在控制台框架下会显示控制台窗口，不建议这种方式)
```
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <UseWindowsForms>false</UseWindowsForms>
```

4、引用GTKSystem.Windows.Forms、System.Resources.Extensions <br/>
GTKSystem.Windows.Forms是必须引用<br/>
System.Resources.Extensions是空程序dll，不是必须引用，只有VS在窗体设计器出现相关异常提示时使用

5、GTKWinFormsApp\obj\Debug\net8.0\GTKWinFormsApp.designer.runtimeconfig.json
GTKWinFormsApp\obj\Debug\net8.0\GTKWinFormsApp.runtimeconfig.json
将name设置为Microsoft.WindowsDesktop.App， **用于VS支持可视化窗体设计器，重新加载工程或重启VS** 
如以下配置：
GTKWinFormsApp.designer.runtimeconfig.json
```
{
  "runtimeOptions": {
    "tfm": "net8.0",
    "framework": {
      "name":"Microsoft.WindowsDesktop.App",
      "version": "8.0.0"
    },
    "additionalProbingPaths": [
      "C:\\Users\\chj\\.dotnet\\store\\|arch|\\|tfm|", 
      "C:\\Users\\chj\\.nuget\\packages",
      "C:\\Program Files (x86)\\Microsoft Visual Studio\\Shared\\NuGetPackages",
      "C:\\Program Files\\dotnet\\sdk\\NuGetFallbackFolder"
    ],
    "configProperties": {
      "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false,
      "Microsoft.NETCore.DotNetHostPolicy.SetAppPaths": true
    }
  }
}
```

GTKWinFormsApp.runtimeconfig.json
```
{
  "runtimeOptions": {
    "tfm": "net8.0",
    "framework": {
      "name": "Microsoft.WindowsDesktop.App",
      "version": "8.0.0"
    },
    "configProperties": {
      "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false
    }
  }
}
```

### Resources资源的使用
* [查看Resources资源的使用教程>>](Readme_Resources.md)

### 支持GTKSystem，获取技术服务

企业服务：[https://www.gtkapp.com/vipservice](https://www.gtkapp.com/vipservice)   

 ![支持GTKSystem](/pic/love_reward_qrcode_.png)
 ![联系GTKSystem](/pic/contact_weixin.png)

### 交流/合作/商务/赞助
QQ群：236066073（满），1011147488
邮箱：438865652@qq.com <br/>

### 默认风格效果
![demo效果](/pic/native-2.png)

### 默认风格工具栏菜单
![工具栏菜单](/pic/native-1.png)

### 图文窗口
![图文窗口](/pic/native-3.png)

### 支持各种主题风格界面（windows xp、vista、7、8、10，macOS系列，等等）
#### 主题风格，window10黑色风格界面
![mwindow10黑色风格界面](/pic/Windows-10-White.png)
#### 主题风格，macOS风格界面
![macOS风格界面](/pic/macOS-1.png)

### 常见问题
  为什么Form窗体设计器打不开？<br/>
  ```
  答：检查runtimeconfig确保配置正确，通过NuGet安装GTKSystem.Windows.FormsDesigner，然后按以下流程操作：
    1、编译一下 
    2、打开Form窗体
    （如果不能打开窗体，执行下面流程） 
    3、关闭Form窗体，编译一下 
    4、重启Visual Studio 
    5、打开Form窗体设计器 
 ```
### 参与贡献

1. https://gitee.com/easywebfactory
2. https://github.com/easywebfactory
3. https://blog.csdn.net/auto_toyota

### 更新记录
* [查看更新记录>>](UpdateHistory.md)