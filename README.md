# GTKSystem.Windows.Forms

### 介绍
**Visual Studio原生开发，无需学习，一次编译，跨平台运行**.
C#桌面应用程序跨平台（windows、linux、macos）开发框架，基于GTK组件开发，使用该框架开发项目，Visual studio可以使用C#的原生winform表单窗体设计器，相同的属性、方法、事件，C#原生开发即可，无需学习。一次编译，跨平台运行。 便于开发跨平台winform软件，便于将C# winform软件升级为跨平台软件。

项目官网：[https://www.gtkapp.com](https://www.gtkapp.com) 

### 软件架构

使用DotNet Csharp为开发语言，使用GTK3.24.24.95作为表单UI，重写C#的System.Windows.Forms组件，在应用时，兼容原生C#程序组件。

### 安装教程
默认的情况下，visual studio从Nuget引用GtkSharp编译时，就会自动下载Gtk.zip运行时安装包，并自动解压安装。本开源项目下载包也包含Gtk.zip包，可手动安装。以下是2种环境安装方法：

1）安装GtkSharp后，编译你的工程项目，手动从本项目下载运行时库安装，（Visual Studio开发环境必需）。
安装GtkSharp后，编译你的工程项目时，会自动下载gtk.zip解压到目录$(LOCALAPPDATA)\Gtk\3.24.24配置Gtk环境，目前国内网络限制，可能会出现无法下载的错误。
自动下载的库版本较低，本项目提供下载 [https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies](https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies)。
也可以下载https://github.com/GtkSharp/Dependencies（版本比较旧，有bug），把文件解压后放到$(LOCALAPPDATA)\Gtk\3.24.24目录即可。
ps: $(LOCALAPPDATA)为电脑的AppData\Local文件夹,如：C:\Users\chj\AppData\Local\Gtk\3.24.24

2）下载exe安装包安装
本项目提供下载 [https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies](https://gitee.com/easywebfactory/GTK-for-Windows/tree/master/Dependencies)。
获取最新版本安装：下载[https://github.com/tschoonj/GTK-for-Windows-Runtime-Environment-Installer](https://github.com/tschoonj/GTK-for-Windows-Runtime-Environment-Installer)，安装后配置电脑变量环境：
```
你可以打开电脑属性配置，或者执行以下.bat命令（自行修改安装目录）：
@set GTK3R_PREFIX=C:\Program Files\GTK3-Runtime Win64\bin
@echo set PATH=%GTK3R_PREFIX%;%%PATH%%
@set PATH=%GTK3R_PREFIX%;%PATH%
//在windows下使用环境变量，在有些情况下可能会出现异常，如：与intel wifi的zlib1.dll冲突
//英特尔的环境变量 PATH: C:\Program Files\Intel\WiFi\bin\
//解决方法：修改C:\Program Files\Intel\WiFi\bin\zlib1.dll的名称或删除该文件
//注意：修改该文件可能对intel wifi正常运行的影响
```
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
    pacman -S mingw-w64-ucrt-x86_64-gtk3 或 pacman -S mingw-w64-x86_64-gtk3

*检查环境情况（需要安装pkg-config）：
    pkg-config --cflags --libs gtk+-3.0
*查找gtk的安装包目录：
    ldconfig -p | grep gtk
```
linux安装DotNet环境：
```
  安装方法可以查看微软官网教程：https://learn.microsoft.com/zh-cn/dotnet/core/install/linux-scripted-manual
```

MacOS安装gtk环境
```
一、应用商店安装：
	brew install gtk+3

二、源码安装	
	# 克隆 Gtk-OSX 仓库（gtk-osx-master.zip解压后可本地安装）
		git clone https://gitlab.gnome.org/GNOME/gtk-osx.git
		cd gtk-osx
	# 运行安装脚本
		/gtk-osx-setup.sh
```
MacOS安装dotnet环境
```
安装方法请看微软官方教程：https://learn.microsoft.com/zh-cn/dotnet/core/install/macos
```

### 开发教程
1.  项目工程框架选择“window应用程序”，改配置\<UseWindowsForms\>为false，.net6及以上版本
2.  NulGet安装GtkSharp(3.24.24.95)、GTKSystem.Windows.Forms
3.  检查form表单是否有使用图像资源，如使用需新建System.Resources.ResourceManager和System.ComponentModel.ComponentResourceManager，具体请看下面内容。
4.  安装本下载包里的【VisualStudio开发插件】，用于添加窗体创建模板。

### 如何运行软件
1. windows下：直接编译发布运行，Debug目录的demo_app.exe文件或demo_app.dll文件都可以直接运行。
2. linux和macos上：执行命令运行dotnet demo_app.dll。
3. 通过visual studio发布成独立程序（包含环境运行时库），直接双击即可运行（可能需要授权：sudo chmod +x demoapp）。

### VisualStudio插件安装[可选装]

工具一、从NuGet上安装GTKSystem.Windows.FormsDesigner类库，此类库可以在编译工程时修正窗体设计器。

工具二、使用本项目插件工具（在项目下载包里），可以有效清理缓存、修正窗体设计器，关闭visual studio，直接双击GTKAppVSIX.vsix文件安装（本框架下的工程，Studio没有添加Form模板项，需要安装此插件）

插件会安装两个功能：

1、新建项的Form窗体模板、用户控件模板。

2、修正窗体设计器。

![输入图片说明](pic/vs_vsix.jpeg)

### 开发教程及说明

以下配置在你的项目工程里操作：

1、GTKWinFormsApp.csproj<br/>
配置\<UseWindowsForms\>为false
```
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <UseWindowsForms>false</UseWindowsForms>
```

2、引用GTKSystem.Windows.Forms <br/>
GTKSystem.Windows.Forms是必须引用

3、【可选项】新建添加配置文件Directory.Build.props，此配置是为了区分窗体设计工程的obj目录，
<br/><b>此配置主要是用于双工程方法，用C#原生窗体工程管理窗体</b>，项目演示工程即使用此方法，详细的使用教程可以访问[https://www.gtkapp.com/formsdesigner](https://www.gtkapp.com/formsdesigner)  
<br/>配置如下：
```
<Project>
	<PropertyGroup>
		<BaseIntermediateOutputPath>obj\$(MSBuildProjectName)</BaseIntermediateOutputPath>
		<IntermediateOutputPath>$(BaseIntermediateOutputPath)\$(TargetFramework)</IntermediateOutputPath>
	</PropertyGroup>
</Project>
```
 
4、【可选项】新建System.Resources.ResourceManager类<br/>
在项目下新建System.Resources.ResourceManager类，继承GTKSystem.Resources.ResourceManager，用于覆盖原生System.Resources.ResourceManager类。
GTKSystem.Resources.ResourceManager实现了项目资源文件和图像文件读取。
*如果项目里没有使用资源图像文件，可以不用新建此文件**。

5、【可选项】新建System.ComponentModel.ComponentResourceManager类<br/>
在项目下新建System.ComponentModel.ComponentResourceManager类，继承GTKSystem.ComponentModel.ComponentResourceManager，用于覆盖原生System.ComponentModel.ComponentResourceManager类。<br/>
GTKSystem.ComponentModel.ComponentResourceManager实现了项目资源文件和图像文件读取（调用GTKSystem.Resources.ResourceManager）。
*如果项目里没有使用资源图像文件，可以不用新建此文件*。


### Resources资源的使用
* [查看Resources资源的使用教程>>](Readme_Resources.md)

### 支持GTKSystem，获取技术服务

企业服务：[https://www.gtkapp.com/vipservice](https://www.gtkapp.com/vipservice)   

### 交流/合作/商务/赞助
QQ群：1011147488
邮箱：438865652@qq.com <br/>

### 默认风格效果
![demo效果](/pic/native-2.png)

### 自定义皮肤效果
![自定义皮肤](/pic/showwork1.png)


### 支持各种主题风格界面（windows xp、vista、7、8、10，macOS系列，等等）
![mwindow10风格界面](/pic/Windows-10-White.png)

### 常见问题
  为什么Form窗体设计器打不开？<br/>
  ```
  详细方法请访问[https://www.gtkapp.com/formsdesigner/](https://www.gtkapp.com/formsdesigner/)。 

  有三种方法使用窗体设计器，
  一）从NuGet安装GTKSystem.Windows.FormsDesigner，安装项目下载包里的VisualStudio插件，重新编译工程、修复窗体设计器。  
  二）切换成原生Net框架windows应用程序工程，把相关form界面文件包含进工程，即可使用窗体设计器。  
  三）切换成framework框架的windows应用工程，把相关form界面文件包含进工程，即可使用窗体设计器。 

 ```
### 参与贡献
1. https://www.gtkapp.com
2. https://gitee.com/easywebfactory
3. https://github.com/easywebfactory

### 更新记录
* [查看更新记录>>](UpdateHistory.md)